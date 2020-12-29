using System.Collections.Generic;

namespace Sammlung.Compatibility
{
    public class ReadOnlyDictionary
    {
        public static IReadOnlyDictionary<TKey, TValue> Wrap<TKey, TValue>(IDictionary<TKey, TValue> dictionary) 
            => new ReadOnlyDictionaryAdapter<TKey, TValue>(dictionary);
    }
}