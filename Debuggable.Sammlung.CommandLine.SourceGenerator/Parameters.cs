using System.Collections.Generic;
using Sammlung.CommandLine.Attributes;

namespace Debuggable.Sammlung.CommandLine.SourceGenerator
{
    [CommandLineInterface("MyUniqueCommandName")]
    [Description("RootCommandDescription", "This is the default description.")]
    internal class Parameters
    {
        [Argument(Position = 0, Arity = 2, MetaNames = new [] {"A", "B"})]
        public int InputPath { get; set; }
    }
}