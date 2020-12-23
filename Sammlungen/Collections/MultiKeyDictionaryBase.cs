using System;
using System.Collections;
using System.Collections.Generic;

namespace Sammlungen.Collections
{
    public abstract class MultiKeyDictionaryBase<TKey, TValue> : IMultiKeyDictionary<TKey, TValue> where TValue : class
    {
        protected MultiKeyDictionaryBase(IDictionary<TKey, TValue> innerDict)
        {
            _innerDict = innerDict ?? throw new ArgumentNullException(nameof(innerDict));
        }
        
        private readonly IDictionary<TKey, TValue> _innerDict;
        
        public int Count => _innerDict.Count;

        public bool IsReadOnly => _innerDict.IsReadOnly;

        public TValue this[TKey key]
        {
            get => _innerDict[key];
            set => _innerDict[key] = value;
        }
        public TValue this[params TKey[] keys]
        {
            set
            {
                foreach (var key in keys) _innerDict[key] = value;
            }
        }
        
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _innerDict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _innerDict).GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item) => _innerDict.Add(item);

        public void Clear() => _innerDict.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => _innerDict.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            _innerDict.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => _innerDict.Remove(item);
        

        public void Add(TKey key, TValue value) => _innerDict.Add(key, value);

        public void Add(TKey[] keys, TValue value) => this[keys] = value;

        public bool ContainsKey(TKey key) => _innerDict.ContainsKey(key);

        public bool Remove(TKey key) => _innerDict.Remove(key);

        public bool TryGetValue(TKey key, out TValue value) => _innerDict.TryGetValue(key, out value);

        public ICollection<TKey> Keys => _innerDict.Keys;

        public ICollection<TValue> Values => _innerDict.Values;
    }
}