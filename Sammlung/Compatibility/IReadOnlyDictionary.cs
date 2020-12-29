namespace Sammlung.Compatibility
{
    /// <summary>
    /// T
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IReadOnlyDictionary<TKey, TValue> : 
        IReadOnlyCollection<System.Collections.Generic.KeyValuePair<TKey, TValue>>
    {
        bool ContainsKey(TKey key);

        bool TryGetValue(TKey key, out TValue value);

        TValue this[TKey key] { get; }

        System.Collections.Generic.IEnumerable<TKey> Keys { get; }

        System.Collections.Generic.IEnumerable<TValue> Values { get; }
    }
}