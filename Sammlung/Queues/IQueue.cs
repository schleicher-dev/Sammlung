using System.Collections.Generic;

namespace Sammlung.Queues
{
    public interface IQueue<T> : ICollection<T>
    {
        /// <summary>
        /// Pushes the passed element to the end of the buffer.
        /// </summary>
        /// <param name="element">the element</param>
        void Enqueue(T element);
        
        /// <summary>
        /// Pops an element from the front of the buffer.
        /// </summary>
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty buffer</exception>
        T Dequeue();
        
        /// <summary>
        /// Pops an element from the front of the buffer.
        /// </summary>
        /// <param name="element">the element found at the front</param>
        /// <returns>true if pop could be done</returns>
        bool TryDequeue(out T element);
    }
}