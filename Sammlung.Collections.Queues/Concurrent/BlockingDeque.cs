using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sammlung.Utilities;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Collections.Queues.Concurrent
{
    /// <summary>
    /// The <see cref="BlockingDeque{T}"/> is a decorator for any <see cref="IDeque{T}"/> type which is thread-safe.
    /// </summary>
    /// <typeparam name="T">the type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class BlockingDeque<T> : IDeque<T>
    {
        private readonly IDeque<T> _inner;
        private readonly EnhancedReaderWriterLock _rwLock;

        /// <summary>
        /// Creates a new <see cref="BlockingDeque{T}"/> using an inner <seealso cref="IDeque{T}"/>.
        /// </summary>
        /// <param name="inner">the inner deque</param>
        public BlockingDeque(IDeque<T> inner)
        {
            _rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.SupportsRecursion);
            _inner = inner.RequireNotNull(nameof(inner));
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
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            using (_rwLock.UseReadLock())
            {
                var snapshot = _inner.ToList();
                return snapshot.GetEnumerator();
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            using (_rwLock.UseReadLock())
            {
                var snapshot = _inner.ToList();
                return ((IEnumerable) snapshot).GetEnumerator();
            }
        }
    }
}