using Sammlung.Utilities;

namespace Sammlung.Queues
{
    public static class QueueExtensions
    {
        /// <summary>
        /// Pops an element from the front of the collection.
        /// </summary>
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty collection</exception>
        public static T PopRight<T>(this IQueue<T> queue) =>
            queue.TryPopRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();
        
        /// <summary>
        /// Peeks an element from the right of the collection.
        /// </summary>
        /// <returns>true if peek could be done</returns>
        /// <exception cref="System.InvalidOperationException">when peeking from empty collection</exception>
        public static T PeekRight<T>(this IQueue<T> queue) =>
            queue.TryPeekRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();
    }
}