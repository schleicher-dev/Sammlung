using System;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Fluent;
using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Parsing;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Pipes;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities
{
    public class Argument<TData> : ArgumentBase, IBindableTrait<TData>
    {
        private readonly IPipeEndpoint<TData, string> _endpoint;

        public Argument(IPipeEndpoint<TData, string> endpoint)
        {
            _endpoint = endpoint.RequireNotNull(nameof(endpoint));
            this.SetArity(1);
            ParseStateMachine = new LocalParseStateMachine(this);
        }

        /// <inheritdoc />
        public override string Format(IEntityFormatter formatter) => formatter.FormatArgument(this);
        
        public void Bind(TData data) => _endpoint.Bind(data);
        
        public override IParseStateMachine ParseStateMachine { get; }
        
        private class LocalParseStateMachine : ParseStateMachineBase<Argument<TData>>
        {
            private readonly Argument<TData> _argument;
            
            public LocalParseStateMachine(Argument<TData> argument) : base(argument)
            {
                _argument = argument.RequireNotNull(nameof(argument));
                CurrentState = ParseState.RequiresNextOccurrence;
            }
            
            /// <inheritdoc />
            public override void ParseNext(string token)
            {
                switch (CurrentState)
                {
                    case ParseState.RequiresNextOccurrence:
                    case ParseState.ExpectNextOccurrence:
                    case ParseState.ExpectNextToken:
                        _argument._endpoint.PushValue(token);
                        _argument.NumArity += 1;
                        ConsiderNextState();
                        break;
                    case ParseState.Finalized:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}