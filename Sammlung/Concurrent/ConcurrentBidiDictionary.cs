using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sammlung.Interfaces;

namespace Sammlung.Concurrent
{
    /// <summary>
    /// This <see cref="ConcurrentBidiDictionary{TForward,TReverse}"/> is a thread-safe variant of the
    /// <seealso cref="IBidiDictionary{TForward,TReverse}"/> interface.
    /// </summary>
    /// <typeparam name="TForward">the forward type</typeparam>
    /// <typeparam name="TReverse">the reverse type</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = Justifications.PublicApiJustification)]
    public class ConcurrentBidiDictionary<TForward, TReverse> 
        : BidiDictionaryBase<ConcurrentDictionary<TForward, TReverse>, ConcurrentDictionary<TReverse, TForward>, TForward, TReverse>
    {
        /// <summary>
        /// Constructs a new <see cref="ConcurrentBidiDictionary{TForward,TReverse}"/> using the default
        /// concurrency level 1.
        /// </summary>
        public ConcurrentBidiDictionary() : this(1) { }
        
        /// <summary>
        /// Constructs a new <see cref="ConcurrentBidiDictionary{TForward,TReverse}"/> using the passed concurrency level.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        public ConcurrentBidiDictionary(int concurrencyLevel) : this(concurrencyLevel, 1) {}

        /// <summary>
        /// Constructs a new <see cref="ConcurrentBidiDictionary{TForward,TReverse}"/> using the passed concurrency
        /// level and dictionary.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="other">the dictionary</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when mapping contains duplicate keys</exception>
        public ConcurrentBidiDictionary(int concurrencyLevel, IDictionary<TForward, TReverse> other)
            : this(concurrencyLevel, other, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentBidiDictionary{TForward,TReverse}"/> using the passed concurrency
        /// level and dictionary and comparers.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="other">the dictionary</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when mapping contains duplicate keys</exception>
        public ConcurrentBidiDictionary(int concurrencyLevel, IDictionary<TForward, TReverse> other, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer) 
            : this(concurrencyLevel, other.Count, fwdComparer, revComparer)
        {
            foreach (var (key, value) in other)
                this.Add(key, value);
        }
        
        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed concurrency level and
        /// enumerable. 
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="other">the enumerable</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when mapping contains duplicate keys</exception>
        public ConcurrentBidiDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TForward, TReverse>> other)
            : this(concurrencyLevel, other, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed concurrency level and
        /// enumerable and the comparers. 
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="other">the enumerable</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when mapping contains duplicate keys</exception>
        public ConcurrentBidiDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TForward, TReverse>> other,
            IEqualityComparer<TForward> fwdComparer, IEqualityComparer<TReverse> revComparer)
            : this(concurrencyLevel, other.ToDictionary(kv => kv.Key, kv => kv.Value), fwdComparer, revComparer) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed concurrency level and
        /// capacity.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="capacity">the capacity</param>
        public ConcurrentBidiDictionary(int concurrencyLevel, int capacity)
            : this(concurrencyLevel, capacity, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }
        
        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed concurrency level and
        /// capacity and comparers.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="capacity">the capacity</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        public ConcurrentBidiDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer) 
            : base(new ConcurrentDictionary<TForward, TReverse>(1, capacity, fwdComparer),
                new ConcurrentDictionary<TReverse, TForward>(concurrencyLevel, capacity, revComparer))
        {
            _lockHandle = new object();
        }
        
        private readonly object _lockHandle;

        /// <inheritdoc />
        public override TReverse this[TForward key]
        {
            set
            {
                lock (_lockHandle)
                {
                    base[key] = value;
                }
            }
        }

        /// <inheritdoc />
        public override bool TryAdd(TForward fwd, TReverse rev)
        {
            lock (_lockHandle)
            {
                return base.TryAdd(fwd, rev);
            }
        }

        /// <inheritdoc />
        public override bool ForwardRemove(TForward key)
        {
            lock (_lockHandle)
            {
                return base.ForwardRemove(key);
            }
        }

        /// <inheritdoc />
        public override bool ReverseRemove(TReverse key)
        {
            lock (_lockHandle)
            {
                return base.ReverseRemove(key);
            }
        }

        /// <inheritdoc />
        public override void Clear()
        {
            lock (_lockHandle)
            {
                base.Clear();
            }
        }
    }
}