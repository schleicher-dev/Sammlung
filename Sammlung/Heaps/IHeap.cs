using System;

namespace Sammlung.Heaps
{
    /// <summary>
    /// This is the abstract interface of a heap data structure.
    /// </summary>
    /// <typeparam name="T">the value type</typeparam>
    public interface IHeap<T>
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
        bool TryPeek(out T value);

        /// <summary>
        /// Removes the root node of the heap and returns it.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if there is a root node else false if the heap is empty.</returns>
        bool TryPop(out T value);

        /// <summary>
        /// Adds the item to the heap, using some sort of comparison.
        /// </summary>
        /// <param name="value">the value to add</param>
        void Push(T value);

        /// <summary>
        /// Replaces the old value with the new value.
        /// </summary>
        /// <param name="newValue">the new value</param>
        /// <param name="oldValue">the old value</param>
        /// <returns>true if replace was successful else false</returns>
        bool TryReplace(T newValue, out T oldValue);

        /// <summary>
        /// Updates the container of the old value with the new value.
        /// </summary>
        /// <param name="oldValue">the old value</param>
        /// <param name="newValue">the new value</param>
        /// <returns></returns>
        bool TryUpdate(T oldValue, T newValue);
    }
}