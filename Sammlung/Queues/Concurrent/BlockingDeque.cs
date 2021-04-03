using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Queues.Concurrent
{
    public static class BlockingDeque
    {
        public static IDeque<T> Wrap<T>(IDeque<T> inner) => new BlockingDeque<T>(inner);
    }
    
    internal class BlockingDeque<T> : IDeque<T>
    {
        private readonly IDeque<T> _inner;
        private readonly EnhancedReaderWriterLock _rwLock;

        public BlockingDeque(IDeque<T> inner)
        {
            _rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.SupportsRecursion);
            _inner = inner;
        }
        
        /// <inheritdoc />
        public int Count
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _inner.Count;
            }
        }

        /// <inheritdoc />
        public void PushLeft(T element)
        {
            using var _ = _rwLock.UseWriteLock();
            _inner.PushLeft(element);
        }

        /// <inheritdoc />
        public bool TryPopRight(out T element)
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.TryPopRight(out element);
        }

        /// <inheritdoc />
        public bool TryPeekRight(out T element)
        {
            using var _ = _rwLock.UseReadLock();
            return _inner.TryPeekRight(out element);
        }

        /// <inheritdoc />
        public void PushRight(T element)
        {
            using var _ = _rwLock.UseWriteLock();
            _inner.PushRight(element);
        }

        /// <inheritdoc />
        public bool TryPopLeft(out T element)
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.TryPopLeft(out element);
        }

        /// <inheritdoc />
        public bool TryPeekLeft(out T element)
        {
            using var _ = _rwLock.UseReadLock();
            return _inner.TryPeekLeft(out element);
        }

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _inner.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
    }
}