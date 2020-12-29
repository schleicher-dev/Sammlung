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
        private int _frontPointer;
        private int _backPointer;

        public ArrayDeque(int capacity)
        {
            _array = new T[capacity];
            _frontPointer = 0;
            _backPointer = 0;
            Count = 0;
        }
        
        private void Grow()
        {
            var oldSize = Capacity;
            var newSize = Math.Max(1, Capacity * 2);
            var newArray = new T[newSize];
            
            var frontLen = oldSize - _frontPointer;
            Array.Copy(_array, _frontPointer, newArray, 0, frontLen);
            Array.Copy(_array, 0, newArray, frontLen, _backPointer);
            _array = newArray;
            _frontPointer = 0;
            _backPointer = oldSize;
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
        public void Enqueue(T element)
        {
            GrowIfNeeded();
            
            _array[_backPointer] = element;
            _backPointer = IncrementPointer(_backPointer);
            Count += 1;
        }

        /// <inheritdoc />
        public T Dequeue() =>
            TryDequeue(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryDequeue(out T element)
        {
            element = default;
            if (Count <= 0)
                return false;

            _backPointer = DecrementPointer(_backPointer);
            element = _array[_backPointer];
            _array[_backPointer] = default;
            Count -= 1;
            
            return true;
        }

        /// <inheritdoc />
        public void InverseEnqueue(T element)
        {
            GrowIfNeeded();
            
            _frontPointer = DecrementPointer(_frontPointer);
            _array[_frontPointer] = element;
            Count += 1;
        }

        /// <inheritdoc />
        public T InverseDequeue() =>
            TryInverseDequeue(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryInverseDequeue(out T element)
        {
            element = default;
            if (Count <= 0)
                return false;

            element = _array[_frontPointer];
            _array[_frontPointer] = default;
            _frontPointer = IncrementPointer(_frontPointer);
            Count -= 1;
            
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
            _frontPointer = 0;
            _backPointer = 0;
            Count = 0;
        }
        
        private IEnumerable<int> GetIndices()
        {
            if (Count == 0) return Enumerable.Empty<int>();
            var ptrDiff = _backPointer - _frontPointer;
            if (0 < ptrDiff) return Enumerable.Range(_frontPointer, ptrDiff);
            return Enumerable.Range(_frontPointer, Capacity - _frontPointer)
                .Concat(Enumerable.Range(0, _backPointer));
        }

        private IEnumerable<T> GetValues() => GetIndices().Select(i => _array[i]);

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => GetValues().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}