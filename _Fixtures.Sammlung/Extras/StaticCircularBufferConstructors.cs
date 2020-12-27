using System;
using Sammlung;
using Sammlung.Interfaces;

namespace _Fixtures.Sammlung.Extras
{
    public delegate ICircularBuffer<T> ScbWithCapacityCtor<T>(int capacity);
    
    public class StaticCircularBufferConstructors<T> : Tuple<ScbWithCapacityCtor<T>>

    {
        public StaticCircularBufferConstructors(ScbWithCapacityCtor<T> item1) : base(item1) { }
    }
}