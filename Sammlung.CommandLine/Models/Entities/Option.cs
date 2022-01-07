using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Options;
using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Parsing;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Pipes;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities
{
    public class Option<TData> : BindableOptionBase<TData>, IArityTrait
    {
        private List<string> _metaNames;
        private readonly IPipeTerminal<TData, string> _terminal;
        private int _arity = 1;

        public int Arity
        {
            get => _arity;
            set => _arity = value.RequireGreaterEqual(0, nameof(value));
        }

        public IEnumerable<string> MetaNames
        {
            get => _metaNames;
            set => _metaNames = value.RequireNotNull(nameof(value)).ToList();
        }

        public Option(IEnumerable<string> keywords, IPipeTerminal<TData, string> terminal) : base(keywords)
        {
            _terminal = terminal.RequireNotNull(nameof(terminal));
            ParseStateMachine = new LocalParseStateMachine(this);
        }

        /// <inheritdoc />
        public override string Format(IEntityFormatter formatter) => formatter.FormatOption(this);

        /// <inheritdoc />
        public override void Bind(TData data) => _terminal.Bind(data);
        
        /// <inheritdoc />
        public override IParseStateMachine ParseStateMachine { get; }

        /// <summary>
        /// The <see cref="LocalParseStateMachine"/> type implements the <see cref="IParseStateMachine"/>
        /// in place of the <see cref="Option{TData}"/> type.
        /// </summary>
        private class LocalParseStateMachine : ParseStateMachineBase<Option<TData>>
        {
            private readonly Option<TData> _option;

            /// <summary>
            /// Creates a new <see cref="LocalParseStateMachine"/> using the option.
            /// </summary>
            /// <param name="option" />
            public LocalParseStateMachine(Option<TData> option) : base(option)
            {
                _option = option.RequireNotNull(nameof(option));
                CurrentState = _option.MaxOccurrences == 0 ? ParseState.Finalized : ParseState.ExpectNextOccurrence;
            }

            /// <inheritdoc />
            public override void ParseNext(string token)
            {
                switch (CurrentState)
                {
                    case ParseState.RequiresNextOccurrence:
                    case ParseState.ExpectNextOccurrence:
                        ConsiderNextState();
                        break;
                    case ParseState.ExpectNextToken:
                        _option._terminal.ExecuteAll(token);
                        _numArity += 1;
                        ConsiderNextState();
                        break;
                    case ParseState.Finalized:
                        throw new ParseException($"Cannot parse a token '{token}' in state '{ParseState.Finalized}'.");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}