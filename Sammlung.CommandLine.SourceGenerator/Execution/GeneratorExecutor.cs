using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.SourceGenerator.Execution
{
    public class GenerationResult
    {
        private readonly List<SourceCodeFile> _sourceCodeFiles;
        public ResultCode ResultCode { get; }
        public IEnumerable<SourceCodeFile> SourceCodeFiles => _sourceCodeFiles;

        public GenerationResult(ResultCode resultCode, params SourceCodeFile[] sourceCodeFiles) : this(resultCode, sourceCodeFiles.AsEnumerable())
        {
        }
        
        public GenerationResult(ResultCode resultCode, IEnumerable<SourceCodeFile> sourceCodeFiles)
        {
            ResultCode = resultCode.RequireNotNull(nameof(resultCode));
            _sourceCodeFiles = new List<SourceCodeFile>(sourceCodeFiles.RequireNotNull(nameof(sourceCodeFiles)));
        }
    }

    public class SourceCodeFile
    {
        public string HintName { get; }
        public SourceText SourceText { get; }

        public SourceCodeFile(string hintName, SourceText sourceText)
        {
            HintName = hintName.RequireNotNullOrEmpty(nameof(hintName));
            SourceText = sourceText.RequireNotNull(nameof(sourceText));
        }

        public void Deconstruct(out string hintName, out SourceText sourceText)
        {
            hintName = HintName;
            sourceText = SourceText;
        }
    }
    
    public class GeneratorExecutor
    {
        private static IEnumerable<SourceCodeFile> AnalyzeSyntaxTree(Compilation compilation, SyntaxTree syntaxTree)
        {
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            foreach (var diagnostic in semanticModel.GetDiagnostics()) Console.WriteLine(diagnostic);

            var analyzer = new SyntaxAnalyzer(semanticModel);
            return analyzer.Visit(syntaxTree.GetRoot()) ?? Enumerable.Empty<SourceCodeFile>();
        }

        private static IEnumerable<SourceCodeFile> AnalyzeCompilation(Compilation compilation) => 
            compilation.SyntaxTrees.SelectMany(t => AnalyzeSyntaxTree(compilation, t));

        public GenerationResult Execute(Compilation compilation)
        {
            var sourceCodeFiles = AnalyzeCompilation(compilation);
            return new GenerationResult(ResultCode.RegularTermination, sourceCodeFiles);
        }

        internal GenerationResult Execute(GeneratorExecutionContext context) => 
            Execute(context.Compilation);
    }
}