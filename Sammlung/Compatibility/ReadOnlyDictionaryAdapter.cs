using System.Collections;
using System.Collections.Generic;
using Sammlung.Utilities;

namespace Sammlung.Compatibility
{
    /// <summary>
    /// The <see cref="ReadOnlyDictionaryAdapter{TKey,TValue}"/> adapts a given <seealso cref="IDictionary{TKey,TValue}"/>
    /// to the <seealso cref="IReadOnlyDictionary{TKey,TValue}"/> type.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
    internal class ReadOnlyDictionaryAdapter<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _impl;
        
        /// <summary>
        /// Constructs a new instance of a readonly dictionary using an <seealso cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="impl">the implementation</param>
        public ReadOnlyDictionaryAdapter(IDictionary<TKey, TValue> impl)
        {
            _impl = impl.RequireNotNull(nameof(impl));
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _impl.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public int Count => _impl.Count;

        /// <inheritdoc />
        public bool ContainsKey(TKey key) => _impl.ContainsKey(key);

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value) => _impl.TryGetValue(key, out value);

        /// <inheritdoc />
        public TValue this[TKey key] => _impl[key];

        /// <inheritdoc />
        public IEnumerable<TKey> Keys => _impl.Keys;

        /// <inheritdoc />
        public IEnumerable<TValue> Values => _impl.Values;
    }
}