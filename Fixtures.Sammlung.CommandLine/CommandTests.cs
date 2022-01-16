using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NUnit.Framework;
using Sammlung.CommandLine;
using Sammlung.CommandLine.Models.Entities;
using Sammlung.CommandLine.Models.Entities.Factories;
using Sammlung.CommandLine.Models.Fluent;
using Sammlung.CommandLine.Models.Parsing;
using Sammlung.CommandLine.Pipes;
using Sammlung.CommandLine.Reflection;

namespace Fixtures.Sammlung.CommandLine
{
    [ExcludeFromCodeCoverage]
    public class Parameters
    {
        public bool Flag { get; set; }
        public string Hello { get; set; }
        public string World { get; set; }
        public SubParameters SubParameters { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SubParameters
    {
        
    }
    
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class CommandTests
    {
        private IPipeFactory _pipeFactory;


        [SetUp]
        public void SetUp()
        {
            _pipeFactory = new PipeFactory();
        }

        private RootCommand<Parameters> CreateRoot()
        {
            var flag = _pipeFactory.BoolPipe(CultureInfo.InvariantCulture)
                .AsPipeTerminal((Parameters p) => p.Flag)
                .BuildFlag("-f", "--force");

            var stringOption = _pipeFactory.StringPipe()
                .AsPipeTerminal((Parameters p) => p.Hello)
                .BuildOption("-s", "--string")
                .SetMultiplicity(0, default);

            var argument = _pipeFactory.StringPipe()
                .AsPipeTerminal((Parameters p) => p.World)
                .BuildArgument().SetArity(2).SetMetaNames("FirstName", "SecondName");

            var cmdProperty = PropertyFactory.Property((Parameters p) => p.SubParameters);
            var command = new Command<Parameters, SubParameters>(new [] {"first_command"}, () => new SubParameters(), cmdProperty);
            
            return RootCommandFactory.Create<Parameters>()
                .SetApplicationName("HalloWelt")
                .AddFlag(flag)
                .AddOption(stringOption)
                .AddArgument(argument)
                .AddCommand(command);
        }

        [TestCase("A", "B", "-f")]
        [TestCase("A", "B", "--force")]
        public void Flag_IsGettingSet(params string[] args)
        {
            var command = CreateRoot();
            var result = command.Parse(args);
            Assert.AreEqual(TerminationInfo.RegularTermination, result);
            Assert.AreEqual(true, command.Data.Flag);
        }

        private const string LionelRichieHello = "Hello, is it me you're looking for?";
        
        [TestCase("\"" + LionelRichieHello + "\"", "A", "B", "-s", "\"\\\"" + LionelRichieHello + "\\\"\"")]
        public void Hello_IsGettingSet(string expectation, params string[] args)
        {
            var command = CreateRoot();
            var result = command.Parse(args);
            Assert.AreEqual(TerminationInfo.RegularTermination, result);
            Assert.AreEqual(expectation, command.Data.Hello);
        }

        [TestCase("-h")]
        [TestCase("A", "B", "first_command", "--help")]
        [TestCase("help")]
        public void ShowHelp(params string[] args)
        {
            var command = CreateRoot();
            var result = command.Parse(args);
            Assert.AreEqual(TerminationInfo.DisplayingHelp, result);
        }
    }
}