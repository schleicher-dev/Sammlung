using System;
using System.Diagnostics.CodeAnalysis;
using Sammlung;
using Sammlung.Queues;

namespace _Fixtures.Sammlung.Extras
{
    public delegate IDeque<T> BufferWithCapacityCtor<T>(int capacity);
    
    [ExcludeFromCodeCoverage]
    public class DequeConstructors<T> : Tuple<BufferWithCapacityCtor<T>>

    {
        public DequeConstructors(BufferWithCapacityCtor<T> item1) : base(item1) { }
    }
}