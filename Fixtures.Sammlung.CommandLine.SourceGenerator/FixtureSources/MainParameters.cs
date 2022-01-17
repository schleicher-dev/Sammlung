using Sammlung.CommandLine.Attributes;

namespace Fixtures.Sammlung.CommandLine.SourceGenerator.FixtureSources
{
    [CommandLineInterface("MyUniqueApplicationName", Namespace = "Hello")]
    [Description("RootCommandDescription", "This is the default description.")]
    public class MainParameters
    {
        
    }
}