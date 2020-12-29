using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sammlung.Utilities;

namespace Sammlung.Queues
{
    /// <summary>
    /// The <see cref="ArrayDeque{T}"/> type is an implementation of a double-ended queue (deque) which uses an array
    /// and implements the <seealso cref="IDeque{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">the contained type</typeparam>
    public class ArrayDeque<T> : IDeque<T>
    {
        private T[] _array;
        private int _leftPointer;
        private int _rightPointer;

        public ArrayDeque(int capacity)
        {
            _array = new T[capacity];
            _leftPointer = 0;
            _rightPointer = 0;
            Count = 0;
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

        #region IDeque<T>

        private int Capacity => _array.Length;

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        public void PushLeft(T element)
        {
            GrowIfNeeded();
            
            _leftPointer = DecrementPointer(_leftPointer);
            _array[_leftPointer] = element;
            Count += 1;
        }

        /// <inheritdoc />
        public T PopRight() =>
            TryPopRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryPopRight(out T element)
        {
            if (!TryPeekRight(out element))
                return false;

            _rightPointer = DecrementPointer(_rightPointer);
            _array[_rightPointer] = default;
            Count -= 1;
            
            return true;
        }

        /// <inheritdoc />
        public T PeekRight()=>
            TryPeekRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

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
            Count += 1;
        }

        /// <inheritdoc />
        public T PopLeft() =>
            TryPopLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryPopLeft(out T element)
        {
            if (!TryPeekLeft(out element))
                return false;
            
            _array[_leftPointer] = default;
            _leftPointer = IncrementPointer(_leftPointer);
            Count -= 1;
            
            return true;
        }

        /// <inheritdoc />
        public T PeekLeft() =>
            TryPeekLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryPeekLeft(out T element)
        {
            element = default;
            if (Count == 0) return false;
            
            element = _array[_leftPointer];
            return true;
        }

        #endregion

        #region ICollection<T>

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public bool Contains(T item) => GetValues().Contains(item);

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array.Length < arrayIndex + Count)
                throw ExceptionsHelper.ValuesNotFittingIntoArray(nameof(array));
            using var values = GetEnumerator();
            for (var i = arrayIndex; values.MoveNext(); ++i)
                array[i] = values.Current;
        }

        /// <inheritdoc />
        public void Add(T item) => throw ExceptionsHelper.NewCallToMethodNotSupportedException();
        
        /// <inheritdoc />
        public bool Remove(T item) => throw ExceptionsHelper.NewCallToMethodNotSupportedException();

        /// <inheritdoc />
        public void Clear()
        {
            _leftPointer = 0;
            _rightPointer = 0;
            Count = 0;
        }
        
        private IEnumerable<int> GetIndices()
        {
            if (Count == 0) return Enumerable.Empty<int>();
            var ptrDiff = _rightPointer - _leftPointer;
            if (0 < ptrDiff) return Enumerable.Range(_leftPointer, ptrDiff);
            return Enumerable.Range(_leftPointer, Capacity - _leftPointer)
                .Concat(Enumerable.Range(0, _rightPointer));
        }

        private IEnumerable<T> GetValues() => GetIndices().Select(i => _array[i]);

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => GetValues().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}