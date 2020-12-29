namespace Sammlung.Heaps
{
    /// <summary>
    /// This is the abstract interface of a heap data structure.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the item type</typeparam>
    public interface IHeap<in TKey, TValue>
    {
        /// <summary>
        /// Returns the number of items in this heap.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Checks if the heap is empty.
        /// </summary>
        /// <returns>true if empty else false</returns>
        bool IsEmpty();
        
        /// <summary>
        /// Checks if the heap has any elements.
        /// </summary>
        /// <returns>true if not empty else false</returns>
        bool Any();

        /// <summary>
        /// Returns the root node of the heap without removing it.
        /// </summary>
        /// <returns>the root element</returns>
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        TValue Peek();
        
        /// <summary>
        /// Returns the root node of the heap.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if there is a root node else false if the heap is empty.</returns>
        bool TryPeek(out TValue value);
        
        /// <summary>
        /// Removes the root node of the heap and returns it.
        /// </summary>
        /// <returns>the root element</returns>
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        TValue Pop();
        
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
        /// Pops the root element and adds the new item.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value">the value</param>
        /// <returns>the popped root element</returns>
        /// <exception cref="System.InvalidOperationException">when heap is empty</exception>
        TValue Replace(TKey key, TValue value);

        /// <summary>
        /// Tries to pop the root item and add the new item.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value">the new item</param>
        /// <param name="oldValue">the popped item</param>
        bool TryReplace(TKey key, TValue value, out TValue oldValue);
        
        /// <summary>
        /// Updates the key of the passed item.
        /// </summary>
        /// <param name="value">the item</param>
        /// <param name="key">the key</param>
        /// <exception cref="System.InvalidOperationException">when element was not found</exception>
        void Update(TValue value, TKey key);
        
        /// <summary>
        /// Tries to updated the key of the passed item.
        /// </summary>
        /// <param name="value">the item</param>
        /// <param name="key">the new key</param>
        bool TryUpdate(TValue value, TKey key);
    }
}