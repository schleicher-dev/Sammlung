using Sammlung.CommandLine.Exceptions;

namespace Sammlung.CommandLine.Models.Parsing
{
    /// <summary>
    /// The <see cref="IParseStateMachine"/> type defines a simple state machine for the token parsing.
    /// </summary>
    public interface IParseStateMachine
    {
        /// <summary>
        /// The current state of the state machine.
        /// </summary>
        ParseState CurrentState { get; }
        
        /// <summary>
        /// Processes the next token.
        /// </summary>
        /// <param name="token">the token</param>
        /// <exception cref="ParseException">if the operation fails</exception>
        void ParseNext(string token);
    }
}