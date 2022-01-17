using System;
using System.Globalization;
using Sammlung.Numerics.Resources;
using Sammlung.Werkzeug;

namespace Sammlung.Numerics
{
    /// <summary>
    /// The <see cref="Interval"/> type contains a static method to create a <see cref="Interval{T}"/> without
    /// the explicitly need of the type parameter on construction.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class Interval
    {
        /// <summary>
        /// Creates a <see cref="Interval{T}"/> with a lower and upper bound.
        /// </summary>
        /// <param name="lower" />
        /// <param name="upper" />
        /// <typeparam name="T">the number type</typeparam>
        /// <returns>the interval</returns>
        public static Interval<T> Create<T>(Bound<T> lower, Bound<T> upper)
            where T : IComparable<T> => new Interval<T>(lower, upper);
        
        /// <summary>
        /// Creates an <see cref="Interval{T}"/> with both passed values interpreted as inclusive bounds.
        /// </summary>
        /// <param name="lower">the lower value</param>
        /// <param name="upper">the upper value</param>
        /// <typeparam name="T">the type of the values</typeparam>
        /// <returns>the interval</returns>
        public static Interval<T> CreateWithInclusiveBounds<T>(T lower, T upper) where T : IComparable<T> =>
            Create(Bound.Inclusive(lower), Bound.Inclusive(upper));
        
        /// <summary>
        /// Creates an <see cref="Interval{T}"/> with both passed values interpreted as exclusive bounds.
        /// </summary>
        /// <param name="lower">the lower value</param>
        /// <param name="upper">the upper value</param>
        /// <typeparam name="T">the type of the values</typeparam>
        /// <returns>the interval</returns>
        public static Interval<T> CreateWithExclusiveBounds<T>(T lower, T upper) where T : IComparable<T> =>
            Create(Bound.Exclusive(lower), Bound.Exclusive(upper));

        /// <summary>
        /// Creates an <see cref="Interval{T}"/> with the first argument interpreted as inclusive and the second
        /// argument interpreted as exclusive bound.
        /// </summary>
        /// <param name="lower">the lower value</param>
        /// <param name="upper">the upper value</param>
        /// <typeparam name="T">the type of the values</typeparam>
        /// <returns>the interval</returns>
        public static Interval<T> CreateWithInclusiveAndExclusiveBounds<T>(T lower, T upper) where T : IComparable<T> =>
            Create(Bound.Inclusive(lower), Bound.Exclusive(upper));
        
        /// <summary>
        /// Creates an <see cref="Interval{T}"/> with the first argument interpreted as exclusive and the second
        /// argument interpreted as inclusive bound.
        /// </summary>
        /// <param name="lower">the lower value</param>
        /// <param name="upper">the upper value</param>
        /// <typeparam name="T">the type of the values</typeparam>
        /// <returns>the interval</returns>
        public static Interval<T> CreateWithExclusiveAndInclusiveBounds<T>(T lower, T upper) where T : IComparable<T> =>
            Create(Bound.Exclusive(lower), Bound.Inclusive(upper));
    }

    /// <summary>
    /// The <see cref="Interval{T}"/> type represents a mathematical interval.
    /// </summary>
    /// <typeparam name="T">the number type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class Interval<T> where T : IComparable<T>
    {
        private static string GetString(Bound<T> lowerBound, Bound<T> upperBound)
        {
            var lowerValueStr = lowerBound != null ? string.Format(CultureInfo.InvariantCulture, "{0}{1}", lowerBound.Inclusive ? "[" : "(", lowerBound.Value) : "(-Inf";
            var upperValueStr = upperBound != null ? string.Format(CultureInfo.InvariantCulture, "{0}{1}", upperBound.Value, upperBound.Inclusive ? "]" : ")") : "+Inf)";

            return $"{lowerValueStr}; {upperValueStr}";
        }

        private static bool IsValidInterval(Bound<T> lowerBound, Bound<T> upperBound) =>
            lowerBound == null || upperBound == null ||
            (!lowerBound.Inclusive && lowerBound.Value.IsLess(upperBound.Value) ||
             lowerBound.Inclusive && lowerBound.Value.IsLessEqual(upperBound.Value)) &&
            (!upperBound.Inclusive && upperBound.Value.IsGreater(lowerBound.Value) ||
             upperBound.Inclusive && upperBound.Value.IsGreaterEqual(lowerBound.Value));

        private static void RequireValidInterval(Bound<T> lowerBound, Bound<T> upperBound)
        {
            if (IsValidInterval(lowerBound, upperBound)) return;
            var representation = GetString(lowerBound, upperBound);
            var msg = string.Format(ErrorMessages.IntervalInvalid, representation);
            throw new ArgumentException(msg);
        }

        /// <summary>
        /// The lower value.
        /// </summary>
        public Bound<T> LowerBound { get; }

        /// <summary>
        /// The upper value.
        /// </summary>
        public Bound<T> UpperBound { get; }
        
        /// <summary>
        /// Creates a new <see cref="Interval{T}"/> using a lower and upper bound.
        /// </summary>
        /// <param name="lowerBound">the lower bound; if null the interval is unbounded below</param>
        /// <param name="upperBound">the upper bound; if null the interval is unbounded above</param>
        public Interval(Bound<T> lowerBound, Bound<T> upperBound)
        {
            RequireValidInterval(lowerBound, upperBound);
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        /// <summary>
        /// Indicates if the interval contains the particular value of not.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if inside the interval else false</returns>
        public bool Contains(T value) =>
            (LowerBound == null ||
             !LowerBound.Inclusive && LowerBound.Value.IsLess(value) ||
             LowerBound.Inclusive && LowerBound.Value.IsLessEqual(value)) &&
            (UpperBound == null ||
             !UpperBound.Inclusive && UpperBound.Value.IsGreater(value) ||
             UpperBound.Inclusive && UpperBound.Value.IsGreaterEqual(value));

        /// <inheritdoc />
        public override string ToString() => GetString(LowerBound, UpperBound);
    }
}