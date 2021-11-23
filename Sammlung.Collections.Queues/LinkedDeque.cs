using System.Collections;
using System.Collections.Generic;

namespace Sammlung.Collections.Queues
{
    /// <summary>
    /// The <see cref="LinkedDeque{T}"/> type is an implementation of a double-ended queue.
    /// </summary>
    /// <typeparam name="T">the type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public sealed class LinkedDeque<T> : IDeque<T>
    {
        private readonly LinkedList<T> _internal;

        /// <summary>
        /// Creates a new instance of the <see cref="LinkedDeque{T}"/>.
        /// </summary>
        public LinkedDeque()
        {
            _internal = new LinkedList<T>();
        }

        /// <inheritdoc />
        public int Count => _internal.Count;

        /// <inheritdoc />
        public void PushLeft(T element) => _internal.AddFirst(element);

        /// <inheritdoc />
        public bool TryPopRight(out T element)
        {
            if (!TryPeekRight(out element))
                return false;

            _internal.RemoveLast();
            return true;
        }

        /// <inheritdoc />
        public bool TryPeekRight(out T element)
        {
            element = default;
            if (_internal.Last == null) return false;

            element = _internal.Last.Value;
            return true;
        }

        /// <inheritdoc />
        public void PushRight(T element) => _internal.AddLast(element);

        /// <inheritdoc />
        public bool TryPopLeft(out T element)
        {
            if (!TryPeekLeft(out element))
                return false;
            _internal.RemoveFirst();
            return true;
        }

        /// <inheritdoc />
        public bool TryPeekLeft(out T element)
        {
            element = default;
            if (_internal.First == null) return false;

            element = _internal.First.Value;
            return true;
        }

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _internal.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>) this).GetEnumerator();
    }
}