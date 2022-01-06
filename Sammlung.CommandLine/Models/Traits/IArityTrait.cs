using System.Collections.Generic;

namespace Sammlung.CommandLine.Models.Traits
{
    /// <summary>
    /// The <see cref="IArityTrait"/> type defines the number of required tokens needed to pass to an entity.
    /// </summary>
    public interface IArityTrait
    {
        /// <summary>
        /// The number of tokens already parsed.
        /// </summary>
        int NumArity { get; internal set; }
        
        /// <summary>
        /// The arity value.
        /// </summary>
        int Arity { get; internal set; }
        
        /// <summary>
        /// The meta names for the arguments at the specified positions.
        /// </summary>
        IEnumerable<string> MetaNames { get; internal set; }
    }
}