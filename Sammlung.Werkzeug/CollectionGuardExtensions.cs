using System;
using System.Collections;
using Sammlung.Werkzeug.Resources;


namespace Sammlung.Werkzeug
{
    [JetBrains.Annotations.PublicAPI]
    public static class CollectionGuardExtensions
    {
        /// <summary>
        /// Requires the parameter to have at least the given number of elements.
        /// </summary>
        /// <param name="param">the parameter collection</param>
        /// <param name="expected">the expected element count</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the collection</typeparam>
        /// <typeparam name="TCollection">the collection type</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentException">The parameter has less than the expected number of elements</exception>
        public static TCollection RequireAtLeastNumElements<TCollection>(this TCollection param, int expected,
            string paramName) where TCollection : ICollection =>
            expected <= param.Count
                ? param
                : throw new ArgumentException(
                    string.Format(ErrorMessages.ParamRequireAtLeastNumElements, expected, param.Count), paramName);
        
        /// <summary>
        /// Requires the parameter to have at most the given number of elements.
        /// </summary>
        /// <param name="param">the parameter collection</param>
        /// <param name="expected">the expected element count</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the collection</typeparam>
        /// <typeparam name="TCollection">the collection type</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentException">The parameter has less than the expected number of elements</exception>
        public static TCollection RequireAtMostNumElements<TCollection>(this TCollection param, int expected,
            string paramName) where TCollection : ICollection =>
            param.Count <= expected
                ? param
                : throw new ArgumentException(
                    string.Format(ErrorMessages.ParamRequireAtMostNumElements, expected, param.Count), paramName);
        
        /// <summary>
        /// Requires the parameter to have exactly the given number of elements.
        /// </summary>
        /// <param name="param">the parameter collection</param>
        /// <param name="expected">the expected element count</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the collection</typeparam>
        /// <typeparam name="TCollection">the collection type</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentException">The parameter has less than the expected number of elements</exception>
        public static TCollection RequireExactlyNumElements<TCollection>(this TCollection param, int expected,
            string paramName) where TCollection : ICollection =>
            param.Count == expected
                ? param
                : throw new ArgumentException(
                    string.Format(ErrorMessages.ParamRequireExactlyNumElements, expected, param.Count), paramName);
    }
}