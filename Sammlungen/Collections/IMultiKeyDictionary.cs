using System.Collections.Generic;

namespace Sammlungen.Collections
{
    /// <summary>
    /// The <see cref="IMultiKeyDictionary{TKey,TValue}"/> extends the ability of a
    /// <seealso cref="IDictionary{TKey,TValue}"/> by the ability to add elements using multiple keys.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IMultiKeyDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// Add a single value using multiple keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="value">The values.</param>
        void Add(TKey[] keys, TValue value);
        
        /// <summary>
        /// Adds a single value to multiple keys.
        /// </summary>
        /// <param name="keys">the keys.</param>
        TValue this[params TKey[] keys] { set; }
    }
}