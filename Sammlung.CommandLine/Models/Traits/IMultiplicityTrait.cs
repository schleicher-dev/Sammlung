namespace Sammlung.CommandLine.Models.Traits
{
    /// <summary>
    /// The <see cref="IMultiplicityTrait"/> type signalizes, that the implementor can be assigned multiple times.
    /// </summary>
    public interface IMultiplicityTrait
    {
        /// <summary>
        /// The number of occurrences already found.
        /// </summary>
        int NumOccurrences { get; internal set; }
        
        /// <summary>
        /// The minimum number of occurrences.
        /// </summary>
        int MinOccurrences { get; }
        
        /// <summary>
        /// The maximum number of occurrences.
        /// </summary>
        int? MaxOccurrences { get; }
        
        /// <summary>
        /// Sets the expected multiplicity of the object.
        /// </summary>
        /// <param name="minOccurrences" />
        /// <param name="maxOccurrences" />
        void AssignMultiplicity(int minOccurrences, int? maxOccurrences);
    }
}