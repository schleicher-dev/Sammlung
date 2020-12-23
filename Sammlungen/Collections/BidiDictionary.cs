using System.Collections.Generic;
using System.Linq;

namespace Sammlungen.Collections
{
    public class BidiDictionary<TForward, TReverse>
        : BidiDictionaryBase<Dictionary<TForward, TReverse>, Dictionary<TReverse, TForward>, TForward, TReverse>
    {
        public BidiDictionary() : this(1) { }

        public BidiDictionary(IDictionary<TForward, TReverse> other)
            : this(other, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        public BidiDictionary(IDictionary<TForward, TReverse> other, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer) : this(other.Count, fwdComparer, revComparer)
        {
            foreach (var (key, value) in other)
                this.Add(key, value);
        }

        public BidiDictionary(IEnumerable<KeyValuePair<TForward, TReverse>> other)
            : this(other, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        public BidiDictionary(IEnumerable<KeyValuePair<TForward, TReverse>> other,
            IEqualityComparer<TForward> fwdComparer, IEqualityComparer<TReverse> revComparer)
            : this(other.ToDictionary(kv => kv.Key, kv => kv.Value), fwdComparer, revComparer) { }

        public BidiDictionary(int capacity)
            : this(capacity, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        public BidiDictionary(int capacity, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer)
            : base(new Dictionary<TForward, TReverse>(capacity, fwdComparer),
                new Dictionary<TReverse, TForward>(capacity, revComparer)) { }
    }
}