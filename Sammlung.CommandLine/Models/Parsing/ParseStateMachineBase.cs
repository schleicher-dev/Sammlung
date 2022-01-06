using System;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Parsing
{
    /// <summary>
    /// The <see cref="ParseStateMachineBase{T}"/> is the least common divisor of all the state machines.
    /// </summary>
    internal abstract class ParseStateMachineBase<T> : IParseStateMachine where T : class, IArityTrait, IMultiplicityTrait
    {
        private readonly T _entity;
        public ParseState CurrentState { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="ParseStateMachineBase{T}"/> using the demanded entity.
        /// </summary>
        /// <param name="entity"></param>
        protected ParseStateMachineBase(T entity)
        {
            _entity = entity.RequireNotNull(nameof(entity));
        }
        
        /// <inheritdoc />
        public abstract void ParseNext(string token);
        
        /// <summary>
        /// Considers the next state to reach.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if the state is out of range</exception>
        protected void ConsiderNextState()
        {
            switch (CurrentState)
            {
                case ParseState.RequiresNextOccurrence:
                case ParseState.ExpectNextOccurrence:
                case ParseState.ExpectNextToken:
                    if (_entity.NumArity < _entity.Arity)
                    {
                        CurrentState = ParseState.ExpectNextToken;
                        break;
                    }
                        
                    _entity.NumOccurrences += 1;
                    _entity.NumArity = 0;

                    if (_entity.NumOccurrences < _entity.MinOccurrences)
                        CurrentState = ParseState.RequiresNextOccurrence;
                    else if (_entity.NumOccurrences < _entity.MaxOccurrences)
                        CurrentState = ParseState.ExpectNextOccurrence;
                    else
                        CurrentState = ParseState.Finalized;
                    break;
                case ParseState.Finalized:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}