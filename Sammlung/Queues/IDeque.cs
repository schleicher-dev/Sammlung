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
        void PushRight(T element);

        /// <summary>
        /// Pops an element from the back of the collection.
        /// </summary>
        /// <returns>true if pop could be done</returns>
        /// <exception cref="System.InvalidOperationException">when popping from empty collection</exception>
        T PopLeft();
        
        /// <summary>
        /// Pops an element from the back of the collection.
        /// </summary>
        /// <param name="element">the element found at the back</param>
        /// <returns>true if pop could be done</returns>
        bool TryPopLeft(out T element);
        
        /// <summary>
        /// Peeks an element from the left of the collection.
        /// </summary>
        /// <returns>true if peek could be done</returns>
        /// <exception cref="System.InvalidOperationException">when peeking from empty collection</exception>
        T PeekLeft();
        
        /// <summary>
        /// Peeks an element from the left of the collection.
        /// </summary>
        /// <returns>true if peek could be done</returns>
        bool TryPeekLeft(out T element);
    }
}