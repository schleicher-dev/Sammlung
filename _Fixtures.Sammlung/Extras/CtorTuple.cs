using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sammlung;
using Sammlung.Interfaces;

namespace _Fixtures.Sammlung.Extras
{
    [ExcludeFromCodeCoverage]
    public class CtorTuple<TKey, TValue> where TValue : class
    {
        public delegate IMultiKeyDictionary<TKey, TValue> DefaultCtor();
        public delegate IMultiKeyDictionary<TKey, TValue> DictCtor(IDictionary<TKey, TValue> d);
        public delegate IMultiKeyDictionary<TKey, TValue> CapacityCtor(int capacity);
        public delegate IMultiKeyDictionary<TKey, TValue> EnumCtor(IEnumerable<KeyValuePair<TKey, TValue>> e);

        public static CtorTuple<TKey, TValue> Create(DefaultCtor defaultConstructor, DictCtor dictConstructor,
            CapacityCtor capacityConstructor, EnumCtor enumConstructor)
        {
            return new CtorTuple<TKey, TValue>(defaultConstructor, dictConstructor, capacityConstructor,
                enumConstructor);
        }

        private CtorTuple(DefaultCtor defaultConstructor, DictCtor dictConstructor, CapacityCtor capacityConstructor, EnumCtor enumConstructor)
        {
            DefaultConstructor = defaultConstructor;
            DictConstructor = dictConstructor;
            CapacityConstructor = capacityConstructor;
            EnumConstructor = enumConstructor;
        }
        
        public DefaultCtor DefaultConstructor { get; }
        public DictCtor DictConstructor { get; }
        public CapacityCtor CapacityConstructor { get; }
        public EnumCtor EnumConstructor { get; }

        public void Deconstruct(out DefaultCtor defCtor, out DictCtor dictCtor, out CapacityCtor capacityCtor, out EnumCtor enumCtor)
        {
            defCtor = DefaultConstructor;
            dictCtor = DictConstructor;
            capacityCtor = CapacityConstructor;
            enumCtor = EnumConstructor;
        }
    }
}