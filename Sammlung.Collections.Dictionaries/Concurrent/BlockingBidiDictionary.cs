using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sammlung.Collections.Dictionaries.Resources;
using Sammlung.Werkzeug;
using Sammlung.Werkzeug.Concurrent;

namespace Sammlung.Collections.Dictionaries.Concurrent
{
    /// <summary>
    /// This <see cref="BlockingBidiDictionary{TForward,TReverse}"/> is a thread-safe variant of the
    /// <seealso cref="IBidiDictionary{TForward,TReverse}"/> interface.
    /// </summary>
    /// <typeparam name="TForward">the forward type</typeparam>
    /// <typeparam name="TReverse">the reverse type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class BlockingBidiDictionary<TForward, TReverse> : IBidiDictionary<TForward, TReverse>
    {
        private readonly EnhancedReaderWriterLock _rwLock;
        private readonly BidiDictionary<TForward, TReverse> _internal;

        /// <summary>
        /// Constructs a new <see cref="BlockingBidiDictionary{TForward,TReverse}"/>.
        /// </summary>
        public BlockingBidiDictionary() : this(1) { }
        
        /// <summary>
        /// Constructs a new <see cref="BlockingBidiDictionary{TForward,TReverse}"/> using the passed concurrency
        /// level and dictionary.
        /// </summary>
        /// <param name="other">the dictionary</param>
        /// <exception cref="ArgumentException">when mapping contains duplicate keys</exception>
        public BlockingBidiDictionary(IDictionary<TForward, TReverse> other)
            : this(other.RequireNotNull(nameof(other)), EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BlockingBidiDictionary{TForward,TReverse}"/> using the passed concurrency
        /// level and dictionary and comparers.
        /// </summary>
        /// <param name="other">the dictionary</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        /// <exception cref="ArgumentException">when mapping contains duplicate keys</exception>
        public BlockingBidiDictionary(IDictionary<TForward, TReverse> other,
            IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer)
            : this(other.RequireNotNull(nameof(other)).Count, fwdComparer, revComparer)
        {
            foreach (var item in other)
                Add(item.Key, item.Value);
        }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed concurrency level and
        /// enumerable. 
        /// </summary>
        /// <param name="other">the enumerable</param>
        /// <exception cref="ArgumentException">when mapping contains duplicate keys</exception>
        public BlockingBidiDictionary(IEnumerable<KeyValuePair<TForward, TReverse>> other)
            : this(other.RequireNotNull(nameof(other)), EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed concurrency level and
        /// enumerable and the comparers. 
        /// </summary>
        /// <param name="other">the enumerable</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        /// <exception cref="ArgumentException">when mapping contains duplicate keys</exception>
        public BlockingBidiDictionary(IEnumerable<KeyValuePair<TForward, TReverse>> other,
            IEqualityComparer<TForward> fwdComparer, IEqualityComparer<TReverse> revComparer)
            : this(other.RequireNotNull(nameof(other)).ToDictionary(kv => kv.Key, kv => kv.Value),
                fwdComparer.RequireNotNull(nameof(fwdComparer)), revComparer.RequireNotNull(nameof(revComparer)))
        {
        }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed concurrency level and
        /// capacity.
        /// </summary>
        /// <param name="capacity">the capacity</param>
        public BlockingBidiDictionary(int capacity)
            : this(capacity, EqualityComparer<TForward>.Default,
                EqualityComparer<TReverse>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed concurrency level and
        /// capacity and comparers.
        /// </summary>
        /// <param name="capacity">the capacity</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        public BlockingBidiDictionary(int capacity, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer)
        {
            fwdComparer.RequireNotNull(nameof(fwdComparer));
            revComparer.RequireNotNull(nameof(revComparer));
            
            _rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
            _internal = new BidiDictionary<TForward, TReverse>(capacity, fwdComparer, revComparer);
        }
        
        /// <inheritdoc />
        public int Count
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _internal.Count;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _internal.IsReadOnly;
            }
        }

        /// <inheritdoc />
        public ICollection<TForward> Keys
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _internal.Keys;
            }
        }

        /// <inheritdoc />
        public ICollection<TReverse> Values
        {
            get { 
                using var _ = _rwLock.UseReadLock();
                return _internal.Values; }
        }

        /// <inheritdoc />
        public Compatibility.IReadOnlyDictionary<TForward, TReverse> ForwardMap => _internal.ForwardMap;

        /// <inheritdoc />
        public Compatibility.IReadOnlyDictionary<TReverse, TForward> ReverseMap => _internal.ReverseMap;

        /// <inheritdoc />
        public bool ContainsKey(TForward key)
        {
            using var _ = _rwLock.UseReadLock();
            return _internal.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TForward, TReverse> item)
        {
            using var _ = _rwLock.UseReadLock();
            return _internal.Contains(item);
        }

        /// <inheritdoc />
        public TReverse this[TForward key]
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _internal[key];
            }
            set
            {
                using var _ = _rwLock.UseWriteLock();
                _internal[key] = value;
            }
        }

        /// <inheritdoc />
        public bool TryAdd(TForward fwd, TReverse rev)
        {
            using var _ = _rwLock.UseWriteLock();
            return _internal.TryAdd(fwd, rev);
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TForward, TReverse> item) => Add(item.Key, item.Value);

        /// <inheritdoc />
        public void Add(TForward fwd, TReverse rev)
        {
            if (!TryAdd(fwd, rev))
                throw new ArgumentException(string.Format(ErrorMessages.DuplicateKeyError, $"{fwd} or {rev}"), $"{nameof(fwd)} or {nameof(rev)}");
        }
        
        /// <inheritdoc />
        public bool TryGetValue(TForward key, out TReverse value)
        {
            using var _ = _rwLock.UseReadLock();
            return _internal.TryGetValue(key, out value);
        }
        
        /// <inheritdoc />
        public bool Remove(KeyValuePair<TForward, TReverse> item)
        {
            using var _ = _rwLock.UseWriteLock();
            return _internal.Remove(item);
        }

        /// <inheritdoc />
        public bool Remove(TForward key)
        {
            using var _ = _rwLock.UseWriteLock();
            return _internal.Remove(key);
        }
        
        /// <inheritdoc />
        public bool ForwardRemove(TForward key)
        {
            using var _ = _rwLock.UseWriteLock();
            return _internal.ForwardRemove(key);
        }

        /// <inheritdoc />
        public bool ReverseRemove(TReverse key)
        {
            using var _ = _rwLock.UseWriteLock();
            return _internal.ReverseRemove(key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TForward, TReverse>[] array, int arrayIndex)
        {
            using var _ = _rwLock.UseReadLock();
            _internal.CopyTo(array, arrayIndex);
        }
        
        /// <inheritdoc />
        public void Clear()
        {
            using var _ = _rwLock.UseWriteLock();
            _internal.Clear();
        }
        
        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TForward, TReverse>> GetEnumerator()
        {
            using var _ = _rwLock.UseReadLock();
            var array = new KeyValuePair<TForward, TReverse>[_internal.Count];
            _internal.CopyTo(array, 0);
            return array.Select(t => t).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}