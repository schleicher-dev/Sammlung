using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Sammlung.CommandLine.Attributes;
using Sammlung.CommandLine.SourceGenerator.Execution;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.SourceGenerator
{
    public class SourceCodeBuilder
    {
        private readonly ClassDeclarationSyntax _node;
        private readonly ISymbol _symbol;
        private readonly SemanticModel _model;
        
        private IndentedTextWriter _writer;
        private CommandLineInterfaceAttribute _cliAttr;
        private string _paramsTypeName;
        private (IPropertySymbol symbol, ArgumentAttribute Attribute)[] _arguments;
        private string _className;

        private static string GetCommentSummary(ISymbol symbol)
        {
            var commentXml = symbol.GetDocumentationCommentXml();
            if (commentXml == null) return null;
            
            var document = new XmlDocument();
            document.LoadXml(commentXml);
            return document.CreateNavigator()?.SelectSingleNode("//summary")?.InnerXml?.Trim();
        }

        public SourceCodeBuilder(ClassDeclarationSyntax node, ISymbol symbol, SemanticModel model)
        {
            _node = node.RequireNotNull(nameof(node));
            _symbol = symbol.RequireNotNull(nameof(symbol));
            _model = model.RequireNotNull(nameof(model));
        }
        
        public SourceCodeFile ConstructCommandLineInterface()
        {
            _cliAttr = _symbol.GetAttributes().CreateAttributeOrDefault<CommandLineInterfaceAttribute>();
            if (_cliAttr == null) return null;
            
            var textWriter = new StringWriter();
            _writer = new IndentedTextWriter(textWriter);
            
            WriteNamespace();

            var hintText = _symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sourceText = SourceText.From(textWriter.ToString());
            return new SourceCodeFile(hintText, sourceText);
        }
        
        private void WriteNamespace()
        {
            var namespaceName = _cliAttr.Namespace ?? _symbol.ContainingNamespace?.ToDisplayString();

            if (namespaceName == null)
            {
                WriteClass();
                return;
            }
            
            _writer.WriteLine($"namespace {namespaceName}");
            using (_writer.Scoped())
            {
                WriteClass();
            }
        }
        
        private void WriteClass()
        {
            var className = _symbol.Name;
            var accessibilityToken = _symbol.DeclaredAccessibility switch
            {
                Accessibility.Private => "private",
                Accessibility.ProtectedAndInternal => "protected internal",
                Accessibility.Protected => "protected",
                Accessibility.Internal => "internal",
                Accessibility.ProtectedOrInternal => "protected internal",
                Accessibility.Public => "public",
                Accessibility.NotApplicable => throw new ArgumentException("Not applicable"),
                _ => throw new ArgumentOutOfRangeException()
            };

            _className = $"{className}CommandLineFactory";
            _paramsTypeName = _symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            
            _writer.WriteLine($"{accessibilityToken} class {_className}");
            using (_writer.Scoped())
            {
                WriteConstructor();
                WriteCreateMethods();
            }
        }

        private IEnumerable<(IPropertySymbol symbol, T Attribute)> GetEntities<T>() where T : Attribute
        {
            foreach (var syntax in _node.ChildNodes().OfType<PropertyDeclarationSyntax>())
            {
                var propSymbol = _model.GetDeclaredSymbol(syntax);

                var attr = propSymbol?.GetAttributes().CreateAttributeOrDefault<T>();
                // TODO: Check if defined as option or command. At most one can be not null.
                if (attr != null) yield return (propSymbol, attr);
            }
        }

        private void WriteConstructor()
        {
            _writer.WriteLine($"private IPipeFactory _pipeFactory;");
            _writer.WriteLine($"public {_className}(IPipeFactory pipeFactory)");
            using (_writer.Scoped())
            {
                _writer.WriteLine("_pipeFactory = pipeFactory ?? throw new ArgumentNullException(nameof(pipeFactory));");
            }
        }

        private void WriteCreateMethods()
        {
            _arguments = GetEntities<ArgumentAttribute>().ToArray();
            
            WriteCreateRootCommandMethod();
            WriteCreateArgumentsMethodsIfNeeded();
        }

        private void WriteCreateRootCommandMethod()
        {
            _writer.WriteLine($"public virtual RootCommand<{_paramsTypeName}> Create()");
            using (_writer.Scoped())
            {
                _writer.Write($"return RootCommandFactory.Create<{_paramsTypeName}>()");
                _writer.WriteFluentStatements(GetRootCommandFluentStatements());
            }
        }

        private void WriteCreateArgumentsMethodsIfNeeded()
        {
            if (!_arguments.Any()) return;
            
            _writer.WriteLine("protected virtual IEnumerable<ArgumentBase> CreateArguments()");
            using (_writer.Scoped())
            {
                foreach (var (symbol, attribute) in _arguments)
                    _writer.WriteLine($"yield return Create{symbol.Name}Argument();");
            }

            foreach (var (symbol, attribute) in _arguments)
            {
                _writer.WriteLine($"protected virtual Argument<{_paramsTypeName}> Create{symbol.Name}Argument()");
                using (_writer.Scoped())
                {
                    if (symbol.GetMethod == null) throw new Exception("Missing get method");
                    if (symbol.SetMethod == null) throw new Exception("Missing set method");

                    var initialPipe = attribute.ConversionPipe != null
                        ? $"new global::{attribute.ConversionPipe.FullName}()"
                        : GetPipeConstruction(symbol.Type);
                    _writer.Write($"return {initialPipe}");
                    _writer.WriteFluentStatements(GetArgumentFluentStatements(symbol, attribute));
                }
            }
        }

        private string GetPipeConstruction(ITypeSymbol typeSymbol)
        {
            string PipeWithPrefix(string prefix) => $"_pipeFactory.{prefix}Pipe()";
            
            var typeName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            return typeName switch
            {
                "bool" => PipeWithPrefix(typeSymbol.Name),
                "short" => PipeWithPrefix(typeSymbol.Name),
                "ushort" => PipeWithPrefix(typeSymbol.Name),
                "int" => PipeWithPrefix(typeSymbol.Name),
                "uint" => PipeWithPrefix(typeSymbol.Name),
                "long" => PipeWithPrefix(typeSymbol.Name),
                "ulong" => PipeWithPrefix(typeSymbol.Name),
                "float" => PipeWithPrefix(typeSymbol.Name),
                "double" => PipeWithPrefix(typeSymbol.Name),
                "string" => PipeWithPrefix(typeSymbol.Name),
                _ => throw new ArgumentOutOfRangeException($"No mapping for type '{typeName}'")
            };
        }

        private IEnumerable<string> GetRootCommandFluentStatements()
        {
            var appName = _cliAttr.ApplicationName ?? Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
            yield return $".SetApplicationName(\"{appName}\")";
            
            foreach (var statement in GetCommandFluentStatements(_symbol.GetAttributes()))
                yield return statement;
        }

        private IEnumerable<string> GetCommandFluentStatements(IEnumerable<AttributeData> attributeData)
        {
            if (TryGetDescriptionStatement(attributeData, out var description)) yield return description;
            if (_arguments.Any()) yield return ".AddArguments(CreateArguments())";
            yield return ".AddOptions(CreateOptions())";
            yield return ".AddFlags(CreateFlags())";
        }

        private IEnumerable<string> GetArgumentFluentStatements(IPropertySymbol symbol, ArgumentAttribute attribute)
        {
            var typeName = symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            yield return $".AsPipeTerminal(({_paramsTypeName} parameters, {typeName} value) => " +
                         $"parameters.{symbol.Name} = value)";
            yield return ".BuildArgument()";
            yield return $".SetArity({attribute.Arity})";
            if (attribute.MetaNames != null && attribute.MetaNames.Any())
                yield return $".SetMetaNames({string.Join(", ", attribute.MetaNames.Select(n => $"\"{n}\""))})";
            if (TryGetDescriptionStatement(symbol.GetAttributes(), out var description)) yield return description;
        }

        private bool TryGetDescriptionStatement(IEnumerable<AttributeData> attributeData, out string description)
        {
            var descriptionAttr = attributeData.CreateAttributeOrDefault<DescriptionAttribute>();
            if (descriptionAttr != null)
            {
                description = $".SetDescription(\"{descriptionAttr.DefaultDescription}\")";
                return true;
            }

            description = null;
            return false;
        }
    }
}

