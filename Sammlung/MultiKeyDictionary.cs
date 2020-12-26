﻿using System.Collections.Generic;
using System.Linq;
using Sammlung.Bases;

namespace Sammlung
{
    /// <summary>
    /// The <see cref="MultiKeyDictionary{TKey,TValue}"/> type implements an <seealso cref="IDictionary{TKey,TValue}"/>
    /// with two extra methods for adding of multiple keys to a single value.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
    public class MultiKeyDictionary<TKey, TValue> : MultiKeyDictionaryBase<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// Constructs a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using the default
        /// <seealso cref="IEqualityComparer{TKey}"/> for the key.
        /// </summary>
        public MultiKeyDictionary() : this(EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using the passed key comparer.
        /// </summary>
        /// <param name="comparer">the comparer for the key</param>
        public MultiKeyDictionary(IEqualityComparer<TKey> comparer) : this(1, comparer) { }

        /// <summary>
        /// Constructs a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using the passed dictionary.
        /// </summary>
        /// <param name="values">the values dictionary</param>
        public MultiKeyDictionary(IDictionary<TKey, TValue> values) :
            this(values, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using the passed values and comparer.
        /// </summary>
        /// <param name="values">the values dictionary</param>
        /// <param name="comparer">the comparer</param>
        public MultiKeyDictionary(IDictionary<TKey, TValue> values, IEqualityComparer<TKey> comparer)
            : this(values.AsEnumerable(), comparer) { }
        
        /// <summary>
        /// Constructs a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using the passed values.
        /// </summary>
        /// <param name="values">the values enumerable</param>
        public MultiKeyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values) 
            : this(values, EqualityComparer<TKey>.Default) {}

        /// <summary>
        /// Constructs a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using the passed values and comparer.
        /// </summary>
        /// <param name="values">the values enumerable</param>
        /// <param name="comparer">the comparer</param>
        public MultiKeyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey> comparer)
            : this(values.ToList(), comparer) { }

        private MultiKeyDictionary(ICollection<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey> comparer)
            : this(values.Count, comparer)
        {
            foreach (var value in values)
                Add(value);
        }

        /// <summary>
        /// Constructs a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using the passed capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public MultiKeyDictionary(int capacity) : this(capacity, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="MultiKeyDictionary{TKey,TValue}"/> using the passed capacity and comparer.
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public MultiKeyDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(new Dictionary<TKey, TValue>(capacity, comparer)) {}
    }
}