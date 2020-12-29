using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sammlung.Utilities;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Queues.Concurrent
{
    public class BlockingArrayDeque
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
        public IEnumerator<T> GetEnumerator()
        {
            using var _ = _rwLock.UseUpgradableReadLock();
            var size = _inner.Count;
            var array = new T[size];
            _inner.CopyTo(array, 0);
            return array.Select(t => t).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(T item) => throw ExceptionsHelper.NewCallToMethodNotSupportedException();

        /// <inheritdoc />
        public void Clear()
        {
            using var _ = _rwLock.UseWriteLock();
            _inner.Clear();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            using var _ = _rwLock.UseReadLock();
            return _inner.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            using var _ = _rwLock.UseReadLock();
            _inner.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(T item) => 
            throw ExceptionsHelper.NewCallToMethodNotSupportedException();

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
        public bool IsReadOnly
        {
            get
            {
                using var _ = _rwLock.UseReadLock();
                return _inner.IsReadOnly;
            }
        }

        /// <inheritdoc />
        public void Enqueue(T element)
        {
            using var _ = _rwLock.UseWriteLock();
            _inner.Enqueue(element);
        }

        /// <inheritdoc />
        public T Dequeue()
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.Dequeue();
        }

        /// <inheritdoc />
        public bool TryDequeue(out T element)
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.TryDequeue(out element);
        }

        /// <inheritdoc />
        public void InverseEnqueue(T element)
        {
            using var _ = _rwLock.UseWriteLock();
            _inner.InverseEnqueue(element);
        }

        /// <inheritdoc />
        public T InverseDequeue()
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.InverseDequeue();
        }

        /// <inheritdoc />
        public bool TryInverseDequeue(out T element)
        {
            using var _ = _rwLock.UseWriteLock();
            return _inner.TryInverseDequeue(out element);
        }
    }
}