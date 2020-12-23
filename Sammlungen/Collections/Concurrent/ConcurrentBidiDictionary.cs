using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Sammlungen.Collections.Concurrent
{
    public class ConcurrentBidiDictionary<TForward, TReverse> 
        : BidiDictionaryBase<ConcurrentDictionary<TForward, TReverse>, ConcurrentDictionary<TReverse, TForward>, TForward, TReverse>
    {
        public ConcurrentBidiDictionary() : this(1) { }

        public ConcurrentBidiDictionary(IDictionary<TForward, TReverse> other)
            : this(other, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        public ConcurrentBidiDictionary(IDictionary<TForward, TReverse> other, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer) : this(other.Count, fwdComparer, revComparer)
        {
            foreach (var (key, value) in other)
                this.Add(key, value);
        }

        public ConcurrentBidiDictionary(IEnumerable<KeyValuePair<TForward, TReverse>> other)
            : this(other, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        public ConcurrentBidiDictionary(IEnumerable<KeyValuePair<TForward, TReverse>> other,
            IEqualityComparer<TForward> fwdComparer, IEqualityComparer<TReverse> revComparer)
            : this(other.ToDictionary(kv => kv.Key, kv => kv.Value), fwdComparer, revComparer) { }

        public ConcurrentBidiDictionary(int capacity)
            : this(capacity, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        public ConcurrentBidiDictionary(int capacity, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer) 
            : base(new ConcurrentDictionary<TForward, TReverse>(1, capacity, fwdComparer),
                new ConcurrentDictionary<TReverse, TForward>(1, capacity, revComparer))
        {
            _lockHandle = new object();
        }
        
        private readonly object _lockHandle;

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

        public override bool TryAdd(TForward fwd, TReverse rev)
        {
            lock (_lockHandle)
            {
                return base.TryAdd(fwd, rev);
            }
        }

        public override bool ForwardRemove(TForward key)
        {
            lock (_lockHandle)
            {
                return base.ForwardRemove(key);
            }
        }

        public override bool ReverseRemove(TReverse key)
        {
            lock (_lockHandle)
            {
                return base.ReverseRemove(key);
            }
        }

        public override void Clear()
        {
            lock (_lockHandle)
            {
                base.Clear();
            }
        }
    }
}