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
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        public static T Pop<T>(this IHeap<T> heap)
        {
            return heap.TryPop(out var item)
                ? item
                : throw ExceptionsHelper.NewEmptyCollectionException(nameof(Pop));
        }

        /// <summary>
        /// Returns the root node of the heap without removing it.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <returns>the root element</returns>
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        public static T Peek<T>(this IHeap<T> heap)
        {
            return heap.TryPeek(out var item)
                ? item
                : throw ExceptionsHelper.NewEmptyCollectionException(nameof(Peek));
        }

        /// <summary>
        /// Replaces the root element with a new value and organizes the collection.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <param name="newValue">the new value</param>
        /// <typeparam name="T">the value type</typeparam>
        /// <returns>the popped root element</returns>
        /// <exception cref="System.InvalidOperationException">when collection is empty</exception>
        public static T Replace<T>(this IHeap<T> heap, T newValue)
        {
            return heap.TryReplace(newValue, out var oldValue)
                ? oldValue
                : throw ExceptionsHelper.NewEmptyCollectionException(nameof(Replace));
        }

        /// <summary>
        /// Updates the container with the new value.
        /// </summary>
        /// <param name="heap">the heap to operate on</param>
        /// <param name="oldValue">the old value</param>
        /// <param name="newValue">the new value</param>
        /// <typeparam name="T">the type param</typeparam>
        /// <exception cref="System.InvalidOperationException">when collection is empty</exception>
        public static void Update<T>(this IHeap<T> heap, T oldValue, T newValue)
        {
            if (!heap.TryUpdate(oldValue, newValue))
                throw ExceptionsHelper.NewHeapUpdateFailedException(nameof(Update));
        }
    }
}