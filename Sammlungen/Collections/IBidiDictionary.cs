using System.Collections.Generic;

namespace Sammlungen.Collections
{
    public interface IBidiDictionary<TForward, TReverse>
    {
        public IReadOnlyDictionary<TForward, TReverse> ForwardMap { get; }
        public IReadOnlyDictionary<TReverse, TForward> ReverseMap { get; }
        
        public TReverse this[TForward key] { set; }

        public int Count { get; }
        public bool TryAdd(TForward fwd, TReverse rev);

        public bool ForwardRemove(TForward key);
        public bool ReverseRemove(TReverse key);

        public void Clear();
    }
}