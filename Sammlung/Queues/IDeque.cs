namespace Sammlung.Queues
{
    /// <summary>
    /// The <see cref="IDeque{T}"/> type denotes a double ended queue.
    /// </summary>
    public interface IDeque<T> : IQueue<T>
    {
        /// <summary>
        /// Pushes the passed element to the front of the collection.
        /// </summary>
        /// <param name="element">the element</param>
        void InverseEnqueue(T element);

        /// <summary>
        /// Pops an element from the back of the collection.
        /// </summary>
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty collection</exception>
        T InverseDequeue();
        
        /// <summary>
        /// Pops an element from the back of the collection.
        /// </summary>
        /// <param name="element">the element found at the back</param>
        /// <returns>true if pop could be done</returns>
        bool TryInverseDequeue(out T element);
    }
}