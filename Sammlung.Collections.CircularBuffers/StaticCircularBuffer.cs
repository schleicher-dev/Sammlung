using System;
using Sammlung.Werkzeug;

namespace Sammlung.Collections.CircularBuffers
{
    /// <summary>
    /// The <see cref="StaticCircularBuffer{T}"/> type is a circular buffer which has a fixed size.
    /// </summary>
    /// <typeparam name="T">the buffered type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class StaticCircularBuffer<T> : CircularBufferBase<T>
    {
        /// <inheritdoc />
        public override int Capacity => Storage.Length;

        /// <summary>
        /// Creates a new <see cref="StaticCircularBuffer{T}"/> using the initial capacity.
        /// </summary>
        /// <param name="capacity">the initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException">when the capacity is not strictly positive</exception>
        public StaticCircularBuffer(int capacity)
        {
            capacity = capacity.RequireGreater(0, nameof(capacity));
            Storage = new T[capacity];
        }

        /// <inheritdoc />
        public override bool TryPut(params T[] putItems)
        {
            var numItems = putItems.Length;
            if (Capacity < Count + numItems)
                return false;

            InternalPut(Storage, putItems);
            return true;
        }
    }
}