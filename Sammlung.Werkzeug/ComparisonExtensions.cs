using System;

namespace Sammlung.Werkzeug
{
    /// <summary>
    /// The <see cref="ComparisonExtensions"/> type should help to clarify what the value of
    /// <see cref="IComparable{T}.CompareTo"/> means.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class ComparisonExtensions
    {
        /// <summary>
        /// Checks if the first value is less than the second value.
        /// </summary>
        /// <param name="value" />
        /// <param name="comp" />
        /// <typeparam name="T">the comparable type</typeparam>
        /// <returns>true if less else false</returns>
        public static bool IsLess<T>(this T value, T comp) where T : IComparable<T> => value.CompareTo(comp) < 0;
        
        /// <summary>
        /// Checks if the first value is less than or equal to the second value.
        /// </summary>
        /// <param name="value" />
        /// <param name="comp" />
        /// <typeparam name="T">the comparable type</typeparam>
        /// <returns>true if less or equal else false</returns>
        public static bool IsLessEqual<T>(this T value, T comp) where T : IComparable<T> => value.CompareTo(comp) <= 0;
        
        /// <summary>
        /// Checks if the first value is equal to the second value.
        /// </summary>
        /// <param name="value" />
        /// <param name="comp" />
        /// <typeparam name="T">the comparable type</typeparam>
        /// <returns>true if equal else false</returns>
        public static bool IsEqual<T>(this T value, T comp) where T : IComparable<T> => value.CompareTo(comp) == 0;
        
        /// <summary>
        /// Checks if the first value is greater than or equal to the second value.
        /// </summary>
        /// <param name="value" />
        /// <param name="comp" />
        /// <typeparam name="T">the comparable type</typeparam>
        /// <returns>true if greater or equal else false</returns>
        public static bool IsGreaterEqual<T>(this T value, T comp) where T : IComparable<T> => 0 <= value.CompareTo(comp);
        
        /// <summary>
        /// Checks if the first value is greater than the second value.
        /// </summary>
        /// <param name="value" />
        /// <param name="comp" />
        /// <typeparam name="T">the comparable type</typeparam>
        /// <returns>true if greater else false</returns>
        public static bool IsGreater<T>(this T value, T comp) where T : IComparable<T> => 0 < value.CompareTo(comp);
    }
}