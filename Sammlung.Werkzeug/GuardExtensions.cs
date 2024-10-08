﻿using System;
using Sammlung.Werkzeug.Resources;

namespace Sammlung.Werkzeug
{
    /// <summary>
    /// The <see cref="GuardExtensions"/> type extends types with helpful validation checks.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class GuardExtensions
    {
        /// <summary>
        /// Requires parameter to be not null.
        /// </summary>
        /// <param name="param">the value to be checked for null</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentNullException">The parameter is null</exception>
        public static T RequireNotNull<T>(this T param, string paramName) where T : class =>
            param ?? throw new ArgumentNullException(paramName, ErrorMessages.ParamRequiredNotNull);

        /// <summary>
        /// Requires the parameter to be not null or empty.
        /// </summary>
        /// <param name="param">the string value to be checked for null or empty</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentNullException">The string parameter is null</exception>
        /// <exception cref="ArgumentException">The string parameter is empty</exception>
        public static string RequireNotNullOrEmpty(this string param, string paramName) =>
            param.RequireNotNull(paramName).Length != 0
                ? param
                : throw new ArgumentException(ErrorMessages.ParamStringRequiredNotNullOrEmpty, paramName);

        /// <summary>
        /// Requires the parameter to be greater than the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireGreater<T>(this T param, T expected, string paramName) where T : IComparable<T> =>
            param.IsGreater(expected)
                ? param
                : throw new ArgumentOutOfRangeException(paramName, param,
                    string.Format(ErrorMessages.ParamRequiredGreater, expected));

        /// <summary>
        /// Requires the parameter to be greater than or equal to the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireGreaterEqual<T>(this T param, T expected, string paramName) where T : IComparable<T> =>
            param.IsGreaterEqual(expected)
                ? param
                : throw new ArgumentOutOfRangeException(paramName, param,
                    string.Format(ErrorMessages.ParamRequiredGreaterEqual, expected));

        /// <summary>
        /// Requires the parameter to be less than the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireLess<T>(this T param, T expected, string paramName) where T : IComparable<T> =>
            param.IsLess(expected)
                ? param
                : throw new ArgumentOutOfRangeException(paramName, param,
                    string.Format(ErrorMessages.ParamRequiredLess, expected));

        /// <summary>
        /// Requires the parameter to be less than or equal to the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireLessEqual<T>(this T param, T expected, string paramName) where T : IComparable<T> =>
            param.IsLessEqual(expected)
                ? param
                : throw new ArgumentOutOfRangeException(paramName, param,
                    string.Format(ErrorMessages.ParamRequiredLessEqual, expected));

        /// <summary>
        /// Requires the parameter to be less than or equal to the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireEqual<T>(this T param, T expected, string paramName) where T : IEquatable<T> =>
            param.Equals(expected)
                ? param
                : throw new ArgumentOutOfRangeException(paramName, param,
                    string.Format(ErrorMessages.ParamRequiredEqual, expected));
    }
}