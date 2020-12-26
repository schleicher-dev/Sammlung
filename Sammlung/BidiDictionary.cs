using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sammlung.Interfaces;

namespace Sammlung
{
    /// <summary>
    /// This <see cref="BidiDictionary{TForward,TReverse}"/> implements the
    /// <seealso cref="IBidiDictionary{TForward,TReverse}"/> interface. The type is explicitly not multi-threading
    /// compatible.
    /// </summary>
    /// <typeparam name="TForward">the forward type</typeparam>
    /// <typeparam name="TReverse">the reverse type</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = Justifications.PublicApiJustification)]
    public class BidiDictionary<TForward, TReverse>
        : BidiDictionaryBase<Dictionary<TForward, TReverse>, Dictionary<TReverse, TForward>, TForward, TReverse>
    {
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
            : base(new Dictionary<TForward, TReverse>(capacity, fwdComparer),
                new Dictionary<TReverse, TForward>(capacity, revComparer)) { }
    }
}