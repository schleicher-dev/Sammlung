using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Models.Fluent
{
    /// <summary>
    /// The <see cref="DescriptionTraitExtensions"/> type holds all the extensions for the
    /// <see cref="IDescriptionTrait"/> interface.
    /// </summary>
    public static class DescriptionTraitExtensions
    {
        /// <summary>
        /// Sets the description in a fluent manner.
        /// </summary>
        /// <param name="subject">the subject</param>
        /// <param name="description">the description text</param>
        /// <typeparam name="T">the original type of the subject</typeparam>
        /// <returns>the same subject</returns>
        public static T SetDescription<T>(this T subject, string description) where T : IDescriptionTrait
        {
            subject.Description = description;
            return subject;
        }
    }
}