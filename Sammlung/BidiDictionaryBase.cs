using System;
using System.Collections.Generic;
using Sammlung.Exceptions;
using Sammlung.Interfaces;

namespace Sammlung
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

        /// <inheritdoc />
        public virtual IReadOnlyDictionary<TForward, TReverse> ForwardMap => _forwardMap;

        /// <inheritdoc />
        public virtual IReadOnlyDictionary<TReverse, TForward> ReverseMap => _reverseMap;

        /// <inheritdoc />
        public virtual int Count => InternalFwdMap.Count;

        /// <inheritdoc />
        public virtual TReverse this[TForward key]
        {
            set => this.Add(key, value);
        }
        
        /// <inheritdoc />
        public virtual void Add(TForward fwd, TReverse rev)
        {
            if (!TryAdd(fwd, rev))
                throw new DuplicateKeyException($"Either forward key '{fwd}' or reverse key '{rev}'" +
                                                $"are already in the {GetType().FullName}");
        }

        /// <inheritdoc />
        public virtual bool TryAdd(TForward fwd, TReverse rev)
        {
            if (InternalFwdMap.ContainsKey(fwd) || InternalRevMap.ContainsKey(rev))
                return false;
            InternalFwdMap[fwd] = rev;
            InternalRevMap[rev] = fwd;
            return true;
        }

        /// <inheritdoc />
        public virtual bool ForwardRemove(TForward key) =>
            InternalFwdMap.Remove(key, out var value) && InternalRevMap.Remove(value);

        /// <inheritdoc />
        public virtual bool ReverseRemove(TReverse key) =>
            InternalRevMap.Remove(key, out var value) && InternalFwdMap.Remove(value);

        /// <inheritdoc />
        public virtual void Clear()
        {
            InternalFwdMap.Clear();
            InternalRevMap.Clear();
        }
    }
}