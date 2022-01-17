using Microsoft.CodeAnalysis;
using Sammlung.CommandLine.SourceGenerator.Execution;

namespace Sammlung.CommandLine.SourceGenerator
{
    [Generator]
    public class CommandLineSyntaxGenerator : ISourceGenerator
    {
        public CommandLineSyntaxGenerator()
        {
            
        }
        
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var generatorExecutor = new GeneratorExecutor();
            var result = generatorExecutor.Execute(context);
            if (!result.ResultCode.DenotesSuccess)
            {
                context.WriteWarning(result.ResultCode);
                return;
            }

            foreach (var (hintName, sourceText) in result.SourceCodeFiles) 
                context.AddSource(hintName, sourceText);
        }
    }
}