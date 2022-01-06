using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Models.Fluent
{
    /// <summary>
    /// The <see cref="ApplicationNameTraitExtensions"/> type holds all the extensions for the
    /// <see cref="IApplicationNameTrait"/> interface.
    /// </summary>
    public static class ApplicationNameTraitExtensions
    {
        /// <summary>
        /// Sets the application name in a fluent manner.
        /// </summary>
        /// <param name="subject">the subject</param>
        /// <param name="name">the name</param>
        /// <typeparam name="T">the original type of the subject</typeparam>
        /// <returns>the same subject</returns>
        public static T SetApplicationName<T>(this T subject, string name) where T : IApplicationNameTrait
        {
            subject.ApplicationName = name;
            return subject;
        }
    }
}