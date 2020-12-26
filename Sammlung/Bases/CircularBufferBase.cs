using System;
using System.Collections;
using System.Collections.Generic;
using Sammlung.Interfaces;

namespace Sammlung.Bases
{
    public abstract class CircularBufferBase<T> : ICircularBuffer<T>
    {
        /// <inheritdoc />
        public abstract int Capacity { get; }
        
        /// <inheritdoc />
        public abstract int Count { get; }
        
        /// <inheritdoc />
        public abstract void PushFront(T element);
        
        /// <inheritdoc />
        public T PopFront() => 
            TryPopFront(out var element)
                ? element
                : throw new InvalidOperationException("Cannot pop from empty collection.");
        
        /// <inheritdoc />
        public abstract bool TryPopFront(out T element);
        
        /// <inheritdoc />
        public abstract void PushBack(T element);
        
        /// <inheritdoc />
        public T PopBack() =>
            TryPopBack(out var element)
                ? element
                : throw new InvalidOperationException("Cannot pop from empty collection.");

        /// <inheritdoc />
        public abstract bool TryPopBack(out T element);

        /// <inheritdoc />
        public abstract IEnumerator<T> GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}