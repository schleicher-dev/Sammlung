using System;
using Sammlung.Interfaces;

namespace Sammlung.Bases
{
    public abstract class HeapBase<TKey, TValue> : IHeap<TKey, TValue>
    {
        /// <inheritdoc />
        public abstract int Count { get; }

        /// <inheritdoc />
        public bool IsEmpty() => Count == 0;

        /// <inheritdoc />
        public bool Any() => Count != 0;

        /// <inheritdoc />
        public TValue Peek()
        {
            return TryPeek(out var item)
                ? item
                : throw new InvalidOperationException("Cannot peek from heap. The heap may be empty.");
        }

        /// <inheritdoc />
        public abstract bool TryPeek(out TValue value);

        /// <inheritdoc />
        public TValue Pop()
        {
            return TryPop(out var item)
                ? item
                : throw new InvalidOperationException("Cannot peek from heap. The heap may be empty.");
        }

        /// <inheritdoc />
        public abstract bool TryPop(out TValue value);

        /// <inheritdoc />
        public abstract void Push(TKey key, TValue value);
        
        /// <inheritdoc />
        public TValue Replace(TKey key, TValue value)
        {
            return TryReplace(key, value, out var oldValue)
                ? oldValue :
                throw new InvalidOperationException("Cannot replace root in heap. The heap may be empty.");
        }

        /// <inheritdoc />
        public abstract bool TryReplace(TKey key, TValue value, out TValue oldValue);

        public void Update(TValue item, TKey key)
        {
            if (!TryUpdate(item, key))
                throw new InvalidOperationException("Cannot update item in heap. It may not exist.");
        }

        /// <inheritdoc />
        public abstract bool TryUpdate(TValue item, TKey key);
    }
}