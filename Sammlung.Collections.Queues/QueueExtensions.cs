using System;
using Sammlung.Werkzeug;

namespace Sammlung.Collections.Queues
{
    /// <summary>
    /// The <see cref="QueueExtensions"/> extends any <seealso cref="IQueue{T}"/> with additional methods.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class QueueExtensions
    {
        /// <summary>
        /// Pops an element from the front of the collection.
        /// </summary>
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty collection</exception>
        public static T PopRight<T>(this IQueue<T> queue) =>
            queue.RequireNotNull(nameof(queue)).TryPopRight(out var element) ? element : 
                throw new InvalidOperationException("The collection is empty.");

        /// <summary>
        /// Peeks an element from the right of the collection.
        /// </summary>
        /// <returns>true if peek could be done</returns>
        /// <exception cref="System.InvalidOperationException">when peeking from empty collection</exception>
        public static T PeekRight<T>(this IQueue<T> queue) =>
            queue.RequireNotNull(nameof(queue)).TryPeekRight(out var element)
                ? element
                : throw new InvalidOperationException("The collection is empty.");
    }
}