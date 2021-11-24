using System.Collections.Generic;

namespace Sammlung.Collections.Dictionaries.Compatibility
{
    /// <summary>
    /// The <see cref="ReadOnlyDictionary"/> class exposes methods to wrap a given dictionary.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class ReadOnlyDictionary
    {
        /// <summary>
        /// Wraps the dictionary such that it acts like an read-only dictionary.
        /// </summary>
        /// <param name="dictionary">the dictionary</param>
        /// <typeparam name="TKey">the key type/</typeparam>
        /// <typeparam name="TValue">the value type</typeparam>
        /// <returns>the read-only variant of the dictionary</returns>
        public static IReadOnlyDictionary<TKey, TValue> Wrap<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) 
            => new ReadOnlyDictionaryAdapter<TKey, TValue>(dictionary);
    }
}