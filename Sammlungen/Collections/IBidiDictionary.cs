using System.Collections.Generic;
using Sammlungen.Exceptions;

namespace Sammlungen.Collections
{
    /// <summary>
    /// The <see cref="IBidiDictionary{TForward,TReverse}"/> type establishes a bidirectional mapping between two
    /// values.
    /// </summary>
    /// <typeparam name="TForward"></typeparam>
    /// <typeparam name="TReverse"></typeparam>
    public interface IBidiDictionary<TForward, TReverse>
    {
        /// <summary>
        /// Holds the forward map, which is used to retrieve the values in the forward direction.
        /// </summary>
        IReadOnlyDictionary<TForward, TReverse> ForwardMap { get; }
        
        /// <summary>
        /// Holds the reverse map, which is used to retrieve the values in the reverse direction.
        /// </summary>
        IReadOnlyDictionary<TReverse, TForward> ReverseMap { get; }
        
        /// <summary>
        /// Maps the forward key to the reverse key and vice versa.
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="Exceptions.DuplicateKeyException">when a duplicate forward or reverse key is added</exception>
        TReverse this[TForward key] { set; }

        /// <summary>
        /// Returns the count of the map. This value is always equal to the size of the forward and reverse mapping.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Adds the mapping between forward and reverse key.
        /// </summary>
        /// <param name="fwd">the forward key</param>
        /// <param name="rev">the reverse key</param>
        /// <exception cref="Exceptions.DuplicateKeyException">when a duplicate forward or reverse key is added</exception>
        void Add(TForward fwd, TReverse rev);
        
        /// <summary>
        /// Tries to add the mapping between forward and reverse key.
        /// </summary>
        /// <param name="fwd">the forward key</param>
        /// <param name="rev">the reverse key</param>
        /// <returns>true if no duplicate key found else false</returns>
        bool TryAdd(TForward fwd, TReverse rev);

        /// <summary>
        /// Removes a particular mapping between values using the forward key.
        /// </summary>
        /// <param name="key">the forward key</param>
        /// <returns>true if remove was successful else false</returns>
        bool ForwardRemove(TForward key);
        
        /// <summary>
        /// Removes a particular mapping between values using the reverse key.
        /// </summary>
        /// <param name="key">the reverse key</param>
        /// <returns>true if remove was successful else false</returns>
        bool ReverseRemove(TReverse key);

        /// <summary>
        /// Empties the collection.
        /// </summary>
        void Clear();
    }
}