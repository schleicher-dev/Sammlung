using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Utilities;

namespace Sammlung.Queues
{
    public sealed class LinkedDeque<T> : DequeBase<T>
    {
        private readonly LinkedList<T> _internal;
        
        public LinkedDeque()
        {
            _internal = new LinkedList<T>();
        }

        /// <inheritdoc />
        public override int Count => _internal.Count;

        /// <inheritdoc />
        public override void PushLeft(T element) => _internal.AddFirst(element);

        /// <inheritdoc />
        public override bool TryPopRight(out T element)
        {
            if (!TryPeekRight(out element))
                return false;
            
            _internal.RemoveLast();
            return true;
        }
        
        /// <inheritdoc />
        public override bool TryPeekRight(out T element)
        {
            element = default;
            if (_internal.Last == null) return false;
            
            element = _internal.Last.Value;
            return true;
        }

        /// <inheritdoc />
        public override void PushRight(T element) => _internal.AddLast(element);
        
        /// <inheritdoc />
        public override bool TryPopLeft(out T element)
        {
            if (!TryPeekLeft(out element))
                return false;
            _internal.RemoveFirst();
            return true;
        }
        
        /// <inheritdoc />
        public override bool TryPeekLeft(out T element)
        {
            element = default;
            if (_internal.First == null) return false;
            
            element = _internal.First.Value;
            return true;
        }
    }
}