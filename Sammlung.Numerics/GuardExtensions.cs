using System;
using Sammlung.Numerics.Resources;
using Sammlung.Werkzeug;

namespace Sammlung.Numerics
{
    /// <summary>
    /// The <see cref="GuardExtensions"/> type extends types with helpful validation checks.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class GuardExtensions
    {
        /// <summary>
        /// Requires the value to be an element of the given interval.
        /// </summary>
        /// <param name="value">the value</param>
        /// <param name="interval">the interval</param>
        /// <param name="paramName">the parameter name</param>
        /// <typeparam name="T">the number type</typeparam>
        /// <returns>the value if element of the interval</returns>
        /// <exception cref="ArgumentOutOfRangeException">when the value is not element of the interval</exception>
        public static T RequireElementOf<T>(this T value, Interval<T> interval, string paramName)
            where T : IComparable<T>
        {
            return interval.RequireNotNull(nameof(interval)).Contains(value)
                ? value
                : throw new ArgumentOutOfRangeException(paramName, value,
                    string.Format(ErrorMessages.ParamRequireElementOf, interval));
        }
    }
}