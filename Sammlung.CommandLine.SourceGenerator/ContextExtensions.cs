using Microsoft.CodeAnalysis;

namespace Sammlung.CommandLine.SourceGenerator
{
    public static class ContextExtensions
    {
        public static void WriteWarning(this GeneratorExecutionContext context, params object[] messages)
        {
            var diagnostic = Diagnostic.Create(DiagnosticDescriptors.GeneratorDescriptor, null, DiagnosticSeverity.Warning, null, null, messages);
            context.ReportDiagnostic(diagnostic);
        }
    }
}