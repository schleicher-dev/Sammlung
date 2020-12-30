using System.Collections.Generic;

namespace Sammlung.Queues
{
    public interface IQueue<T>
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
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty collection</exception>
        T PopRight();
        
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
        /// <exception cref="System.InvalidOperationException">when peeking from empty collection</exception>
        T PeekRight();
        
        /// <summary>
        /// Peeks an element from the right of the collection.
        /// </summary>
        /// <returns>true if peek could be done</returns>
        bool TryPeekRight(out T element);
    }
}