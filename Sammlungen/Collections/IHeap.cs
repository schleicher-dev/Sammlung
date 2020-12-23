namespace Sammlungen.Collections
{
    /// <summary>
    /// This is the abstract interface of a heap data structure.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the item type</typeparam>
    public interface IHeap<TKey, TValue>
    {
        /// <summary>
        /// Returns the number of items in this heap.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Returns the root node of the heap.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if there is a root node else false if the heap is empty.</returns>
        bool TryPeek(out TValue value);
        
        /// <summary>
        /// Removes the root node of the heap and returns it.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if there is a root node else false if the heap is empty.</returns>
        bool TryPop(out TValue value);

        /// <summary>
        /// Adds the item to the heap, using some sort of comparison.
        /// </summary>
        /// <param name="key">the key for the item</param>
        /// <param name="value">the value to add</param>
        void Push(TKey key, TValue value);

        /// <summary>
        /// Tries to pop the root item and add the new item.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value">the new item</param>
        /// <param name="oldValue">the popped item</param>
        bool TryReplace(TKey key, TValue value, out TValue oldValue);

        /// <summary>
        /// Tries to updated the key of the passed item.
        /// </summary>
        /// <param name="item">the item</param>
        /// <param name="key">the new key</param>
        bool TryUpdate(TValue item, TKey key);
    }
}