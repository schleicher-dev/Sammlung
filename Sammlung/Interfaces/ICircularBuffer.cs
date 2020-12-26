using System.Collections.Generic;

namespace Sammlung.Interfaces
{
    /// <summary>
    /// The <see cref="ICircularBuffer{T}"/> type saves a number of items into a circular buffer.
    /// </summary>
    public interface ICircularBuffer<T> : IEnumerable<T>
    {
        /// <summary>
        /// Contains the capacity of elements this buffer may take.
        /// </summary>
        int Capacity { get; }
        
        /// <summary>
        /// Contains the count of the contained elements.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Pushes the passed element to the front of the buffer.
        /// </summary>
        /// <param name="element">the element</param>
        void PushFront(T element);
        
        /// <summary>
        /// Pops an element from the front of the buffer.
        /// </summary>
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty buffer</exception>
        T PopFront();
        
        /// <summary>
        /// Pops an element from the front of the buffer.
        /// </summary>
        /// <param name="element">the element found at the front</param>
        /// <returns>true if pop could be done</returns>
        bool TryPopFront(out T element);
        
        /// <summary>
        /// Pushes the passed element to the end of the buffer.
        /// </summary>
        /// <param name="element">the element</param>
        void PushBack(T element);

        /// <summary>
        /// Pops an element from the back of the buffer.
        /// </summary>
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty buffer</exception>
        T PopBack();
        
        /// <summary>
        /// Pops an element from the back of the buffer.
        /// </summary>
        /// <param name="element">the element found at the back</param>
        /// <returns>true if pop could be done</returns>
        bool TryPopBack(out T element);
    }
}