using Sammlung.CommandLine.Models.Formatting;

namespace Sammlung.CommandLine.Models.Traits
{
    /// <summary>
    /// The <see cref="IDisplayFormatTrait"/> helps formatting the given entity using an <see cref="EntityFormatter"/>
    /// </summary>
    public interface IDisplayFormatTrait
    {
        /// <summary>
        /// The <see cref="Format(IEntityFormatter)"/> method manifests the visitor pattern used with
        /// <see cref="IEntityFormatter"/>
        /// </summary>
        /// <param name="formatter">the formatter</param>
        /// <returns>the string</returns>
        string Format(IEntityFormatter formatter);
    }
}