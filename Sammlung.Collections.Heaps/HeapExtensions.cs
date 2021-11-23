using System;
using Sammlung.Collections.Heaps.Concurrent;
using Sammlung.Werkzeug;

namespace Sammlung.Collections.Heaps
{
    [JetBrains.Annotations.PublicAPI]
    public static class HeapExtensions
    {
        /// <summary>
        /// Decorates a <see cref="IHeap{T,TPriority}"/> as a <see cref="BlockingHeap{T,TPriority}"/>.
        /// </summary>
        /// <param name="decorated">the heap to be decorated</param>
        /// <typeparam name="T">the type</typeparam>
        /// <typeparam name="TPriority">the priority type</typeparam>
        /// <returns>the blocking heap</returns>
        public static BlockingHeap<T, TPriority> Wrap<T, TPriority>(this IHeap<T, TPriority> decorated)
            where TPriority : IComparable<TPriority> => new BlockingHeap<T, TPriority>(decorated);

        /// <summary>
        /// Removes the root node of the heap and returns it.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <returns>the root element</returns>
        /// <typeparam name="T">the value type</typeparam>
        /// <typeparam name="TPriority">the priority value</typeparam>
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        public static HeapPair<T, TPriority> Pop<T, TPriority>(this IHeap<T, TPriority> heap)
            where TPriority : IComparable<TPriority> =>
            heap.RequireNotNull(nameof(heap)).TryPop(out var item)
                ? item
                : throw ExceptionFactory.NewEmptyCollectionException();

        /// <summary>
        /// Returns the root node of the heap without removing it.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <typeparam name="T">the value type</typeparam>
        /// <typeparam name="TPriority">the priority value</typeparam>
        /// <returns>the root element</returns>
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        public static HeapPair<T, TPriority> Peek<T, TPriority>(this IHeap<T, TPriority> heap)
            where TPriority : IComparable<TPriority> =>
            heap.RequireNotNull(nameof(heap)).TryPeek(out var item)
                ? item
                : throw ExceptionFactory.NewEmptyCollectionException();

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
            Replace<T, TPriority>(this IHeap<T, TPriority> heap, T newValue, TPriority priority)
            where TPriority : IComparable<TPriority> =>
            heap.RequireNotNull(nameof(heap)).TryReplace(newValue, priority, out var oldValue)
                ? oldValue
                : throw ExceptionFactory.NewElementNotFoundException(newValue, nameof(newValue));

        /// <summary>
        /// Updates the container with the new value.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <param name="oldValue">the old value</param>
        /// <param name="priority">the priority</param>
        /// <typeparam name="T">the value type</typeparam>
        /// <typeparam name="TPriority">the priority value</typeparam>
        /// <exception cref="System.InvalidOperationException">when collection is empty</exception>
        public static void Update<T, TPriority>(this IHeap<T, TPriority> heap, T oldValue, TPriority priority)
            where TPriority : IComparable<TPriority>
        {
            if (!heap.RequireNotNull(nameof(heap)).TryUpdate(oldValue, priority))
                throw ExceptionFactory.NewElementNotFoundException(oldValue, nameof(oldValue));
        }
    }
}