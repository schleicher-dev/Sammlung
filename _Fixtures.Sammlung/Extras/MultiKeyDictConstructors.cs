using System;
using System.Collections.Generic;
using Sammlung.Dictionaries;

namespace _Fixtures.Sammlung.Extras
{
    public delegate IMultiKeyDictionary<TKey, TValue> MultiKeyDictDefaultCtor<TKey, TValue>() where TValue : class;
    public delegate IMultiKeyDictionary<TKey, TValue> MultiKeyDictWithDictCtor<TKey, TValue>(IDictionary<TKey, TValue> d) where TValue : class;
    public delegate IMultiKeyDictionary<TKey, TValue> MultiKeyDictWithCapacityCtor<TKey, TValue>(int capacity) where TValue : class;
    [ExcludeFromCodeCoverage]
    public sealed class MultiKeyDictConstructors<TKey, TValue> : Tuple<MultiKeyDictDefaultCtor<TKey, TValue>,
        MultiKeyDictWithDictCtor<TKey, TValue>, MultiKeyDictWithCapacityCtor<TKey, TValue>> where TValue : class
    {
        public MultiKeyDictConstructors(MultiKeyDictDefaultCtor<TKey, TValue> item1,
            MultiKeyDictWithDictCtor<TKey, TValue> item2, MultiKeyDictWithCapacityCtor<TKey, TValue> item3) : base(item1, item2, item3) { }
    }
}