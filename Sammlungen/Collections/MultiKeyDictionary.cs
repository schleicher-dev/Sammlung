using System.Collections.Generic;
using System.Linq;

namespace Sammlungen.Collections
{
    public class MultiKeyDictionary<TKey, TValue> : MultiKeyDictionaryBase<TKey, TValue> where TValue : class
    {
        public MultiKeyDictionary() : this(EqualityComparer<TKey>.Default) { }

        public MultiKeyDictionary(IEqualityComparer<TKey> comparer) : this(1, comparer) { }

        public MultiKeyDictionary(IDictionary<TKey, TValue> values) :
            this(values, EqualityComparer<TKey>.Default) { }

        public MultiKeyDictionary(IDictionary<TKey, TValue> values, IEqualityComparer<TKey> comparer)
            : this(values.AsEnumerable(), comparer) { }
        
        public MultiKeyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values) 
            : this(values, EqualityComparer<TKey>.Default) {}

        public MultiKeyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey> comparer)
            : this(values.ToList(), comparer) { }

        private MultiKeyDictionary(ICollection<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey> comparer)
            : this(values.Count, comparer)
        {
            foreach (var value in values)
                Add(value);
        }

        public MultiKeyDictionary(int capacity) : this(capacity, EqualityComparer<TKey>.Default) { }

        public MultiKeyDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(new Dictionary<TKey, TValue>(capacity, comparer)) {}
    }
}