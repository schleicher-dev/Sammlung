using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Dictionaries.Concurrent
{
    /// <summary>
    /// The <see cref="BlockingDictionary{TKey,TValue}"/> is a primitive variant to the concurrent dictionary
    /// available since .NET Framework 4.0.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
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
        
        /// <summary>
        /// Creates a new 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keyComparer"></param>
        public BlockingDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> keyComparer) : 
            this(new Dictionary<TKey, TValue>(dictionary, keyComparer)) { }
        
        /// <summary>
        /// Creates a new <see cref="BlockingDictionary{TKey,TValue}"/> using the passed dicitionary.
        /// </summary>
        /// <param name="dictionary">the dictionary to use</param>
        public BlockingDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _innerDict = new Dictionary<TKey, TValue>(dictionary);
            _rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            using var _ = _rwLock.UseWriteLock();

            var snapshot = new Dictionary<TKey, TValue>(_innerDict);
            return snapshot.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            using var _ = _rwLock.UseWriteLock();

            var snapshot = new Dictionary<TKey, TValue>(_innerDict);
            return snapshot.GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            using var _ = _rwLock.UseWriteLock();
            _innerDict.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            using var _ = _rwLock.UseWriteLock();
            _innerDict.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            using var _ = _rwLock.UseReadLock();
            return _innerDict.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using var _ = _rwLock.UseWriteLock();
            _innerDict.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            using var _ = _rwLock.UseWriteLock();
            return _innerDict.Remove(item);
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _innerDict.Count;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _innerDict.IsReadOnly;
            }
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            using var _ = _rwLock.UseReadLock();
            return _innerDict.ContainsKey(key);
        }

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            using var _ = _rwLock.UseWriteLock();
            _innerDict.Add(key, value);
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            using var _ = _rwLock.UseWriteLock();
            return _innerDict.Remove(key);
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            using var _ = _rwLock.UseReadLock();
            return _innerDict.TryGetValue(key, out value);
        }

        /// <inheritdoc />
        public TValue this[TKey key]
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

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                var snapshot = new List<TKey>(_innerDict.Keys);
                return snapshot;
            }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values
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