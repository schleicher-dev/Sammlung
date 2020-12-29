using System;
using Sammlung.Utilities;

namespace Sammlung.Heaps
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
                : throw ExceptionsHelper.NewEmptyCollectionException();
        }

        /// <inheritdoc />
        public abstract bool TryPeek(out TValue value);

        /// <inheritdoc />
        public TValue Pop()
        {
            return TryPop(out var item)
                ? item
                : throw ExceptionsHelper.NewEmptyCollectionException();
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
                throw ExceptionsHelper.NewEmptyCollectionException();
        }

        /// <inheritdoc />
        public abstract bool TryReplace(TKey key, TValue value, out TValue oldValue);

        public void Update(TValue value, TKey key)
        {
            if (!TryUpdate(value, key))
                throw ExceptionsHelper.NewHeapUpdateFailedException();
        }

        /// <inheritdoc />
        public abstract bool TryUpdate(TValue value, TKey key);
    }
}