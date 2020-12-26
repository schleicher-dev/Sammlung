using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Bases;
using Sammlung.Interfaces;

namespace Sammlung.Concurrent
{
    /// <summary>
    /// The <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> is the thread-safe variant of the
    /// <seealso cref="IMultiKeyDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
    public class ConcurrentMultiKeyDictionary<TKey, TValue> : MultiKeyDictionaryBase<TKey, TValue> where TValue : class
    {
        private readonly object _lockHandle = new object();
        
        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the default concurrency
        /// level 1.
        /// </summary>
        public ConcurrentMultiKeyDictionary() : this(1) { }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the passed concurrency level.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <seealso cref="ConcurrentDictionary{TKey,TValue}"/>
        public ConcurrentMultiKeyDictionary(int concurrencyLevel)
            : this(concurrencyLevel, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the passed concurrency level
        /// and comparer.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="comparer">the comparer</param>
        public ConcurrentMultiKeyDictionary(int concurrencyLevel, IEqualityComparer<TKey> comparer)
            : this(concurrencyLevel, 1, comparer) { }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the passed concurrency level
        /// and values dictionary.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="values">the values dictionary</param>
        public ConcurrentMultiKeyDictionary(int concurrencyLevel, IDictionary<TKey, TValue> values) :
            this(concurrencyLevel, values, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the passed concurrency level
        /// and values dictionary and comparer.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="values">the values dictionary</param>
        /// <param name="comparer">the comparer</param>
        public ConcurrentMultiKeyDictionary(int concurrencyLevel, IDictionary<TKey, TValue> values,
            IEqualityComparer<TKey> comparer)
            : this(concurrencyLevel, values.AsEnumerable(), comparer) { }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the passed concurrency level
        /// and values enumerable and comparer.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="values">the values enumerable</param>
        public ConcurrentMultiKeyDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> values)
            : this(concurrencyLevel, values, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the passed concurrency level
        /// and values dictionary and comparer.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="values">the values enumerable</param>
        /// <param name="comparer">the comparer</param>
        public ConcurrentMultiKeyDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> values,
            IEqualityComparer<TKey> comparer)
            : this(concurrencyLevel, values.ToList(), comparer) { }

        private ConcurrentMultiKeyDictionary(int concurrencyLevel, ICollection<KeyValuePair<TKey, TValue>> values,
            IEqualityComparer<TKey> comparer)
            : this(concurrencyLevel, values.Count, comparer)
        {
            foreach (var value in values)
                Add(value);
        }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the passed concurrency level
        /// and capacity.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="capacity">the capacity</param>
        public ConcurrentMultiKeyDictionary(int concurrencyLevel, int capacity)
            : this(concurrencyLevel, capacity, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="ConcurrentMultiKeyDictionary{TKey,TValue}"/> using the passed concurrency level
        /// and capacity and comparer.
        /// </summary>
        /// <param name="concurrencyLevel">the concurrency level</param>
        /// <param name="capacity">the capacity</param>
        /// <param name="comparer">the comparer.</param>
        public ConcurrentMultiKeyDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer)
            : base(new ConcurrentDictionary<TKey, TValue>(concurrencyLevel, capacity, comparer)) { }

        /// <inheritdoc />
        public override TValue this[params TKey[] keys]
        {
            set
            {
                lock (_lockHandle)
                {
                    base[keys] = value;
                }
            }
        }

        /// <inheritdoc />
        public override void Add(TKey[] keys, TValue value)
        {
            lock (_lockHandle)
            {
                base.Add(keys, value);
            }
        }
    }
}