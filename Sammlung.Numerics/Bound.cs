using System;

namespace Sammlung.Numerics
{
    /// <summary>
    /// The <see cref="Bound"/> type offers methods to create bounds.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class Bound
    {
        /// <summary>
        /// Creates an empty <see cref="Bound{T}"/>.
        /// </summary>
        /// <typeparam name="T">the number type</typeparam>
        /// <returns>a special bound</returns>
        public static Bound<T> Empty<T>() where T : IComparable<T> => null;
        
        /// <summary>
        /// Creates an inclusive <see cref="Bound{T}"/>.
        /// </summary>
        /// <param name="value">the value of the bound</param>
        /// <typeparam name="T">the number type</typeparam>
        /// <returns>the inclusive bound</returns>
        public static Bound<T> Inclusive<T>(T value) where T : IComparable<T> => new Bound<T>(value, true);
        
        /// <summary>
        /// Creates an exclusive <see cref="Bound{T}"/>.
        /// </summary>
        /// <param name="value">the value of the bound</param>
        /// <typeparam name="T">the number type</typeparam>
        /// <returns>the exclusive bound</returns>
        public static Bound<T> Exclusive<T>(T value) where T : IComparable<T> => new Bound<T>(value, false);

        /// <summary>
        /// Creates a bound with any attribute.
        /// </summary>
        /// <param name="value">the value of the bound</param>
        /// <param name="inclusive">if the value is inclusive</param>
        /// <typeparam name="T">the number type</typeparam>
        /// <returns>the bound</returns>
        public static Bound<T> Create<T>(T value, bool inclusive) where T : IComparable<T> =>
            new Bound<T>(value, inclusive);
    }

    /// <summary>
    /// The <see cref="Bound{T}"/> type denotes a boundary point.
    /// </summary>
    /// <typeparam name="T">the number type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class Bound<T> where T : IComparable<T>
    {
        /// <summary>
        /// The value of the bound.
        /// </summary>
        public T Value { get; }
        
        /// <summary>
        /// If the value of the bound is inclusive or exclusive when set.
        /// </summary>
        public bool Inclusive { get; }

        /// <summary>
        /// Creates a new <see cref="Bound{T}"/> using the value and the inclusive indicator.
        /// </summary>
        /// <param name="value">the value</param>
        /// <param name="inclusive">the indicator if the value is inclusive or exclusive</param>
        public Bound(T value, bool inclusive)
        {
            Value = value;
            Inclusive = inclusive;
        }
    }
}