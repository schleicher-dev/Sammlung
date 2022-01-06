using System.Globalization;
using NUnit.Framework;
using Sammlung.CommandLine;
using Sammlung.CommandLine.Models;
using Sammlung.CommandLine.Models.Entities;
using Sammlung.CommandLine.Models.Entities.Factories;
using Sammlung.CommandLine.Models.Fluent;
using Sammlung.CommandLine.Pipes;

namespace _Fixtures.Sammlung.CommandLine
{
    [ExcludeFromCodeCoverage]
    public class Parameters
    {
        public bool Flag { get; set; }
        public string Hello { get; set; }
        public string World { get; set; }
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
                .AsPipeEndpoint((Parameters p, bool v) => p.Flag = v)
                .BuildFlag("-f", "--force");

            var stringOption = _pipeFactory.StringPipe()
                .AsPipeEndpoint((Parameters p, string v) => p.Hello = v)
                .BuildOption("-s", "--string")
                .SetMultiplicity(0, default);

            var argument = _pipeFactory.StringPipe()
                .AsPipeEndpoint((Parameters p, string v) => p.World = v)
                .BuildArgument().SetArity(2).SetMetaNames("FirstName", "SecondName");

            var command = new Command<SubParameters>(new [] {"first_command"}, () => new SubParameters());
            
            return RootCommandFactory.Create<Parameters>()
                .SetApplicationName("HalloWelt")
                .AddFlag(flag)
                .AddOption(stringOption)
                .AddArgument(argument)
                .AddCommand(command);
        }

        [TestCase("-f")]
        [TestCase("--force")]
        public void Flag_IsGettingSet(params string[] args)
        {
            var command = CreateRoot();
            var result = command.Parse(args);
            Assert.AreEqual(TerminationInfo.RegularTermination, result);
            Assert.AreEqual(true, command.Data.Flag);
        }

        private const string LionelRichieHello = "Hello, is it me you're looking for?";
        
        [TestCase(LionelRichieHello, "-s", "\"" + LionelRichieHello + "\"")]
        public void Hello_IsGettingSet(string expectation, params string[] args)
        {
            var command = CreateRoot();
            var result = command.Parse(args);
            Assert.AreEqual(TerminationInfo.RegularTermination, result);
            Assert.AreEqual(expectation, command.Data.Hello);
        }

        [TestCase]
        public void ShowHelp(params string[] args)
        {
            var command = CreateRoot();
            command.ShowHelp();
        }
    }
}