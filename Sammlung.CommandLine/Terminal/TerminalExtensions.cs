using System.Collections.Generic;
using System.Linq;

namespace Sammlung.CommandLine.Terminal
{
    internal static class TerminalExtensions
    {
        public static void WriteLine(this IOutputWriter writer) => writer.Write(writer.NewLine);
        
        public static void Write(this IOutputWriter writer, params string[] values) => writer.Write(values.AsEnumerable());
        public static void Write(this IOutputWriter writer, IEnumerable<string> values) => writer.Write(string.Join(" ", values.ToArray()));
        public static void WriteLine(this IOutputWriter writer, string content) => writer.Write($"{content}{writer.NewLine}");
        public static void WriteLine(this IOutputWriter writer, params string[] values) => writer.WriteLine(values.AsEnumerable());
        public static void WriteLine(this IOutputWriter writer, IEnumerable<string> values) => writer.WriteLine(string.Join(" ", values.ToArray()));
    }
}