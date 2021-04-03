
namespace Sammlung.Dictionaries
{
    /// <summary>
    /// The <see cref="IBidiDictionary{TForward,TReverse}"/> type establishes a bidirectional mapping between two
    /// values.
    /// </summary>
    /// <typeparam name="TForward"></typeparam>
    /// <typeparam name="TReverse"></typeparam>
    public interface IBidiDictionary<TForward, TReverse> : 
        System.Collections.Generic.IDictionary<TForward, TReverse>
    {
        /// <summary>
        /// Holds the forward map, which is used to retrieve the values in the forward direction.
        /// </summary>
        Compatibility.IReadOnlyDictionary<TForward, TReverse> ForwardMap { get; }
        
        /// <summary>
        /// Holds the reverse map, which is used to retrieve the values in the reverse direction.
        /// </summary>
        Compatibility.IReadOnlyDictionary<TReverse, TForward> ReverseMap { get; }

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
    }
}