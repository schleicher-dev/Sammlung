using System;

namespace Sammlung.Utilities
{
    public static class ArgsHelper
    {
        private static T RequireNonNegative<T>(string name, T zero, T value) where T : IComparable<T>
            => zero.CompareTo(value) <= 0
                ? value
                : throw new ArgumentOutOfRangeException(name, value, "Non-negative value is required.");

        /// <summary>
        /// Returns the value if it is non-negative or throws an <seealso cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="name">the name</param>
        /// <param name="value">the value</param>
        /// <returns>the value if not out of range.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When argument out of range.</exception>
        public static int RequireNonNegative(string name, int value) => RequireNonNegative(name, 0, value);

        public static T RequireRangeInclusive<T>(string name, T lower, T upper, T value) where T : IComparable<T>
        {
            if (0 < lower.CompareTo(upper))
                throw new ArgumentException($"Expected {nameof(lower)} to be smaller or equal to {nameof(upper)}.");

            if (0 < lower.CompareTo(value) || 0 < value.CompareTo(upper))
                throw new ArgumentOutOfRangeException(name, value, $"Must be in range [{lower}, {upper}]");

            return value;
        }
    }
}