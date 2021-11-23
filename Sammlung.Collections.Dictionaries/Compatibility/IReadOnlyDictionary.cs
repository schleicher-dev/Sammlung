using System.Collections.Generic;

namespace Sammlung.Collections.Dictionaries.Compatibility
{
    /// <summary>
    /// The <see cref="IReadOnlyDictionary{TKey,TValue}"/> is a polyfill class to
    /// prevent write access to an ordinary dictionary.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public interface IReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Returns if the key is contained in the dictionary.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>true if contained else false</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Tries to get the value using a key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value">the value to be returned</param>
        /// <returns>true if values was found by the given key else false</returns>
        bool TryGetValue(TKey key, out TValue value);
        
        /// <summary>
        /// Returns the amount of elements in the dictionary.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Accesses the value at the passed key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <exception cref="KeyNotFoundException">When key was not found in the collection</exception>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Returns all keys in this dictionary.
        /// </summary>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Returns all values in this dictionary.
        /// </summary>
        IEnumerable<TValue> Values { get; }
    }
}