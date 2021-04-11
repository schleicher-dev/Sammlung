using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sammlung.Utilities;

namespace Sammlung.Heaps
{
    [PublicAPI]
    public static class HeapExtensions
    {
        /// <summary>
        /// Removes the root node of the heap and returns it.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <returns>the root element</returns>
        /// <typeparam name="T">the value type</typeparam>
        /// <typeparam name="TPriority">the priority value</typeparam>
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        public static HeapPair<T, TPriority> Pop<T, TPriority>([NotNull] this IHeap<T, TPriority> heap)
            where TPriority : IComparable<TPriority> =>
            heap.TryPop(out var item)
                ? item
                : throw ExceptionsHelper.NewEmptyCollectionException(nameof(Pop));

        /// <summary>
        /// Returns the root node of the heap without removing it.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <typeparam name="T">the value type</typeparam>
        /// <typeparam name="TPriority">the priority value</typeparam>
        /// <returns>the root element</returns>
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        public static HeapPair<T, TPriority> Peek<T, TPriority>([NotNull] this IHeap<T, TPriority> heap)
            where TPriority : IComparable<TPriority> =>
            heap.TryPeek(out var item)
                ? item
                : throw ExceptionsHelper.NewEmptyCollectionException(nameof(Peek));

        /// <summary>
        /// Replaces the root element with a new value and organizes the collection.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <param name="newValue">the new value</param>
        /// <param name="priority">the priority value</param>
        /// <typeparam name="T">the value type</typeparam>
        /// <typeparam name="TPriority">the priority value</typeparam>
        /// <returns>the popped root element</returns>
        /// <exception cref="System.InvalidOperationException">when collection is empty</exception>
        public static HeapPair<T, TPriority> 
            Replace<T, TPriority>([NotNull] this IHeap<T, TPriority> heap, [NotNull] T newValue, [NotNull] TPriority priority)
            where TPriority : IComparable<TPriority>
        {
            return heap.TryReplace(newValue, priority, out var oldValue)
                ? oldValue
                : throw ExceptionsHelper.NewEmptyCollectionException(nameof(Replace));
        }

        /// <summary>
        /// Updates the container with the new value.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <param name="oldValue">the old value</param>
        /// <param name="priority">the priority</param>
        /// <typeparam name="T">the value type</typeparam>
        /// <typeparam name="TPriority">the priority value</typeparam>
        /// <exception cref="System.InvalidOperationException">when collection is empty</exception>
        public static void Update<T, TPriority>([NotNull] this IHeap<T, TPriority> heap, [NotNull] T oldValue, [NotNull] TPriority priority)
            where TPriority : IComparable<TPriority>
        {
            if (!heap.TryUpdate(oldValue, priority))
                throw ExceptionsHelper.NewHeapUpdateFailedException(nameof(Update));
        }
    }
}