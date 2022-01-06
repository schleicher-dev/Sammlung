using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Models.Fluent
{
    /// <summary>
    /// The <see cref="ArityTraitExtensions"/> type holds all the extensions for the <see cref="IArityTrait"/> interface.
    /// </summary>
    public static class ArityTraitExtensions
    {
        /// <summary>
        /// Sets the arity of the passed subject.
        /// </summary>
        /// <param name="subject">the subject</param>
        /// <param name="value">the arity value</param>
        /// <typeparam name="T">the type of the subject</typeparam>
        /// <returns>the same subject</returns>
        public static T SetArity<T>(this T subject, int value) where T : IArityTrait
        {
            subject.Arity = value;
            return subject;
        }

        /// <summary>
        /// Sets the meta names of the passed subject.
        /// </summary>
        /// <param name="subject">the subject</param>
        /// <param name="names">the meta names</param>
        /// <typeparam name="T">the type of the subject</typeparam>
        /// <returns>the same subject</returns>
        public static T SetMetaNames<T>(this T subject, params string[] names) where T : IArityTrait =>
            subject.SetMetaNames(names.AsEnumerable());

        /// <summary>
        /// Sets the meta names of the passed subject.
        /// </summary>
        /// <param name="subject">the subject</param>
        /// <param name="names">the meta names</param>
        /// <typeparam name="T">the type of the subject</typeparam>
        /// <returns>the same subject</returns>
        public static T SetMetaNames<T>(this T subject, IEnumerable<string> names) where T : IArityTrait
        {
            subject.MetaNames = names;
            return subject;
        }
    }
}