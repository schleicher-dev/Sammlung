using System;
using System.Collections.Generic;
using System.Linq;

namespace Sammlungen.Collections
{
    public sealed class BinaryHeap<TKey, TValue> : IHeap<TKey, TValue>
    {
        private readonly IComparer<TKey> _keyComparer;
        private readonly IEqualityComparer<TValue> _valueComparer;
        private readonly List<KeyValuePair<TKey, TValue>> _binaryHeap;
        
        public BinaryHeap() : this(0, Comparer<TKey>.Default, EqualityComparer<TValue>.Default) { }

        public BinaryHeap(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) : 
            this(0, keyComparer, valueComparer) { }
        
        public BinaryHeap(IEnumerable<KeyValuePair<TKey, TValue>> containers) : 
            this(containers, Comparer<TKey>.Default, EqualityComparer<TValue>.Default) { }

        public BinaryHeap(IEnumerable<KeyValuePair<TKey, TValue>> containers, IComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueComparer)
        {
            _keyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
            _valueComparer = valueComparer ?? throw new ArgumentNullException(nameof(valueComparer));
            _binaryHeap = containers.ToList();
            
            // Sort by key. This creates heap order implicitly.
            _binaryHeap.Sort((kv1, kv2) => _keyComparer.Compare(kv1.Key, kv2.Key));
        }

        public BinaryHeap(int capacity, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            _keyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
            _valueComparer = valueComparer ?? throw new ArgumentNullException(nameof(valueComparer));
            _binaryHeap = new List<KeyValuePair<TKey, TValue>>(capacity);
        }

        public int Count => _binaryHeap.Count;
        
        public bool TryPeek(out TValue value)
        {
            value = default;
            if (!_binaryHeap.Any()) return false;

            value = _binaryHeap[0].Value;
            return true;
        }

        public bool TryPop(out TValue value)
        {
            value = default;
            if (Count == 0) return false;
            
            // Swap then remove.
            var root = _binaryHeap[0];
            Swap(0, Count - 1);
            _binaryHeap.RemoveAt(Count - 1);

            // Assign item and remove it from mapping.
            value = root.Value;
            
            // If there aren't any elements left, no sifting is needed.
            if (1 < Count) SiftDown(0);
            
            return true;
        }

        public void Push(TKey key, TValue value)
        {
            var container = KeyValuePair.Create(key, value); 
            _binaryHeap.Add(container);
            
            SiftUp(_binaryHeap.Count - 1);
        }

        public bool TryReplace(TKey key, TValue value, out TValue oldValue)
        {
            oldValue = default;
            if (Count == 0) return false;
            
            // Get the container and the old value.
            var container = _binaryHeap[0];
            oldValue = container.Value;

            // Set the new value.
            _binaryHeap[0] = KeyValuePair.Create(key, value);
            SiftDown(0);
            
            return true;
        }

        public bool TryUpdate(TValue value, TKey key)
        {
            if (!TryFindContainer(value, out var index, out var container))
                return false;

            _binaryHeap[index] = KeyValuePair.Create(key, container.Value);
            index = SiftUp(index);
            SiftDown(index);
            return true;
        }

        private bool TryFindContainer(TValue value, out int index, out KeyValuePair<TKey, TValue> container)
        {
            for (var i = 0; i < _binaryHeap.Count; ++i)
            {
                var candidate = _binaryHeap[i];
                if (!_valueComparer.Equals(candidate.Value, value)) continue;

                index = i;
                container = candidate;
                return true;
            }

            index = -1;
            container = default;
            return false;
        }

        private void Swap(int fstIndex, int sndIndex)
        {
            var swap = _binaryHeap[fstIndex];
            _binaryHeap[fstIndex] = _binaryHeap[sndIndex];
            _binaryHeap[sndIndex] = swap;
        }

        #region SiftUp

        private int SiftUp(int nodeIndex)
        {
            var node = _binaryHeap[nodeIndex];
            while (nodeIndex != 0)
            {
                var parentIndex = (nodeIndex - 1) / 2;
                var parent = _binaryHeap[parentIndex];

                if (_keyComparer.Compare(parent.Key, node.Key) <= 0)
                    return nodeIndex;
                Swap(nodeIndex, parentIndex);
                nodeIndex = parentIndex;
            }

            return nodeIndex;
        }

        #endregion

        #region SiftDown

        private int SiftDown(int nodeIndex)
        {
            var (key, _) = _binaryHeap[nodeIndex];
            while (true)
            {
                // To keep the heap invariant we have to compare the keys of both child nodes of this node.
                var leftIndex = nodeIndex * 2 + 1;
                var rightIndex = nodeIndex * 2 + 2;

                if (Count <= leftIndex)
                    return nodeIndex;

                // There is only a left node, decide which node is the smaller one.
                if (Count <= rightIndex)
                {
                    var leftNode = _binaryHeap[leftIndex];
                    if (_keyComparer.Compare(key, leftNode.Key) < 0)
                        return nodeIndex;
                    Swap(nodeIndex, leftIndex);
                    nodeIndex = leftIndex;
                    continue;
                }
                
                // Compare both nodes.
                var (leftKey, _) = _binaryHeap[leftIndex];
                var (rightKey, _) = _binaryHeap[rightIndex];
                if (_keyComparer.Compare(key, leftKey) < 0 && _keyComparer.Compare(key, rightKey) < 0)
                    return nodeIndex;
                var swapIndex = _keyComparer.Compare(leftKey, rightKey) < 0 ? leftIndex : rightIndex;
                Swap(nodeIndex, swapIndex);
                nodeIndex = swapIndex;
            }
        }

        #endregion

    }
}