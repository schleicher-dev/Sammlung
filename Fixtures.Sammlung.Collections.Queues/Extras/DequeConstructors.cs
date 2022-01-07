using System;
using System.Diagnostics.CodeAnalysis;
using Sammlung.Collections.Queues;

namespace Fixtures.Sammlung.Collections.Queues.Extras
{
    public delegate IDeque<T> BufferWithCapacityCtor<T>(int capacity);
    
    [ExcludeFromCodeCoverage]
    public class DequeConstructors<T> : Tuple<BufferWithCapacityCtor<T>>

    {
        private readonly string _name;

        public DequeConstructors(string name, BufferWithCapacityCtor<T> item1) : base(item1)
        {
            _name = name ?? "Unnamed";
        }


        /// <inheritdoc />
        public override string ToString() => _name;
    }
}