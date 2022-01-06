using Sammlung.CommandLine.Models.Parsing;

namespace Sammlung.CommandLine.Models.Traits
{
    /// <summary>
    /// The <see cref="IParseTrait"/> marks an instance which is able to parse.
    /// </summary>
    public interface IParseTrait
    {
        /// <summary>
        /// The parse state machine.
        /// </summary>
        IParseStateMachine ParseStateMachine { get; }
    }
}