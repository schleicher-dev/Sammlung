using System;
using System.Collections.Generic;

namespace Sammlungen.Collections
{
    public abstract class BidiDictionaryBase<TFwdDict, TRevDict, TForward, TReverse> 
        : IBidiDictionary<TForward, TReverse>
        where TFwdDict : IDictionary<TForward, TReverse>, IReadOnlyDictionary<TForward, TReverse>
        where TRevDict : IDictionary<TReverse, TForward>, IReadOnlyDictionary<TReverse, TForward>
    {
        protected BidiDictionaryBase(TFwdDict fwdDict, TRevDict revDict)
        {
            _forwardMap = fwdDict ?? throw new ArgumentNullException(nameof(fwdDict));
            _reverseMap = revDict ?? throw new ArgumentNullException(nameof(revDict));
        }

        private readonly TFwdDict _forwardMap;
        private readonly TRevDict _reverseMap;

        private IDictionary<TForward, TReverse> InternalFwdMap => _forwardMap;

        private IDictionary<TReverse, TForward> InternalRevMap => _reverseMap;

        public virtual IReadOnlyDictionary<TForward, TReverse> ForwardMap => _forwardMap;

        public virtual IReadOnlyDictionary<TReverse, TForward> ReverseMap => _reverseMap;

        public virtual int Count => InternalFwdMap.Count;

        public virtual TReverse this[TForward key]
        {
            set
            {
                InternalFwdMap[key] = value;
                InternalRevMap[value] = key;
            }
        }

        public virtual bool TryAdd(TForward fwd, TReverse rev)
        {
            if (InternalFwdMap.ContainsKey(fwd) || InternalRevMap.ContainsKey(rev))
                return false;
            this[fwd] = rev;
            return true;
        }

        public virtual bool ForwardRemove(TForward key) =>
            InternalFwdMap.Remove(key, out var value) && InternalRevMap.Remove(value);

        public virtual bool ReverseRemove(TReverse key) =>
            InternalRevMap.Remove(key, out var value) && InternalFwdMap.Remove(value);

        public virtual void Clear()
        {
            InternalFwdMap.Clear();
            InternalRevMap.Clear();
        }
    }
}