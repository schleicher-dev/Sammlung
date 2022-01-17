using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Collections.Queues;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Models.Entities;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Models.Entities.Bases.Options;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Terminal;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Parsing
{
    internal class CommandParser<TData>
    {
        private readonly CommandBase _command;
        private readonly IDeque<Argument<TData>> _arguments;
        private readonly List<BindableOptionBase<TData>> _options;
        private readonly List<CommandBase> _commands;
        private IParseStateMachine _currentStateMachine;
        private IDeque<string> _tokens;
        private ITerminal _terminal;
        public TData Data { get; }

        public CommandParser(CommandBase command, TData data, List<Argument<TData>> arguments, List<BindableOptionBase<TData>> options, List<CommandBase> commands)
        {
            _command = command.RequireNotNull(nameof(command));
            _arguments = arguments.RequireNotNull(nameof(arguments)).OrderBy(a => a.Position).ToDeque();
            _options = options.RequireNotNull(nameof(options));
            _commands = commands.RequireNotNull(nameof(commands));
            Data = data;
                
            _currentStateMachine = default;
        }
        
        public TerminationInfo Parse(IEnumerable<string> args, ITerminal terminal)
        {
            _tokens = args.ToDeque();
            _terminal = terminal;
                
            while (_tokens.Count != 0)
            {
                ResetCurrentStateMachineIfNotActive();
                    
                var token = _tokens.PopLeft();

                if (TryParseHelpCommand(token) || TryParseHelpOption(token))
                    return _command.ShowHelp(_terminal);
                if (TryParseCommand(token, out var terminationInfo))
                    return terminationInfo;
                if (TryActivateOptionStateMachine(token))
                    continue;
                if (TryActivateArgumentStateMachineIfNoneActive(token))
                    continue;
                ParseNextToken(token);
            }
            
            CheckPostConditions();
            return TerminationInfo.RegularTermination;
        }


        private bool TryGetCommand(string keyword, out CommandBase command)
        {
            command = _commands.FirstOrDefault(c =>
                c.Keywords.Contains(keyword, StringComparer.InvariantCultureIgnoreCase));
            return command != null;
        }
            
        private bool TryGetOption(string keyword, out BindableOptionBase<TData> bindableOption)
        {
            bindableOption = _options.FirstOrDefault(o => o.Keywords.Contains(keyword, StringComparer.InvariantCultureIgnoreCase));
            return bindableOption != null;
        }

        private void ResetCurrentStateMachineIfNotActive()
        {
            if (_currentStateMachine == null || !_currentStateMachine.HasState(ParseState.ExpectNextToken))
                _currentStateMachine = default;
        }

        private void RequireNoActiveStateMachine(string token, IParserEntity parserEntity)
        {
            if (_currentStateMachine != null)
                throw new ParseException(
                    $"The next token is expected to be passed to another entity but the token '{token}' " +
                    $"is equivalent to a keyword of the entity '{parserEntity}'. Place the token into " +
                    "double quotes if this was intended.");
        }

        private void RequireActiveStateMachine(string token)
        {
            if (_currentStateMachine == null)
                throw new ParseException($"The token '{token}' is not treated.");
        }

        private bool TryParseHelpCommand(string token) => Reservations.CommandHelpKeywords.Contains(token);

        private bool TryParseHelpOption(string token)=> Reservations.OptionHelpKeywords.Contains(token);

        private bool TryParseCommand(string token, out TerminationInfo terminationInfo)
        {
            if (!TryGetCommand(token, out var command))
            {
                terminationInfo = null;
                return false;
            }

            RequireNoActiveStateMachine(token, command);

            terminationInfo = command.Parse(_tokens, _terminal);
            return true;
        }

        private bool TryActivateOptionStateMachine(string token)
        {
            if (!TryGetOption(token, out var option)) return false;

            RequireNoActiveStateMachine(token, option);
            _currentStateMachine = option.ParseStateMachine;
            ParseNextToken(token);
            return true;
        }

        private bool TryActivateArgumentStateMachineIfNoneActive(string token)
        {
            if (_currentStateMachine != null) return false;
            if (!_arguments.TryPeekLeft(out var argument)) return false;
                
            if (1 < _arguments.Count || argument.MaxOccurrences == 1) _arguments.PopLeft();
            _currentStateMachine = argument.ParseStateMachine;
            ParseNextToken(token);
            return true;
        }

        private void ParseNextToken(string token)
        {
            RequireActiveStateMachine(token);
            _currentStateMachine.ParseNext(token);
        }

        private void CheckPostConditions()
        {
            if (_currentStateMachine.HasState(ParseState.ExpectNextToken))
                throw new ParseException(
                    "The entity is still expecting at least one token. But the end of the tokens was reached.");
            if (_currentStateMachine.HasState(ParseState.RequiresNextOccurrence))
                throw new ParseException(
                    "The entity is still expecting at least one occurrence. But the end of the tokens was reached.");

            var stateMachines = _options.OfType<IParseTrait>().Concat(_arguments.OfType<IParseTrait>())
                .Select(t => t.ParseStateMachine);
            foreach (var stateMachine in stateMachines)
            {
                if (stateMachine.HasState(ParseState.ExpectNextToken))
                    throw new ParseException(
                        "The entity is still expecting at least one token. But the end of the tokens was reached.");
                if (stateMachine.HasState(ParseState.RequiresNextOccurrence))
                    throw new ParseException(
                        "The entity is still expecting at least one occurrence. But the end of the tokens was reached.");
            }
        }
    }
}