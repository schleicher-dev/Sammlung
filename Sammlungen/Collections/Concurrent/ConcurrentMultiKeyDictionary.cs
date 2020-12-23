using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Sammlungen.Collections.Concurrent
{
    public class ConcurrentMultiKeyDictionary<TKey, TValue> : MultiKeyDictionaryBase<TKey, TValue> where TValue : class
    {
        public ConcurrentMultiKeyDictionary() : this(EqualityComparer<TKey>.Default) { }

        public ConcurrentMultiKeyDictionary(IEqualityComparer<TKey> comparer) : this(1, comparer) { }

        public ConcurrentMultiKeyDictionary(IDictionary<TKey, TValue> values) :
            this(values, EqualityComparer<TKey>.Default) { }

        public ConcurrentMultiKeyDictionary(IDictionary<TKey, TValue> values, IEqualityComparer<TKey> comparer)
            : this(values.AsEnumerable(), comparer) { }
        
        public ConcurrentMultiKeyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values) 
            : this(values, EqualityComparer<TKey>.Default) {}

        public ConcurrentMultiKeyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey> comparer)
            : this(values.ToList(), comparer) { }

        private ConcurrentMultiKeyDictionary(ICollection<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey> comparer)
            : this(values.Count, comparer)
        {
            foreach (var value in values)
                Add(value);
        }

        public ConcurrentMultiKeyDictionary(int capacity) : this(capacity, EqualityComparer<TKey>.Default) { }

        public ConcurrentMultiKeyDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(new ConcurrentDictionary<TKey, TValue>(1, capacity, comparer)) {}
    }
}