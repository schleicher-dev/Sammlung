using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sammlung.Utilities;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Queues.Concurrent
{
    public static class BlockingArrayDeque
    {
        public static IDeque<T> Wrap<T>(IDeque<T> inner) => new BlockingArrayDeque<T>(inner);
    }
    
    internal class BlockingArrayDeque<T> : IDeque<T>
    {
        private readonly IDeque<T> _inner;
        private readonly EnhancedReaderWriterLock _rwLock;

        public BlockingArrayDeque(IDeque<T> inner)
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
        public T PopRight()
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.PopRight();
        }

        /// <inheritdoc />
        public bool TryPopRight(out T element)
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.TryPopRight(out element);
        }

        /// <inheritdoc />
        public T PeekRight() => 
            TryPeekRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

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
        public T PopLeft()
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.PopLeft();
        }

        /// <inheritdoc />
        public bool TryPopLeft(out T element)
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.TryPopLeft(out element);
        }

        /// <inheritdoc />
        public T PeekLeft() => 
            TryPeekLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryPeekLeft(out T element)
        {
            using var _ = _rwLock.UseReadLock();
            return _inner.TryPeekLeft(out element);
        }
    }
}