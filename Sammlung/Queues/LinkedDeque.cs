using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Utilities;

namespace Sammlung.Queues
{
    public class LinkedDeque<T> : IDeque<T>
    {
        private readonly LinkedList<T> _internal;
        
        public LinkedDeque()
        {
            _internal = new LinkedList<T>();
        }
        
        /// <inheritdoc />
        public int Count => _internal.Count;

        /// <inheritdoc />
        public void PushLeft(T element) => _internal.AddFirst(element);

        /// <inheritdoc />
        public T PopRight() =>
            TryPopRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryPopRight(out T element)
        {
            if (!TryPeekRight(out element))
                return false;
            
            _internal.RemoveLast();
            return true;
        }

        /// <inheritdoc />
        public T PeekRight() =>
            TryPeekRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

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
        public T PopLeft() =>
            TryPopLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryPopLeft(out T element)
        {
            if (!TryPeekLeft(out element))
                return false;
            _internal.RemoveFirst();
            return true;
        }

        /// <inheritdoc />
        public T PeekLeft() =>
            TryPeekLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();
        
        /// <inheritdoc />
        public bool TryPeekLeft(out T element)
        {
            element = default;
            if (_internal.First == null) return false;
            
            element = _internal.First.Value;
            return true;
        }
    }
}