using System;
using Sammlung.Resources;

namespace Sammlung.CircularBuffers
{
    /// <summary>
    /// The <see cref="StaticCircularBuffer{T}"/> type is a circular buffer which has a fixed size.
    /// </summary>
    /// <typeparam name="T">the buffered type</typeparam>
    public class StaticCircularBuffer<T> : CircularBufferBase<T>
    {
        private readonly T[] _storage;
        
        /// <inheritdoc />
        public override int Capacity => _storage.Length;

        /// <summary>
        /// Creates a new <see cref="StaticCircularBuffer{T}"/> using the initial capacity.
        /// </summary>
        /// <param name="capacity">the initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException">when the capacity is not strictly positive</exception>
        public StaticCircularBuffer(int capacity)
        {
            capacity = 0 < capacity
                ? capacity
                : throw new ArgumentOutOfRangeException(nameof(capacity), ErrorMessages.ValueMustBeStrictlyPositive);
            _storage = new T[capacity];
        }

        /// <inheritdoc />
        public override bool TryPut(params T[] putItems)
        {
            var numItems = putItems.Length;
            if (Capacity < Count + numItems)
                return false;

            InternalPut(_storage, putItems);
            return true;
        }

        /// <inheritdoc />
        public override bool TryTake(T[] takeItems, int offset, int length)
        {
            if (!base.TryTake(takeItems, offset, length))
                return false;
            
            InternalTake(_storage, takeItems, offset, length);
            return true;
        }
    }
}