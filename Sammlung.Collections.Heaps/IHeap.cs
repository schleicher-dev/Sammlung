using System;
using System.Collections.Generic;

namespace Sammlung.Collections.Heaps
{
    /// <summary>
    /// The <see cref="IHeap{T,TPriority}"/> is a representation of a heap data structure also known as priority queue.
    /// </summary>
    /// <typeparam name="T">the value type</typeparam>
    /// <typeparam name="TPriority">the priority type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public interface IHeap<T, TPriority> : IEnumerable<HeapPair<T, TPriority>> 
        where TPriority : IComparable<TPriority>
    {
        /// <summary>
        /// Returns the contained elements in the heap.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Returns if the heap is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Returns the root node of the heap.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if there is a root node else false if the heap is empty.</returns>
        bool TryPeek(out HeapPair<T, TPriority> value);

        /// <summary>
        /// Removes the root node of the heap and returns it.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if there is a root node else false if the heap is empty.</returns>
        bool TryPop(out HeapPair<T, TPriority> value);

        /// <summary>
        /// Adds the item to the heap, using some sort of comparison.
        /// </summary>
        /// <param name="priority">the priority value</param>
        /// <param name="value">the value to add</param>
        void Push(T value, TPriority priority);

        /// <summary>
        /// Replaces the top-most value with the new value.
        /// </summary>
        /// <param name="newValue">the new value</param>
        /// <param name="priority">the priority value</param>
        /// <param name="oldValue">the old value</param>
        /// <returns>true if replace was successful else false</returns>
        bool TryReplace(T newValue, TPriority priority, out HeapPair<T, TPriority> oldValue);

        /// <summary>
        /// Updates the priority of the old value with the new priority value.
        /// </summary>
        /// <param name="oldValue">the old value</param>
        /// <param name="priority">the priority value</param>
        /// <returns>true if update was successful else false</returns>
        bool TryUpdate(T oldValue, TPriority priority);
    }
}