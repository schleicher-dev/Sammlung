using System.Collections.Generic;

namespace Sammlung.Queues
{
    /// <summary>
    /// The <see cref="IQueue{T}"/> represents a single-ended queue.
    /// </summary>
    /// <typeparam name="T">the element type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public interface IQueue<T> : IEnumerable<T>
    {
        /// <summary>
        /// Returns the number of elements in this collection.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Pushes the passed element to the end of the collection.
        /// </summary>
        /// <param name="element">the element</param>
        void PushLeft(T element);
        
        /// <summary>
        /// Pops an element from the front of the collection.
        /// </summary>
        /// <param name="element">the element found at the right</param>
        /// <returns>true if pop could be done</returns>
        bool TryPopRight(out T element);
        
        /// <summary>
        /// Peeks an element from the right of the collection.
        /// </summary>
        /// <returns>true if peek could be done</returns>
        bool TryPeekRight(out T element);
    }
}