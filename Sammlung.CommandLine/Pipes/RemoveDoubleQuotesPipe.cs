using System.Text.RegularExpressions;
using Sammlung.Pipes;

namespace Sammlung.CommandLine.Pipes
{
    /// <summary>
    /// The <see cref="RemoveDoubleQuotesPipe"/> removes double quotes during the process.
    /// </summary>
    public class RemoveDoubleQuotesPipe : IUnDiPipe<string, string>
    {
        /// <inheritdoc />
        public string Process(string input)
        {
            var match = Regex.Match(input, "^\"\\s*(?<content>.*)\\s*\"$");
            return match.Success ? Regex.Unescape(match.Groups["content"].Value) : input;
        }
    }
}