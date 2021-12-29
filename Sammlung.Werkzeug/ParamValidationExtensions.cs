﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Werkzeug.Resources;

namespace Sammlung.Werkzeug
{
    /// <summary>
    /// The <see cref="ParamValidationExtensions"/> type extends types with helpful validation checks.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class ParamValidationExtensions
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
        /// <exception cref="ArgumentOutOfRangeException">The string parameter is null or empty</exception>
        public static string RequireNotNullOrEmpty(this string param, string paramName) =>
            !string.IsNullOrEmpty(param)
                ? param
                : throw new ArgumentOutOfRangeException(paramName, param,
                    ErrorMessages.ParamStringRequiredNotNullOrEmpty);

        /// <summary>
        /// Requires the parameter to be greater than the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireGreater<T>(this T param, T expected, string paramName) where T : IComparable<T>
        {
            var msg = string.Format(ErrorMessages.ParamRequiredGreater, expected);
            return param.IsGreater(expected) ? param : throw new ArgumentOutOfRangeException(paramName, param, msg);
        }

        /// <summary>
        /// Requires the parameter to be greater than or equal to the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireGreaterEqual<T>(this T param, T expected, string paramName) where T : IComparable<T>
        {
            var msg = string.Format(ErrorMessages.ParamRequiredGreaterEqual, expected);
            return param.IsGreaterEqual(expected) ? param : throw new ArgumentOutOfRangeException(paramName, param, msg);
        }

        /// <summary>
        /// Requires the parameter to be less than the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireLess<T>(this T param, T expected, string paramName) where T : IComparable<T>
        {
            var msg = string.Format(ErrorMessages.ParamRequiredGreater, expected);
            return param.IsLess(expected) ? param : throw new ArgumentOutOfRangeException(paramName, param, msg);
        }

        /// <summary>
        /// Requires the parameter to be less than or equal to the expected value.
        /// </summary>
        /// <param name="param">the comparable value</param>
        /// <param name="expected">the expected value</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the parameter which is comparable</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is less or equal to the expected value</exception>
        public static T RequireLessEqual<T>(this T param, T expected, string paramName) where T : IComparable<T>
        {
            var msg = string.Format(ErrorMessages.ParamRequiredGreaterEqual, expected);
            return param.IsLessEqual(expected) ? param : throw new ArgumentOutOfRangeException(paramName, param, msg);
        }

        /// <summary>
        /// Requires the parameter to have at least the given number of element.s
        /// </summary>
        /// <param name="param">the parameter collection</param>
        /// <param name="expected">the expected element count</param>
        /// <param name="paramName">the name of the parameter</param>
        /// <typeparam name="T">the type of the collection</typeparam>
        /// <typeparam name="TCollection">the collection type</typeparam>
        /// <returns>the parameter</returns>
        /// <exception cref="ArgumentException">The parameter has less than the expected number of elements</exception>
        public static TCollection RequireNumElements<TCollection>(this TCollection param, ushort expected,
            string paramName) where TCollection : ICollection =>
            expected <= param.Count
                ? param
                : throw new ArgumentException(
                    string.Format(ErrorMessages.ParamRequireNumElements, expected, param.Count), paramName);
    }
}