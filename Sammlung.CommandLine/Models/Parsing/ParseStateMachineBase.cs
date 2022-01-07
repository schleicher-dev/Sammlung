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
        private int _numOccurrences;
        protected int _numArity;
        public ParseState CurrentState { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="ParseStateMachineBase{T}"/> using the demanded entity.
        /// </summary>
        /// <param name="entity"></param>
        protected ParseStateMachineBase(T entity)
        {
            _entity = entity.RequireNotNull(nameof(entity));
            _numArity = _numOccurrences = 0;
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
                    if (_numArity < _entity.Arity)
                    {
                        CurrentState = ParseState.ExpectNextToken;
                        break;
                    }
                        
                    _numOccurrences += 1;
                    _numArity = 0;

                    if (_numOccurrences < _entity.MinOccurrences)
                        CurrentState = ParseState.RequiresNextOccurrence;
                    else if (_numOccurrences < _entity.MaxOccurrences)
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