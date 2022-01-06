using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Collections.Queues;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Models.Parsing;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Terminal;
using Sammlung.CommandLine.Utilities;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities.Bases
{
    public abstract class BindableCommandBase<TData> : CommandBase, IBindableTrait<TData>
    {
        public delegate TData ConstructorDelegate();
        
        private readonly ConstructorDelegate _dataConstructor;
        
        public TData Data { get; private set; }
        public List<BindableOptionBase<TData>> Options { get; } = new();
        public List<Argument<TData>> Arguments { get; } = new();
        
        public BindableCommandBase(IEnumerable<string> keywords, ConstructorDelegate dataConstructor) : base(keywords)
        {
            _dataConstructor = dataConstructor.RequireNotNull(nameof(dataConstructor));
        }

        internal void PushArgument(Argument<TData> argument) => Arguments.Add(argument);

        internal void PushOption(BindableOptionBase<TData> bindableOption)
        {
            var knownKeywords = Options.SelectMany(o => o.Keywords).Concat(Reservations.OptionKeywords);
            RequireUniqueNames(knownKeywords, bindableOption.Keywords.ToList());
            Options.Add(bindableOption);
        }

        protected bool TryGetOption(string keyword, out BindableOptionBase<TData> bindableOption)
        {
            bindableOption = Options.FirstOrDefault(o => o.Keywords.Contains(keyword, StringComparer.InvariantCultureIgnoreCase));
            return bindableOption != null;
        }

        public override TerminationInfo ShowHelp(HelpVisualizer helpVisualizer, Exception ex = null)
        {
            helpVisualizer = helpVisualizer.RequireNotNull(nameof(helpVisualizer));
            helpVisualizer.ShowException(ex);
            helpVisualizer.ShowHelp(this);
            return TerminationInfo.DisplayingHelp;
        }

        private static void RequireNoActiveStateMachine(IParseStateMachine stateMachine, string token, EntityBase entity)
        {
            if (stateMachine == null) return;

            throw new ParseException(
                $"The next token is expected to be passed to another entity but the token '{token}' " +
                $"is equivalent to the entity '{entity}'. Place the token into double quotes if this was intended.");
        }

        public override TerminationInfo Parse(IEnumerable<string> args, ITerminal terminal = null)
        {
            Bind(Data = _dataConstructor.Invoke());
            
            var tokens = args.ToDeque();

            var arguments = Arguments.ToDeque();
            var currentStateMachine = default(IParseStateMachine);

            while (tokens.Count != 0)
            {
                var token = tokens.PopLeft();

                // Reset the current state machine.
                if (!currentStateMachine.HasState(ParseState.ExpectNextToken))
                    currentStateMachine = default;

                if (TryGetCommand(token, out var command))
                {
                    RequireNoActiveStateMachine(currentStateMachine, token, command);
                    
                    var result = command.Parse(tokens, terminal);
                    return result;
                }

                if (TryGetOption(token, out var option))
                {
                    RequireNoActiveStateMachine(currentStateMachine, token, option);
                    currentStateMachine = option.ParseStateMachine;
                    currentStateMachine.ParseNext(token);
                    continue;
                }

                if (currentStateMachine == null && arguments.TryPeekLeft(out var argument))
                {
                    if (1 < arguments.Count) arguments.PopLeft();
                    currentStateMachine = argument.ParseStateMachine;
                    currentStateMachine.ParseNext(token);
                    continue;
                }

                if (currentStateMachine == null)
                    throw new ParseException($"The token '{token}' is not treated.");
                
                currentStateMachine.ParseNext(token);
            }

            if (currentStateMachine.HasState(ParseState.ExpectNextToken))
                throw new ParseException("The entity is still expecting at least one token. But the end of the tokens was reached.");
            if (currentStateMachine.HasState(ParseState.RequiresNextOccurrence))
                throw new ParseException("The entity is still expecting ad least one occurrence. But the end of the tokens was reached.");
            
            // TODO: Check if all arguments were supplied.

            return TerminationInfo.RegularTermination;
        }

        public void Bind(TData data)
        {
            foreach (var option in Options)
                option.Bind(data);
            foreach (var argument in Arguments)
                argument.Bind(data);
        }
    }
}