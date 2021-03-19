using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Sammlung.Queues
{
    /// <summary>
    /// The <see cref="ArrayDeque{T}"/> type is an implementation of a double-ended queue (deque) which uses an array
    /// and implements the <seealso cref="IDeque{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">the contained type</typeparam>
    public sealed class ArrayDeque<T> : IDeque<T>
    {
        private T[] _array;
        private int _leftPointer;
        private int _rightPointer;
        private int _count;

        public ArrayDeque(int capacity)
        {
            _array = new T[capacity];
            _leftPointer = 0;
            _rightPointer = 0;
            _count = 0;
        }
        
        private void Grow()
        {
            var oldSize = Capacity;
            var newSize = Math.Max(1, Capacity * 2);
            var newArray = new T[newSize];
            
            var frontLen = oldSize - _leftPointer;
            Array.Copy(_array, _leftPointer, newArray, 0, frontLen);
            Array.Copy(_array, 0, newArray, frontLen, _rightPointer);
            _array = newArray;
            _leftPointer = 0;
            _rightPointer = oldSize;
        }

        private void GrowIfNeeded()
        {
            while (Capacity <= Count) Grow();
        }

        private int IncrementPointer(int pointer) => pointer < Capacity - 1 ? pointer + 1 : 0;
        private int DecrementPointer(int pointer) => 0 < pointer ? pointer - 1 : Capacity - 1;

        private int Capacity => _array.Length;

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ConvertToAutoPropertyWhenPossible", 
            Justification = "Cannot make this a auto-property. This is essentially a code analysis bug.")]
        public int Count => _count;

        /// <inheritdoc />
        public void PushLeft(T element)
        {
            GrowIfNeeded();
            
            _leftPointer = DecrementPointer(_leftPointer);
            _array[_leftPointer] = element;
            _count += 1;
        }
        
        /// <inheritdoc />
        public bool TryPopRight(out T element)
        {
            if (!TryPeekRight(out element))
                return false;

            _rightPointer = DecrementPointer(_rightPointer);
            _array[_rightPointer] = default;
            _count -= 1;
            
            return true;
        }

        /// <inheritdoc />
        public bool TryPeekRight(out T element)
        {
            element = default;
            if (Count == 0) return false;
            
            element = _array[DecrementPointer(_rightPointer)];
            return true;
        }

        /// <inheritdoc />
        public void PushRight(T element)
        {
            GrowIfNeeded();
            
            _array[_rightPointer] = element;
            _rightPointer = IncrementPointer(_rightPointer);
            _count += 1;
        }
        
        /// <inheritdoc />
        public bool TryPopLeft(out T element)
        {
            if (!TryPeekLeft(out element))
                return false;
            
            _array[_leftPointer] = default;
            _leftPointer = IncrementPointer(_leftPointer);
            _count -= 1;
            
            return true;
        }

        /// <inheritdoc />
        public bool TryPeekLeft(out T element)
        {
            element = default;
            if (Count == 0) return false;
            
            element = _array[_leftPointer];
            return true;
        }

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _array.Cast<T>().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>) this).GetEnumerator();
    }
}