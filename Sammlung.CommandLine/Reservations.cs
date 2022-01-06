using System.Collections.Generic;

namespace Sammlung.CommandLine
{
    public static class Reservations
    {
        private static readonly List<string> InternalCommandKeywords = new List<string> { "help" };
        private static readonly List<string> InternalOptionKeywords = new List<string> { "h", "help" };

        public static IEnumerable<string> CommandKeywords => InternalCommandKeywords;
        public static IEnumerable<string> OptionKeywords => InternalOptionKeywords;
    }
}