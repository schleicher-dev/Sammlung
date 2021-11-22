using System;
using Sammlung.Resources;

namespace Sammlung.CircularBuffers
{
    /// <summary>
    /// The <see cref="DynamicCircularBuffer{T}"/> type is a circular buffer which grows when needed.
    /// </summary>
    /// <typeparam name="T">the buffered type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class DynamicCircularBuffer<T> : CircularBufferBase<T>
    {
        private static int NextPowerOfTwo(int value)
        {
            var power = 1;
            while (power < value) power <<= 1;
            return power;
        }

        /// <inheritdoc />
        public override int Capacity => Storage.Length;

        /// <summary>
        /// Creates a new <see cref="DynamicCircularBuffer{T}"/> with the passed initial capacity.
        /// </summary>
        /// <param name="capacity">the initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException">when the capacity is not strictly positive</exception>
        public DynamicCircularBuffer(int capacity)
        {
            capacity = 0 < capacity
                ? capacity
                : throw new ArgumentOutOfRangeException(nameof(capacity), ErrorMessages.ValueMustBeStrictlyPositive);

            var nextCapacity = NextPowerOfTwo(capacity);
            Storage = new T[nextCapacity];
        }

        private void Grow(int extraSpace)
        {
            var requiredSpace = NextPowerOfTwo(Count + extraSpace);
            var newStorage = new T[requiredSpace];

            var position = 0;
            var count = Count;
            var copyItems = Math.Max(0, Math.Min(Count, Capacity - Head));
            Array.Copy(Storage, Head, newStorage, position, copyItems);
            position += copyItems;
            count -= copyItems;

            copyItems = Math.Min(count, Tail);
            Array.Copy(Storage, 0, newStorage, position, copyItems);

            Head = 0;
            Tail = Count;
            Storage = newStorage;
        }

        /// <inheritdoc />
        public override bool TryPut(params T[] putItems)
        {
            var numItems = putItems.Length;
            if (Capacity < Count + numItems) Grow(numItems);
            InternalPut(Storage, putItems);
            return true;
        }
    }
}