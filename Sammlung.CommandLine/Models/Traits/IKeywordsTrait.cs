using System.Collections.Generic;

namespace Sammlung.CommandLine.Models.Traits
{
    /// <summary>
    /// The <see cref="IKeywordsTrait"/> identifies an entity with keywords.
    /// </summary>
    public interface IKeywordsTrait
    {
        /// <summary>
        /// The keywords of the entity.
        /// </summary>
        IEnumerable<string> Keywords { get; }
    }
}