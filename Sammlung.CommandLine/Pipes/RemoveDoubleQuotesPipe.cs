using System.Text.RegularExpressions;
using Sammlung.Pipes;

namespace Sammlung.CommandLine.Pipes
{
    public class RemoveDoubleQuotesPipe : IUnDiPipe<string, string>
    {
        public string Process(string input) => Regex.Replace(input, "^\"(.*)\"$", "$1");
    }
}