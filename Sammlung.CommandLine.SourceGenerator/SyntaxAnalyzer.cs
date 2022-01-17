using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sammlung.CommandLine.SourceGenerator.Execution;

namespace Sammlung.CommandLine.SourceGenerator
{

    public class SyntaxAnalyzer : CSharpSyntaxVisitor<IEnumerable<SourceCodeFile>>
    {
        private readonly SemanticModel _semanticModel;
        private string _namespaceName;

        public SyntaxAnalyzer(SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
        }

        public override IEnumerable<SourceCodeFile> DefaultVisit(SyntaxNode node) =>
            node?.ChildNodes().SelectMany(n => Visit(n) ?? Enumerable.Empty<SourceCodeFile>());

        public override IEnumerable<SourceCodeFile> VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            _namespaceName = node.Name.ToFullString();
            return base.VisitNamespaceDeclaration(node);
        }
        
        public override IEnumerable<SourceCodeFile> VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(node) ??
                         throw new NullReferenceException($"Could not resolve symbol for class '{node.Identifier}'");
            var builder = new SourceCodeBuilder(node, symbol, _semanticModel);
            
            var sourceCodeFile = builder.ConstructCommandLineInterface();
            if (sourceCodeFile != null) yield return sourceCodeFile;
        }
    }
}