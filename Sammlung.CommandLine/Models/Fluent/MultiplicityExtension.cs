using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Models.Fluent
{
    
    /// <summary>
    /// The <see cref="MultiplicityExtension"/> type holds all the extensions for the
    /// <see cref="IMultiplicityTrait"/> interface.
    /// </summary>
    public static class MultiplicityExtension
    {
        /// <summary>
        /// Sets the multiplicity of the subject.
        /// </summary>
        /// <param name="subject">the subject</param>
        /// <param name="minOccurrences">the minimum occurrences</param>
        /// <param name="maxOccurrences">the maximum occurrences</param>
        /// <typeparam name="T">the type of the subject</typeparam>
        /// <returns>the same subject</returns>
        public static T SetMultiplicity<T>(this T subject, int minOccurrences, int? maxOccurrences) where T : IMultiplicityTrait
        {
            subject.AssignMultiplicity(minOccurrences, maxOccurrences);
            return subject;
        }
    }
}