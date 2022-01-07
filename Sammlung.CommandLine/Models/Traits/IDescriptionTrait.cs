namespace Sammlung.CommandLine.Models.Traits
{
    /// <summary>
    /// The <see cref="IDescriptionTrait"/> signalizes that there is a description present on the implementing object.
    /// </summary>
    public interface IDescriptionTrait
    {
        /// <summary>
        /// The description.
        /// </summary>
        string Description { get; set; }
    }
}