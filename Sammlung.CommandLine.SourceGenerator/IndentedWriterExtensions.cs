using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.SourceGenerator
{
    public static class IndentedWriterExtensions
    {
        private static void EnterScope(this IndentedTextWriter writer, int amount = 1)
        {
            writer.WriteLine("{");
            writer.Indent += amount;
        }

        private static void ExitScope(this IndentedTextWriter writer, int amount = 1)
        {
            writer.Indent -= amount;
            writer.WriteLine("}");
        }
        
        private static void EnterIndent(this IndentedTextWriter writer, int amount = 1)
        {
            writer.Indent += amount;
        }

        private static void ExitIndent(this IndentedTextWriter writer, int amount = 1)
        {
            writer.Indent -= amount;
        }
        
        public static IDisposable Scoped(this IndentedTextWriter writer, int amount = 1)
        {
            amount = amount.RequireGreaterEqual(1, nameof(amount));
            return new ScopedAction(() => writer.EnterScope(amount), () => writer.ExitScope(amount));
        }
        
        public static IDisposable Indented(this IndentedTextWriter writer, int amount = 1)
        {
            amount = amount.RequireGreaterEqual(1, nameof(amount));
            return new ScopedAction(() => writer.EnterIndent(amount), () => writer.ExitIndent(amount));
        }

        public static void WriteFluentStatements(this IndentedTextWriter writer, IEnumerable<string> statements, int amount = 2)
        {
            using (writer.Indented(amount))
            {
                foreach (var statement in statements)
                {
                    writer.WriteLine();
                    writer.Write(statement);
                }
                writer.WriteLine(";");
            }
        }
    }
}