using System.Collections.Generic;

namespace Sammlung.CommandLine
{
    public static class Reservations
    {
        private static readonly List<string> InternalCommandHelpKeywords = new List<string> { "help" };
        private static readonly List<string> InternalOptionHelpKeywords = new List<string> { "-h", "--help", "-?" };

        public static IEnumerable<string> CommandHelpKeywords => InternalCommandHelpKeywords;
        public static IEnumerable<string> OptionHelpKeywords => InternalOptionHelpKeywords;
    }
}