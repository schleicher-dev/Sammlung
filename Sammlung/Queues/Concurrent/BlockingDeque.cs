using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Queues.Concurrent
{
    /// <summary>
    /// The <see cref="BlockingDeque"/> is a static class which can decorate a <see cref="IDeque{T}"/>.
    /// </summary>
    [PublicAPI]
    public static class BlockingDeque
    {
        public static IDeque<T> Wrap<T>(IDeque<T> inner) => new BlockingDeque<T>(inner);
    }
    
    /// <summary>
    /// The <see cref="BlockingDeque{T}"/> is a decorator for any <see cref="IDeque{T}"/> type which is thread-safe.
    /// </summary>
    /// <typeparam name="T">the type</typeparam>
    [PublicAPI]
    public class BlockingDeque<T> : IDeque<T>
    {
        private readonly IDeque<T> _inner;
        private readonly EnhancedReaderWriterLock _rwLock;

        /// <summary>
        /// Creates a new <see cref="BlockingDeque{T}"/> using an inner <seealso cref="IDeque{T}"/>.
        /// </summary>
        /// <param name="inner">the inner deque</param>
        public BlockingDeque([NotNull] IDeque<T> inner)
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