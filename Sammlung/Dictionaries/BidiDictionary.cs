using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sammlung.Compatibility;
using Sammlung.Utilities;

namespace Sammlung.Dictionaries
{
    /// <summary>
    /// This <see cref="BidiDictionary{TForward,TReverse}"/> implements the
    /// <seealso cref="IBidiDictionary{TForward,TReverse}"/> interface. The type is explicitly not multi-threading
    /// compatible.
    /// </summary>
    /// <typeparam name="TForward">the forward type</typeparam>
    /// <typeparam name="TReverse">the reverse type</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = Justifications.PublicApiJustification)]
    public class BidiDictionary<TForward, TReverse> : IBidiDictionary<TForward, TReverse>
    {
        private readonly IDictionary<TForward, TReverse> _forwardMap;
        private readonly IDictionary<TReverse, TForward> _reverseMap;

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> with the default capacity=1.
        /// </summary>
        public BidiDictionary() : this(1) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed dictionary.
        /// </summary>
        /// <param name="other">the dictionary</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when mapping contains duplicate keys</exception>
        public BidiDictionary(IDictionary<TForward, TReverse> other)
            : this(other, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed dictionary and comparers.
        /// </summary>
        /// <param name="other">the dictionary</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when mapping contains duplicate keys</exception>
        public BidiDictionary(IDictionary<TForward, TReverse> other, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer) : this(other.AsEnumerable(), fwdComparer, revComparer) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed enumerable. 
        /// </summary>
        /// <param name="other">the enumerable</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when mapping contains duplicate keys</exception>
        public BidiDictionary(IEnumerable<KeyValuePair<TForward, TReverse>> other)
            : this(other, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed enumerable and the
        /// comparers. 
        /// </summary>
        /// <param name="other">the enumerable</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when mapping contains duplicate keys</exception>
        public BidiDictionary(IEnumerable<KeyValuePair<TForward, TReverse>> other,
            IEqualityComparer<TForward> fwdComparer, IEqualityComparer<TReverse> revComparer)
            : this(other.ToList(), fwdComparer, revComparer)
        {
        }

        private BidiDictionary(ICollection<KeyValuePair<TForward, TReverse>> other,
            IEqualityComparer<TForward> fwdComparer, IEqualityComparer<TReverse> revComparer)
            : this(other.Count, fwdComparer, revComparer)
        {
            foreach (var (key, value) in other)
                this.Add(key, value);
        }
        
        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> with the passed capacity.
        /// </summary>
        /// <param name="capacity">the capacity</param>
        public BidiDictionary(int capacity)
            : this(capacity, EqualityComparer<TForward>.Default, EqualityComparer<TReverse>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> with the passed capacity and the comparers.
        /// </summary>
        /// <param name="capacity">the capacity</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        public BidiDictionary(int capacity, IEqualityComparer<TForward> fwdComparer,
            IEqualityComparer<TReverse> revComparer)
        {
            _forwardMap = new Dictionary<TForward, TReverse>(capacity, fwdComparer);
            _reverseMap = new Dictionary<TReverse, TForward>(capacity, revComparer);
            ForwardMap = ReadOnlyDictionary.Wrap(_forwardMap);
            ReverseMap = ReadOnlyDictionary.Wrap(_reverseMap);
        }

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => ForwardMap.Count;

        /// <inheritdoc />
        public bool IsReadOnly => _forwardMap.IsReadOnly;

        /// <inheritdoc />
        public ICollection<TForward> Keys => _forwardMap.Keys;

        /// <inheritdoc />
        public ICollection<TReverse> Values => _forwardMap.Values;

        /// <inheritdoc />
        public Compatibility.IReadOnlyDictionary<TForward, TReverse> ForwardMap { get; }

        /// <inheritdoc />
        public Compatibility.IReadOnlyDictionary<TReverse, TForward> ReverseMap { get; }

        /// <inheritdoc />
        public bool ContainsKey(TForward key) => _forwardMap.ContainsKey(key);

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TForward, TReverse> item) => _forwardMap.Contains(item);

        /// <inheritdoc />
        public bool Remove(TForward key) => _forwardMap.Remove(key);

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TForward, TReverse> item) => _forwardMap.Remove(item);

        /// <inheritdoc />
        public TReverse this[TForward key]
        {
            get => _forwardMap[key];
            set => Add(key, value);
        }
        /// <inheritdoc />
        public bool TryGetValue(TForward key, out TReverse value) => _forwardMap.TryGetValue(key, out value);
        
        /// <inheritdoc />
        public bool TryAdd(TForward fwd, TReverse rev)
        {
            if (_forwardMap.ContainsKey(fwd) || _reverseMap.ContainsKey(rev))
                return false;
            _forwardMap.Add(fwd, rev);
            _reverseMap.Add(rev, fwd);
            return true;
        }

        /// <inheritdoc />
        public void Add(TForward fwd, TReverse rev)
        {
            if (!TryAdd(fwd, rev))
                throw ExceptionsHelper.NewDuplicateKeyException(fwd);
        }
        
        /// <inheritdoc />
        public void Add(KeyValuePair<TForward, TReverse> item) => Add(item.Key, item.Value);

        /// <inheritdoc />
        public bool ForwardRemove(TForward key) =>
            _forwardMap.Remove(key, out var value) && _reverseMap.Remove(value);

        /// <inheritdoc />
        public bool ReverseRemove(TReverse key) =>
            _reverseMap.Remove(key, out var value) && _forwardMap.Remove(value);

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TForward, TReverse>[] array, int arrayIndex) =>
            _forwardMap.CopyTo(array, arrayIndex);
        
        /// <inheritdoc />
        public void Clear()
        {
            _forwardMap.Clear();
            _reverseMap.Clear();
        }

        public IEnumerator<KeyValuePair<TForward, TReverse>> GetEnumerator()
        {
            return _forwardMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
}