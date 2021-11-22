using System.Threading;
using Sammlung.Utilities;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.CircularBuffers.Concurrent
{
    /// <summary>
    /// The <see cref="BlockingCircularBuffer{T}"/> type is a decorator for each <see cref="ICircularBuffer{T}"/>
    /// which ensures an ordered put into and take out off the buffer.
    /// </summary>
    /// <typeparam name="T">the inner type of the buffer</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class BlockingCircularBuffer<T> : ICircularBuffer<T>
    {
        private readonly EnhancedReaderWriterLock _rwLock =
            new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
        private readonly ICircularBuffer<T> _decorated;

        /// <summary>
        /// Creates a new <see cref="BlockingCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="decorated">the circular buffer to decorate</param>
        internal BlockingCircularBuffer(ICircularBuffer<T> decorated)
        {
            _decorated = decorated.RequireNotNull(nameof(decorated));
        }

        /// <inheritdoc />
        public int Capacity
        {
            get
            {
                using (_rwLock.UseReadLock())
                {
                    return _decorated.Capacity;
                }
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get {
                using (_rwLock.UseReadLock())
                {
                    return _decorated.Count;
                }
            }
        }

        /// <inheritdoc />
        public bool TryPut(params T[] putItems)
        {
            using (_rwLock.UseWriteLock())
            {
                return _decorated.TryPut(putItems);
            }
        }

        /// <inheritdoc />
        public bool TryTake(T[] takeItems, int offset, int length)
        {
            using (_rwLock.UseWriteLock())
            {
                return _decorated.TryTake(takeItems, offset, length);
            }
        }

        /// <inheritdoc />
        public bool TryPeek(T[] peekItems, int offset, int length)
        {
            using (_rwLock.UseReadLock())
            {
                return _decorated.TryPeek(peekItems, offset, length);
            }
        }
    }
}