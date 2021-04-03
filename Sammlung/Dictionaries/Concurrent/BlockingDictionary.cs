using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Dictionaries.Concurrent
{
    [PublicAPI]
    public class BlockingDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _innerDict;
        private readonly EnhancedReaderWriterLock _rwLock;

        public BlockingDictionary() : this(new Dictionary<TKey, TValue>()) { }
        
        public BlockingDictionary(int capacity) : this(new Dictionary<TKey, TValue>(capacity)) { }
        
        public BlockingDictionary(int capacity, IEqualityComparer<TKey> keyComparer) : 
            this(new Dictionary<TKey, TValue>(capacity, keyComparer)) { }

        public BlockingDictionary(IEqualityComparer<TKey> keyComparer) : 
            this(new Dictionary<TKey, TValue>(keyComparer)) { }
        
        public BlockingDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> keyComparer) : 
            this(new Dictionary<TKey, TValue>(dictionary, keyComparer)) { }
        
        public BlockingDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _innerDict = dictionary;
            _rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            using var _ = _rwLock.UseWriteLock();

            var snapshot = new Dictionary<TKey, TValue>(_innerDict);
            return snapshot.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            using var _ = _rwLock.UseWriteLock();

            var snapshot = new Dictionary<TKey, TValue>(_innerDict);
            return snapshot.GetEnumerator();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            using var _ = _rwLock.UseWriteLock();
            _innerDict.Add(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            using var _ = _rwLock.UseWriteLock();
            _innerDict.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            using var _ = _rwLock.UseReadLock();
            return _innerDict.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using var _ = _rwLock.UseWriteLock();
            _innerDict.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            using var _ = _rwLock.UseWriteLock();
            return _innerDict.Remove(item);
        }

        int ICollection<KeyValuePair<TKey, TValue>>.Count
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _innerDict.Count;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _innerDict.IsReadOnly;
            }
        }

        bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
        {
            using var _ = _rwLock.UseReadLock();
            return _innerDict.ContainsKey(key);
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            using var _ = _rwLock.UseWriteLock();
            _innerDict.Add(key, value);
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            using var _ = _rwLock.UseWriteLock();
            return _innerDict.Remove(key);
        }

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            using var _ = _rwLock.UseReadLock();
            return _innerDict.TryGetValue(key, out value);
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _innerDict[key];
            }
            set
            {
                using var _ = _rwLock.UseWriteLock();
                _innerDict[key] = value;
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                var snapshot = new List<TKey>(_innerDict.Keys);
                return snapshot;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                var snapshot = new List<TValue>(_innerDict.Values);
                return snapshot;
            }
        }
    }
}