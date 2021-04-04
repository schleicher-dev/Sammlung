using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sammlung.Compatibility
{
    /// <summary>
    /// The <see cref="ReadOnlyDictionary"/> class exposes methods to wrap a given dictionary.
    /// </summary>
    [PublicAPI]
    public class ReadOnlyDictionary
    {
        /// <summary>
        /// Wraps the dictionary such that it acts like an read-only dictionary.
        /// </summary>
        /// <param name="dictionary">the dictionary</param>
        /// <typeparam name="TKey">the key type/</typeparam>
        /// <typeparam name="TValue">the value type</typeparam>
        /// <returns>the read-only variant of the dictionary</returns>
        public static IReadOnlyDictionary<TKey, TValue> Wrap<TKey, TValue>(IDictionary<TKey, TValue> dictionary) 
            => new ReadOnlyDictionaryAdapter<TKey, TValue>(dictionary);
    }
}