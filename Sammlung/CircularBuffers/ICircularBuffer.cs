namespace Sammlung.CircularBuffers
{
    /// <summary>
    /// The <see cref="ICircularBuffer{T}"/> interface exposes the minimal method footprint for a circular buffer also
    /// known as ring buffer.
    /// </summary>
    /// <typeparam name="T">the buffered type</typeparam>
    public interface ICircularBuffer<in T>
    {
        /// <summary>
        /// The capacity of the buffer.
        /// </summary>
        int Capacity { get; }
        
        /// <summary>
        /// The number of elements in the buffer.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Tries to put the items into the buffer.
        /// </summary>
        /// <param name="putItems">the items to put into the buffer</param>
        /// <returns>true if there is enough space else false</returns>
        bool TryPut(params T[] putItems);
        
        /// <summary>
        /// Tries to take the items from the buffer and writes them to the passed destination array at the offset and
        /// the amount of items.
        /// </summary>
        /// <param name="takeItems">the destination array</param>
        /// <param name="offset">the offset inside the array</param>
        /// <param name="length">the amount of items to take</param>
        /// <returns>true if there were enough items and the destination array has enough space else false</returns>
        bool TryTake(T[] takeItems, int offset, int length);

        /// <summary>
        /// Tries to peek the items from the buffer and writes them to the passed destination array at the offset and
        /// the amount of items.
        /// </summary>
        /// <param name="peekItems">the destination array</param>
        /// <param name="offset">the offset inside the array</param>
        /// <param name="length">the amount of items to take</param>
        /// <returns>true if there were enough items and the destination array has enough space else false</returns>
        bool TryPeek(T[] peekItems, int offset, int length);
    }
}