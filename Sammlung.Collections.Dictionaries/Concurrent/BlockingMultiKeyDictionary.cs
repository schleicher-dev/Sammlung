using System.Collections.Generic;
using System.Threading;
using Sammlung.Werkzeug;
using Sammlung.Werkzeug.Concurrent;

namespace Sammlung.Collections.Dictionaries.Concurrent
{
    /// <summary>
    /// The <see cref="BlockingMultiKeyDictionary{TKey,TValue}"/> is the thread-safe variant of the
    /// <seealso cref="IMultiKeyDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class BlockingMultiKeyDictionary<TKey, TValue> : MultiKeyDictionaryBase<TKey, TValue> where TValue : class
    {
        private readonly EnhancedReaderWriterLock _rwLock;

        /// <summary>
        /// Constructs a new <see cref="BlockingMultiKeyDictionary{TKey,TValue}"/>
        /// </summary>
        public BlockingMultiKeyDictionary() : this(new BlockingDictionary<TKey, TValue>()) { }

        /// <summary>
        /// Constructs a new <see cref="BlockingMultiKeyDictionary{TKey,TValue}"/> using the passed dictionary.
        /// </summary>
        /// <param name="dictionary">the dictionary</param>
        /// <param name="comparer">the key comparer</param>
        public BlockingMultiKeyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : this(new BlockingDictionary<TKey, TValue>(dictionary.RequireNotNull(nameof(dictionary)), comparer.RequireNotNull(nameof(comparer)))) { }

        /// <summary>
        /// Constructs a new <see cref="BlockingMultiKeyDictionary{TKey,TValue}"/> using the passed capacity.
        /// </summary>
        /// <param name="capacity">the capacity</param>
        public BlockingMultiKeyDictionary(int capacity)
            : this(new BlockingDictionary<TKey, TValue>(capacity, EqualityComparer<TKey>.Default)) { }

        /// <summary>
        /// Constructs a new <see cref="BlockingMultiKeyDictionary{TKey,TValue}"/> using the passed capacity and comparer.
        /// </summary>
        /// <param name="capacity">the capacity</param>
        /// <param name="comparer">the comparer.</param>
        public BlockingMultiKeyDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : this(new BlockingDictionary<TKey, TValue>(capacity, comparer.RequireNotNull(nameof(comparer))))
        {
        }

        /// <summary>
        /// Creates a new <see cref="BlockingMultiKeyDictionary{TKey,TValue}"/> using the passed dictionary.
        /// </summary>
        /// <param name="dictionary">the dictionary</param>
        public BlockingMultiKeyDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            _rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
        }

        /// <inheritdoc />
        public override void Add(TKey[] keys, TValue value)
        {
            using var _ = _rwLock.UseWriteLock();
            base.Add(keys, value);
        }
    }
}