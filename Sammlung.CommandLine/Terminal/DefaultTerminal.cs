using System;
using System.IO;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Terminal
{
    public class DefaultTerminal : ITerminal
    {
        public IOutputWriter Pipe { get; } = new ConsoleWriter(Console.Out);
        public IOutputWriter Message { get; } = new ConsoleWriter(Console.Error);

        private class ConsoleWriter : IOutputWriter
        {
            private readonly TextWriter _textWriter;
            public string NewLine => Environment.NewLine;

            public ConsoleWriter(TextWriter textWriter)
            {
                _textWriter = textWriter.RequireNotNull(nameof(textWriter));
            }
            
            public void Write(string content) => _textWriter.Write(content);
        }
    }
    
}