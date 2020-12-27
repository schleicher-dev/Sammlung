using System.Collections.Generic;
using System.Threading;
using Sammlung.Bases;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Concurrent
{
    public class ConcurrentStaticCircularBuffer<T> : CircularBufferBase<T>
    {
        private readonly EnhancedReaderWriterLock _rwLock;
        private readonly StaticCircularBuffer<T> _buffer;

        public ConcurrentStaticCircularBuffer(int capacity)
        {
            _rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
            _buffer = new StaticCircularBuffer<T>(capacity);
        }

        /// <inheritdoc />
        public override int Capacity
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _buffer.Capacity;
            }
        }

        /// <inheritdoc />
        public override int Count
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _buffer.Count;
            }
        }

        /// <inheritdoc />
        public override void PushFront(T element)
        {
            using var _ = _rwLock.UseWriteLock();
            _buffer.PushFront(element);
        }

        /// <inheritdoc />
        public override bool TryPopFront(out T element)
        {
            using var _ = _rwLock.UseWriteLock();
            return _buffer.TryPopFront(out element);
        }

        /// <inheritdoc />
        public override void PushBack(T element)
        {
            using var _ = _rwLock.UseWriteLock();
            _buffer.PushBack(element);
        }

        /// <inheritdoc />
        public override bool TryPopBack(out T element)
        {
            using var _ = _rwLock.UseWriteLock();
            return _buffer.TryPopBack(out element);
        }

        /// <inheritdoc />
        public override IEnumerator<T> GetEnumerator()
        {
            using var _ = _rwLock.UseReadLock();
            return _buffer.GetEnumerator();
        }
    }
}