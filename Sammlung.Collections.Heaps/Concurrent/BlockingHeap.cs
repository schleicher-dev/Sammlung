using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sammlung.Werkzeug;
using Sammlung.Werkzeug.Concurrent;

namespace Sammlung.Collections.Heaps.Concurrent
{
    /// <summary>
    /// The <see cref="BlockingHeap{T,TPriority}"/> class is a decorator around each <see cref="IHeap{T,TPriority}"/>
    /// which blocks on the particular action.
    /// </summary>
    /// <typeparam name="T">the type</typeparam>
    /// <typeparam name="TPriority">the priority type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class BlockingHeap<T, TPriority> : IHeap<T, TPriority> where TPriority : IComparable<TPriority>
    {
        private readonly EnhancedReaderWriterLock _rwLock = new(LockRecursionPolicy.NoRecursion);
        private readonly IHeap<T, TPriority> _decorated;

        /// <summary>
        /// Creates a new <see cref="BlockingHeap{T,TPriority}"/>.
        /// </summary>
        /// <param name="decorated">the heap to be decorated</param>
        internal BlockingHeap(IHeap<T, TPriority> decorated)
        {
            _decorated = decorated.RequireNotNull(nameof(decorated));
        }
        
        /// <inheritdoc />
        public int Count
        {
            get
            {
                using (_rwLock.UseReadLock())
                {
                    return _decorated.Count;
                }
            }
        }

        /// <inheritdoc />
        public bool IsEmpty
        {
            get
            {
                using (_rwLock.UseReadLock())
                {
                    return _decorated.IsEmpty;
                }
            }
        }

        /// <inheritdoc />
        public bool TryPeek(out HeapPair<T, TPriority> value)
        {
            using (_rwLock.UseReadLock())
            {
                return _decorated.TryPeek(out value);
            }
        }

        /// <inheritdoc />
        public bool TryPop(out HeapPair<T, TPriority> value)
        {
            using (_rwLock.UseWriteLock())
            {
                return _decorated.TryPop(out value);
            }
        }

        /// <inheritdoc />
        public void Push(T value, TPriority priority)
        {
            using (_rwLock.UseWriteLock())
            {
                _decorated.Push(value, priority);
            }
        }

        /// <inheritdoc />
        public bool TryReplace(T newValue, TPriority priority, out HeapPair<T, TPriority> oldValue)
        {
            using (_rwLock.UseWriteLock())
            {
                return _decorated.TryReplace(newValue, priority, out oldValue);
            }
        }

        /// <inheritdoc />
        public bool TryUpdate(HeapPair<T, TPriority> oldValue, TPriority priority)
        {
            using (_rwLock.UseWriteLock())
            {
                return _decorated.TryUpdate(oldValue, priority);
            }
        }

        /// <inheritdoc />
        IEnumerator<HeapPair<T, TPriority>> IEnumerable<HeapPair<T, TPriority>>.GetEnumerator()
        {
            using (_rwLock.UseReadLock())
            {
                var snapshot = _decorated.ToList();
                return snapshot.GetEnumerator();
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            using (_rwLock.UseReadLock())
            {
                var snapshot = _decorated.ToList();
                return ((IEnumerable)snapshot).GetEnumerator();
            }
        }
    }
}