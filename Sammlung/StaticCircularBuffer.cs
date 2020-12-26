using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Bases;

namespace Sammlung
{
    /// <summary>
    /// The <see cref="StaticCircularBuffer{T}"/> type saves a fixed number of items in the buffer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StaticCircularBuffer<T> : CircularBufferBase<T>
    {
        private readonly T[] _internalArray;
        private int _count;
        private int _frontPointer;
        private int _backPointer;

        /// <inheritdoc />
        public override int Capacity => _internalArray.Length;

        /// <inheritdoc />
        public override int Count => _count;

        public StaticCircularBuffer(int capacity)
        {
            _internalArray = new T[capacity];
            _count = 0;
            _frontPointer = 0;
            _backPointer = 0;
        }

        private bool IsFull => Count == Capacity;
        private int IncrementPointer(int pointer) => pointer < Capacity - 1 ? pointer + 1 : 0;
        private int DecrementPointer(int pointer) => 0 < pointer ? pointer - 1 : Capacity - 1;

        /// <inheritdoc />
        public override void PushFront(T element)
        {
            _frontPointer = DecrementPointer(_frontPointer);
            if (IsFull) _backPointer = DecrementPointer(_backPointer);
            _internalArray[_frontPointer] = element;
            _count = Math.Min(Capacity, _count + 1);
        }
        
        /// <inheritdoc />
        public override bool TryPopFront(out T element)
        {
            element = default;
            if (Count <= 0)
                return false;

            element = _internalArray[_frontPointer];
            _internalArray[_frontPointer] = default;
            _frontPointer = IncrementPointer(_frontPointer);
            _count -= 1;
            
            return true;
        }

        /// <inheritdoc />
        public override void PushBack(T element)
        {
            _internalArray[_backPointer] = element;
            if (IsFull) _frontPointer = IncrementPointer(_frontPointer);
            _backPointer = IncrementPointer(_backPointer);
            _count = Math.Min(Capacity, _count + 1);
        }

        /// <inheritdoc />
        public override bool TryPopBack(out T element)
        {
            element = default;
            if (Count <= 0)
                return false;

            _backPointer = DecrementPointer(_backPointer);
            element = _internalArray[_backPointer];
            _internalArray[_backPointer] = default;
            _count -= 1;
            
            return true;
        }

        /// <inheritdoc />
        public override IEnumerator<T> GetEnumerator()
        {
            if (_count == 0) return Enumerable.Empty<T>().GetEnumerator();
            
            var indices = _frontPointer < _backPointer
                ? Enumerable.Range(_frontPointer, Count)
                : Enumerable.Range(_frontPointer, Capacity - _frontPointer)
                    .Concat(Enumerable.Range(0, _backPointer));
            return new Enumerator(_internalArray, indices);
        }

        private class Enumerator : IEnumerator<T>
        {
            private readonly T[] _internalArray;
            private readonly IEnumerator<int> _indicesEnumerator;
            
            public T Current => _internalArray[_indicesEnumerator.Current];

            object? IEnumerator.Current => Current;

            public Enumerator(T[] internalArray, IEnumerable<int> indices)
            {
                _internalArray = internalArray;
                _indicesEnumerator = indices.GetEnumerator();
            }

            public bool MoveNext() => _indicesEnumerator.MoveNext();

            public void Reset() => _indicesEnumerator.Reset();

            public void Dispose() => _indicesEnumerator.Dispose();
        }
    }
}