using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sammlung.Utilities;

namespace Sammlung.Dictionaries
{
    /// <summary>
    /// The <see cref="MultiKeyDictionaryBase{TKey,TValue}"/> type delegates the given methods to an initially
    /// provided <seealso cref="IDictionary{TKey,TValue}"/> class.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "PublicAPI")]
    public abstract class MultiKeyDictionaryBase<TKey, TValue> : IMultiKeyDictionary<TKey, TValue> where TValue : class
    {
        private readonly IDictionary<TKey, TValue> _innerDict;
        
        /// <summary>
        /// Creates a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using an inner dictionary.
        /// </summary>
        /// <param name="innerDict">the inner dictionary</param>
        protected MultiKeyDictionaryBase(IDictionary<TKey, TValue> innerDict)
        {
            _innerDict = innerDict.RequireNotNull(nameof(innerDict));
        }
        
        /// <inheritdoc />
        public int Count => _innerDict.Count;

        /// <inheritdoc />
        public bool IsReadOnly => _innerDict.IsReadOnly;

        /// <inheritdoc cref="IMultiKeyDictionary{TKey, TValue}.this[TKey]"/>
        public TValue this[TKey key]
        {
            get => _innerDict[key];
            set => _innerDict[key] = value;
        }
        
        /// <inheritdoc />
        public virtual TValue this[params TKey[] keys]
        {
            set
            {
                foreach (var key in keys) _innerDict[key] = value;
            }
        }

        /// <inheritdoc />
        public virtual void Add(TKey[] keys, TValue value) => this[keys] = value;

        #region IDictionary

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _innerDict.GetEnumerator();
        
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _innerDict).GetEnumerator();
        
        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item) => _innerDict.Add(item);

        /// <inheritdoc />
        public void Clear() => _innerDict.Clear();

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item) => _innerDict.Contains(item);

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            _innerDict.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item) => _innerDict.Remove(item);

        /// <inheritdoc />
        public void Add(TKey key, TValue value) => _innerDict.Add(key, value);

        /// <inheritdoc />
        public bool ContainsKey(TKey key) => _innerDict.ContainsKey(key);

        /// <inheritdoc />
        public bool Remove(TKey key) => _innerDict.Remove(key);

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value) => _innerDict.TryGetValue(key, out value);

        /// <inheritdoc />
        public ICollection<TKey> Keys => _innerDict.Keys;

        /// <inheritdoc />
        public ICollection<TValue> Values => _innerDict.Values;

        #endregion
    }
}