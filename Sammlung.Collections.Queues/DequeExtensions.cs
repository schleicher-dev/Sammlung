using Sammlung.Utilities;

namespace Sammlung.Collections.Queues
{
    /// <summary>
    /// The <see cref="DequeExtensions"/> extend any <see cref="IDeque{T}"/> implementation with additional methods.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class DequeExtensions
    {
        /// <summary>
        /// Pops an element from the back of the collection.
        /// </summary>
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty collection</exception>
        public static T PopLeft<T>(this IDeque<T> queue) =>
            queue.RequireNotNull(nameof(queue)).TryPopLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException(nameof(PopLeft));
        
        /// <summary>
        /// Peeks an element from the left of the collection.
        /// </summary>
        /// <returns>true if peek could be done</returns>
        /// <exception cref="System.InvalidOperationException">when peeking from empty collection</exception>
        public static T PeekLeft<T>(this IDeque<T> queue) =>
            queue.RequireNotNull(nameof(queue)).TryPeekLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException(nameof(PeekLeft));
    }
}