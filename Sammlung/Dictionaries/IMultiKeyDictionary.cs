using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sammlung.Dictionaries
{
    /// <summary>
    /// The <see cref="IMultiKeyDictionary{TKey,TValue}"/> extends the ability of a
    /// <seealso cref="IDictionary{TKey,TValue}"/> by the ability to add elements using multiple keys.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
    [JetBrains.Annotations.PublicAPI]
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
        new TValue this[TKey keys] { get; set; }
        
        /// <summary>
        /// Adds a single value to multiple keys.
        /// </summary>
        /// <param name="keys">the keys.</param>
        TValue this[params TKey[] keys] { set; }
    }
}