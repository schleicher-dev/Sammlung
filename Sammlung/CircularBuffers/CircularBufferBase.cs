using System;

namespace Sammlung.CircularBuffers
{
    /// <summary>
    /// The <see cref="CircularBufferBase{T}"/> implements the <see cref="ICircularBuffer{T}"/> interface and
    /// does encapsulate the most common operations.
    /// </summary>
    /// <typeparam name="T">the buffered type</typeparam>
    public abstract class CircularBufferBase<T> : ICircularBuffer<T>
    {
        protected int Head;
        protected int Tail;

        protected CircularBufferBase()
        {
            Head = Tail = 0;
            Count = 0;
        }

        /// <inheritdoc />
        public abstract int Capacity { get; }
        
        /// <inheritdoc />
        public int Count { get; private set; }
        private int Increment(int ptr, int inc) => (ptr + inc) % Capacity;

        protected void InternalPut(T[] storage, T[] putItems)
        {
            var numItems = putItems.Length;
            var position = 0;
            var numTailItems = Math.Max(0, Math.Min(numItems, Capacity - Tail));
            Array.Copy(putItems, position, storage, Tail, numTailItems);
            Tail = Increment(Tail, numTailItems);
            Count += numTailItems;
            numItems -= numTailItems;
            position += numTailItems;

            var numHeadItems = Math.Max(0, Math.Min(numItems, Head - Tail));
            Array.Copy(putItems, position, storage, Tail, numHeadItems);
            Tail = Increment(Tail, numHeadItems);
            Count += numHeadItems;
        }

        protected void InternalTake(T[] storage, T[] takeItems, int offset, int length)
        {
            var position = offset;
            var numTailItems = Math.Max(0, Math.Min(length, Capacity - Head));
            Array.Copy(storage, Head, takeItems, position, numTailItems);
            Head = Increment(Head, numTailItems);
            Count -= numTailItems;
            length -= numTailItems;
            position += numTailItems;

            var numHeadItems = Math.Max(0, Math.Min(length, Tail - Head));
            Array.Copy(storage, Head, takeItems, position, numHeadItems);
            Head = Increment(Head, numHeadItems);
            Count -= numHeadItems;
        }

        /// <inheritdoc />
        public abstract bool TryPut(params T[] putItems);

        /// <inheritdoc />
        public virtual bool TryTake(T[] takeItems, int offset, int length) =>
            length <= Count && length <= takeItems.Length - offset;
    }
}