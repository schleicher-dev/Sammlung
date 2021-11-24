using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Collections.Dictionaries.Compatibility;
using Sammlung.Collections.Dictionaries.Resources;
using Sammlung.Werkzeug;

namespace Sammlung.Collections.Dictionaries
{
    /// <summary>
    /// This <see cref="BidiDictionary{TForward,TReverse}"/> implements the
    /// <seealso cref="IBidiDictionary{TForward,TReverse}"/> interface. The type is explicitly not multi-threading
    /// compatible.
    /// </summary>
    /// <typeparam name="TFwd">the forward type</typeparam>
    /// <typeparam name="TRev">the reverse type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class BidiDictionary<TFwd, TRev> : IBidiDictionary<TFwd, TRev>
    {
        private readonly IDictionary<TFwd, TRev> _forwardMap;
        private readonly IDictionary<TRev, TFwd> _reverseMap;

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> with the default capacity=1.
        /// </summary>
        public BidiDictionary() : this(1) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed dictionary.
        /// </summary>
        /// <param name="other">the dictionary</param>
        /// <exception cref="ArgumentException">when mapping contains duplicate keys</exception>
        public BidiDictionary(IDictionary<TFwd, TRev> other)
            : this(other.RequireNotNull(nameof(other)), EqualityComparer<TFwd>.Default, EqualityComparer<TRev>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed dictionary and comparers.
        /// </summary>
        /// <param name="other">the dictionary</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        /// <exception cref="ArgumentException">when mapping contains duplicate keys</exception>
        public BidiDictionary(IDictionary<TFwd, TRev> other, IEqualityComparer<TFwd> fwdComparer,
            IEqualityComparer<TRev> revComparer) : this(other.RequireNotNull(nameof(other)).AsEnumerable(), fwdComparer.RequireNotNull(nameof(fwdComparer)), revComparer.RequireNotNull(nameof(revComparer))) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed enumerable. 
        /// </summary>
        /// <param name="other">the enumerable</param>
        /// <exception cref="ArgumentException">when mapping contains duplicate keys</exception>
        public BidiDictionary(IEnumerable<KeyValuePair<TFwd, TRev>> other)
            : this(other.RequireNotNull(nameof(other)), EqualityComparer<TFwd>.Default, EqualityComparer<TRev>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> using the passed enumerable and the
        /// comparers. 
        /// </summary>
        /// <param name="other">the enumerable</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        /// <exception cref="ArgumentException">when mapping contains duplicate keys</exception>
        public BidiDictionary(IEnumerable<KeyValuePair<TFwd, TRev>> other,
            IEqualityComparer<TFwd> fwdComparer, IEqualityComparer<TRev> revComparer)
            : this(other.RequireNotNull(nameof(other)).ToList(), fwdComparer.RequireNotNull(nameof(fwdComparer)), revComparer.RequireNotNull(nameof(revComparer)))
        {
        }

        private BidiDictionary(ICollection<KeyValuePair<TFwd, TRev>> other,
            IEqualityComparer<TFwd> fwdComparer, IEqualityComparer<TRev> revComparer)
            : this(other.RequireNotNull(nameof(other)).Count, fwdComparer.RequireNotNull(nameof(fwdComparer)), revComparer.RequireNotNull(nameof(revComparer)))
        {
            foreach (var item in other) Add(item.Key, item.Value);
        }
        
        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> with the passed capacity.
        /// </summary>
        /// <param name="capacity">the capacity</param>
        public BidiDictionary(int capacity)
            : this(capacity, EqualityComparer<TFwd>.Default, EqualityComparer<TRev>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="BidiDictionary{TForward,TReverse}"/> with the passed capacity and the comparers.
        /// </summary>
        /// <param name="capacity">the capacity</param>
        /// <param name="fwdComparer">the comparer of the forward key</param>
        /// <param name="revComparer">the comparer of the reverse key</param>
        public BidiDictionary(int capacity, IEqualityComparer<TFwd> fwdComparer,
            IEqualityComparer<TRev> revComparer)
        {
            _forwardMap = new Dictionary<TFwd, TRev>(capacity, fwdComparer.RequireNotNull(nameof(fwdComparer)));
            _reverseMap = new Dictionary<TRev, TFwd>(capacity, revComparer.RequireNotNull(nameof(revComparer)));
        }

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => ForwardMap.Count;

        /// <inheritdoc />
        public bool IsReadOnly => _forwardMap.IsReadOnly;

        /// <inheritdoc />
        public ICollection<TFwd> Keys => _forwardMap.Keys;

        /// <inheritdoc />
        public ICollection<TRev> Values => _forwardMap.Values;

        /// <inheritdoc />
        public Compatibility.IReadOnlyDictionary<TFwd, TRev> ForwardMap => _forwardMap.Wrap();

        /// <inheritdoc />
        public Compatibility.IReadOnlyDictionary<TRev, TFwd> ReverseMap => _reverseMap.Wrap();

        /// <inheritdoc />
        public bool ContainsKey(TFwd key) => _forwardMap.ContainsKey(key);

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TFwd, TRev> item) => _forwardMap.Contains(item);

        /// <inheritdoc />
        public bool Remove(TFwd key) => _forwardMap.Remove(key);

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TFwd, TRev> item) => _forwardMap.Remove(item);

        /// <inheritdoc />
        public TRev this[TFwd key]
        {
            get => _forwardMap[key];
            set => Add(key, value);
        }
        /// <inheritdoc />
        public bool TryGetValue(TFwd key, out TRev value) => _forwardMap.TryGetValue(key, out value);
        
        /// <inheritdoc />
        public bool TryAdd(TFwd fwd, TRev rev)
        {
            if (_forwardMap.ContainsKey(fwd) || _reverseMap.ContainsKey(rev))
                return false;
            _forwardMap.Add(fwd, rev);
            _reverseMap.Add(rev, fwd);
            return true;
        }

        /// <inheritdoc />
        public void Add(TFwd fwd, TRev rev)
        {
            if (!TryAdd(fwd, rev))
                throw new ArgumentException(string.Format(ErrorMessages.DuplicateKeyError, $"{fwd} or {rev}"), $"{nameof(fwd)} or {nameof(rev)}");
        }
        
        /// <inheritdoc />
        public void Add(KeyValuePair<TFwd, TRev> item) => Add(item.Key, item.Value);

        /// <inheritdoc />
        public bool ForwardRemove(TFwd key) => 
            _forwardMap.TryGetValue(key, out var value) && _forwardMap.Remove(key) && _reverseMap.Remove(value);

        /// <inheritdoc />
        public bool ReverseRemove(TRev key) =>
            _reverseMap.TryGetValue(key, out var value) && _reverseMap.Remove(key) && _forwardMap.Remove(value);

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TFwd, TRev>[] array, int arrayIndex) =>
            _forwardMap.CopyTo(array, arrayIndex);
        
        /// <inheritdoc />
        public void Clear()
        {
            _forwardMap.Clear();
            _reverseMap.Clear();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TFwd, TRev>> GetEnumerator()
        {
            return _forwardMap.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
}