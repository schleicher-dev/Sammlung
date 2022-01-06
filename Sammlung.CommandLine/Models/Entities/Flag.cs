using System;
using System.Collections.Generic;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Parsing;
using Sammlung.CommandLine.Pipes;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities
{
    /// <summary>
    /// A <see cref="Flag{TData}"/> is a special <see cref="BindableOptionBase{TData}"/> which does also have keywords
    /// but the keywords cannot expect any arguments.
    /// </summary>
    /// <typeparam name="TData">the data type</typeparam>
    public class Flag<TData> : BindableOptionBase<TData>
    {
        private readonly IPipeEndpoint<TData, string> _endpoint;

        /// <summary>
        /// Creates a new <see cref="Flag{TData}"/> using the keywords and the <see cref="IPipeEndpoint{TData, T}"/>
        /// </summary>
        /// <param name="keywords">the keywords</param>
        /// <param name="endpoint">the endpoint of the pipe</param>
        public Flag(IEnumerable<string> keywords, IPipeEndpoint<TData, string> endpoint) : base(keywords)
        {
            _endpoint = endpoint.RequireNotNull(nameof(endpoint));
            AssignMultiplicity(0, 1);
            ParseStateMachine = new LocalParseStateMachine(this);
        }

        /// <inheritdoc />
        public override string Format(IEntityFormatter formatter) => formatter.FormatFlag(this);
        
        /// <inheritdoc />
        public override void Bind(TData data) => _endpoint.Bind(data);

        public override IParseStateMachine ParseStateMachine { get; }
        
        /// <summary>
        /// The <see cref="LocalParseStateMachine"/> type implements the <see cref="IParseStateMachine"/>
        /// in place of the <see cref="Option{TData}"/> type.
        /// </summary>
        private class LocalParseStateMachine : IParseStateMachine
        {
            private readonly Flag<TData> _flag;
            
            /// <inheritdoc />
            public ParseState CurrentState { get; private set; }

            /// <summary>
            /// Creates a new <see cref="LocalParseStateMachine"/> using the option.
            /// </summary>
            /// <param name="flag" />
            public LocalParseStateMachine(Flag<TData> flag)
            {
                _flag = flag.RequireNotNull(nameof(flag));
                CurrentState = _flag.MinOccurrences <= 0 ? ParseState.ExpectNextOccurrence : ParseState.RequiresNextOccurrence;
            }

            /// <inheritdoc />
            public void ParseNext(string token)
            {
                switch (CurrentState)
                {
                    case ParseState.RequiresNextOccurrence:
                    case ParseState.ExpectNextOccurrence:
                        _flag._endpoint.PushValue(bool.TrueString);
                        ConsiderNextState();
                        break;
                    case ParseState.ExpectNextToken:
                    case ParseState.Finalized:
                        throw new ParseException($"Cannot parse a token '{token}' in state '{CurrentState}' for the given entity.");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private void ConsiderNextState()
            {
                switch (CurrentState)
                {
                    case ParseState.RequiresNextOccurrence:
                    case ParseState.ExpectNextOccurrence:
                        _flag.NumOccurrences += 1;
                        if (_flag.NumOccurrences < _flag.MinOccurrences)
                            CurrentState = ParseState.RequiresNextOccurrence;
                        else if (_flag.NumOccurrences < _flag.MaxOccurrences)
                            CurrentState = ParseState.ExpectNextOccurrence;
                        else
                            CurrentState = ParseState.Finalized;
                        break;
                    case ParseState.ExpectNextToken:
                    case ParseState.Finalized:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}