using Microsoft.CodeAnalysis;

namespace Sammlung.CommandLine.SourceGenerator
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor GeneratorDescriptor =
            new DiagnosticDescriptor("GEN0001", "ABCD", "{0}", "Generator", DiagnosticSeverity.Info, true);
    }
}