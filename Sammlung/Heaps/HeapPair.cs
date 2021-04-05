using System;
using JetBrains.Annotations;

namespace Sammlung.Heaps
{
    /// <summary>
    /// The <see cref="HeapPair"/> eases the painful task of creating a new <seealso cref="HeapPair{TValue,TPriority}"/>
    /// by making it possible to skip the generic arguments.
    /// </summary>
    [PublicAPI]
    public static class HeapPair
    {
        /// <summary>
        /// Creates a new <see cref="HeapPair{TValue,TPriority}"/> using the value and priority.
        /// </summary>
        /// <param name="value">the value</param>
        /// <param name="priority">the priority</param>
        /// <typeparam name="TValue">the value type</typeparam>
        /// <typeparam name="TPriority">the priority type</typeparam>
        /// <returns></returns>
        public static HeapPair<TValue, TPriority> Create<TValue, TPriority>(TValue value, TPriority priority)
            where TPriority : IComparable<TPriority> => new HeapPair<TValue, TPriority>(value, priority);
    }
    
    /// <summary>
    /// The <see cref="HeapPair{TValue,TPriority}"/> holds the value and its priority.
    /// </summary>
    /// <typeparam name="TValue">the value type</typeparam>
    /// <typeparam name="TPriority">the priority type</typeparam>
    [PublicAPI]
    public class HeapPair<TValue, TPriority> where TPriority : IComparable<TPriority>
    {
        /// <summary>
        /// Creates a new <see cref="HeapPair{TValue,TPriority}"/> using the value and priority.
        /// </summary>
        /// <param name="value">the value</param>
        /// <param name="priority">the priority</param>
        public HeapPair(TValue value, TPriority priority)
        {
            Value = value;
            Priority = priority;
        }
        
        /// <summary>
        /// The Value.
        /// </summary>
        public TValue Value { get; }
        
        /// <summary>
        /// The Priority.
        /// </summary>
        public TPriority Priority { get; }
    }
}