using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Terminal;
using Sammlung.CommandLine.Utilities;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities.Bases.Commands
{
    /// <summary>
    /// The <see cref="CommandBase"/> type is the root type for all commands associated with the command line.
    /// </summary>
    public abstract class CommandBase : IParserEntity, IKeywordsTrait
    {
        protected static void RequireUniqueNames(IEnumerable<string> knownKeywords, IList<string> newKeywords)
        {
            var duplicates = knownKeywords
                .Where(n => newKeywords.Contains(n, StringComparer.InvariantCultureIgnoreCase))
                .ToList();

            if (!duplicates.Any()) return;

            var exceptions =
                duplicates.Select(kw => new ArgumentException($"Found a duplicate keyword '{kw}'")).ToList();
            if (exceptions.Count == 1)
                throw exceptions[0];
            throw new AggregateException(exceptions);
        }
        
        private readonly List<string> _keywords;
        public CommandBase Parent { get; private set; }
        public List<CommandBase> Commands { get; } = new List<CommandBase>();

        public IEnumerable<string> Keywords => _keywords;

        protected CommandBase(IEnumerable<string> keywords)
        {
            _keywords = keywords.RequireNotNull(nameof(keywords)).ToList();
        }

        internal void PushCommand(CommandBase command)
        {
            command = command.RequireNotNull(nameof(command));
            if (command.Parent != null)
                throw new InvalidOperationException("Cannot add command which already has a parent.");
                    
            var knownKeywords = Commands.SelectMany(c => c.Keywords).Concat(Reservations.CommandKeywords);
            RequireUniqueNames(knownKeywords, command.Keywords.ToList());
            
            Commands.Add(command);
            command.Parent = this;
        }
        
        public TerminationInfo ShowHelp(ITerminal terminal = null, Exception ex = null)
        {
            terminal ??= new DefaultTerminal();
            var helpVisualizer = new HelpVisualizer(terminal.Message);
            return ShowHelp(helpVisualizer, ex);
        }

        public abstract TerminationInfo ShowHelp(HelpVisualizer helpVisualizer, Exception ex = null);

        public abstract TerminationInfo Parse(IEnumerable<string> args, ITerminal terminal = null);
        
        public string Format(IEntityFormatter formatter) => formatter.FormatCommand(this);
        public string Description { get; set; }
    }
}