using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Sammlung.Utilities.Patterns;

namespace Sammlung.Heaps
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global",
        Justification = Justifications.PublicApiJustification)]
    public sealed class BinaryHeap<T> : IHeap<T> where T : class
    {
        private static readonly Lazy<HeapNodeObjectPool> HeapNodePoolLoader =
            new Lazy<HeapNodeObjectPool>(() => new HeapNodeObjectPool(), LazyThreadSafetyMode.PublicationOnly);

        private static HeapNodeObjectPool HeapNodePool => HeapNodePoolLoader.Value;
        
        private readonly List<HeapNode> _binaryHeap;
        private readonly IComparer<T> _comparer;
        private readonly Dictionary<T, HeapNode> _lookup;

        private static List<HeapNode> HeapOrderedListFromEnumerable(IComparer<T> comparer, IEnumerable<T> enumerable) =>
            enumerable.OrderBy(i => i, comparer).Select((item, i) => HeapNodePool.Get(i, item)).ToList();

        public BinaryHeap() : this(Comparer<T>.Default) { }
        
        public BinaryHeap(IEnumerable<T> containers) : this(Comparer<T>.Default, containers) { }

        public BinaryHeap(IComparer<T> comparer, IEnumerable<T> containers) : 
            this(comparer, HeapOrderedListFromEnumerable(comparer, containers)) { }

        public BinaryHeap(IComparer<T> comparer, int capacity = 0) : this(comparer, new List<T>(capacity)) { }

        private BinaryHeap(IComparer<T> comparer, List<HeapNode> binaryHeap)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _binaryHeap = binaryHeap ?? throw new ArgumentNullException(nameof(binaryHeap));
            _lookup = new Dictionary<T, HeapNode>();
        }

        /// <inheritdoc />
        public int Count => _binaryHeap.Count;

        /// <inheritdoc />
        public bool IsEmpty => !_binaryHeap.Any();

        /// <inheritdoc />
        public bool TryPeek(out T value)
        {
            value = default;
            if (!_binaryHeap.Any()) return false;

            value = _binaryHeap[0].Item;
            return true;
        }
        
        /// <inheritdoc />
        public bool TryPop(out T value)
        {
            value = default;
            if (IsEmpty) return false;

            // Swap then remove.
            var root = _binaryHeap[0];
            Swap(0, Count - 1);
            _binaryHeap.RemoveAt(Count - 1);

            // Assign item and remove it from mapping.
            value = root.Item;
            _lookup.Remove(root.Item);
            HeapNodePool.Return(root);
            
            // If there aren't any elements left, no sifting is needed.
            if (1 < Count) SiftDown(0);

            return true;
        }
        
        /// <inheritdoc />
        public void Push(T value)
        {
            var node = HeapNodePool.Get(Count, value);
            _binaryHeap.Add(node);
            _lookup.Add(value, node);
            SiftUp(Count - 1);
        }

        /// <inheritdoc />
        public bool TryReplace(T newValue, out T oldValue)
        {
            oldValue = default;
            if (IsEmpty) return false;

            var node = _binaryHeap[0];
            oldValue = node.Item;
            _lookup.Remove(node.Item);
            _lookup.Add(newValue, node);
            node.Item = newValue;
            SiftDown(node.Index);
            
            return true;
        }

        /// <inheritdoc />
        public bool TryUpdate(T oldValue, T newValue)
        {
            if (!_lookup.TryGetValue(oldValue, out var node)) return false;

            _lookup.Remove(oldValue);
            _lookup.Add(newValue, node);
            node.Item = newValue;
            
            var index = SiftUp(node.Index);
            SiftDown(index);
            return true;
        }

        private bool SmallerThan(T lhs, T rhs) => _comparer.Compare(lhs, rhs) < 0;

        private void Swap(int fstIndex, int sndIndex)
        {
            var fstNode = _binaryHeap[fstIndex];
            var sndNode = _binaryHeap[sndIndex];

            sndNode.Index = fstIndex;
            _binaryHeap[fstIndex] = sndNode;

            fstNode.Index = sndIndex;
            _binaryHeap[sndIndex] = fstNode;
        }

        #region SiftUp

        private int SiftUp(int nodeIndex)
        {
            var node = _binaryHeap[nodeIndex];
            while (nodeIndex != 0)
            {
                var parentIndex = (nodeIndex - 1) / 2;
                var parent = _binaryHeap[parentIndex];

                if (!SmallerThan(node.Item, parent.Item)) return nodeIndex;
                Swap(nodeIndex, parentIndex);
                nodeIndex = parentIndex;
            }

            return 0;
        }

        #endregion

        #region SiftDown

        private void SiftDown(int nodeIndex)
        {
            var node = _binaryHeap[nodeIndex];
            while (true)
            {
                // To keep the heap invariant we have to compare the keys of both child nodes of this node.
                var leftIndex = nodeIndex * 2 + 1;
                var rightIndex = nodeIndex * 2 + 2;

                if (Count <= leftIndex)
                    return;

                // There is only a left node, decide which node is the smaller one.
                if (Count <= rightIndex)
                {
                    var leftNode = _binaryHeap[leftIndex];
                    if (SmallerThan(node.Item, leftNode.Item))
                        return;
                    Swap(nodeIndex, leftIndex);
                    nodeIndex = leftIndex;
                    continue;
                }

                // Compare both nodes.
                var leftValue = _binaryHeap[leftIndex];
                var rightValue = _binaryHeap[rightIndex];
                if (SmallerThan(node.Item, leftValue.Item) && SmallerThan(node.Item, rightValue.Item))
                    return;
                
                var swapIndex = SmallerThan(leftValue.Item, rightValue.Item) ? leftIndex : rightIndex;
                Swap(nodeIndex, swapIndex);
                nodeIndex = swapIndex;
            }
        }

        #endregion

        #region HeapNode

        private class HeapNode
        {
            public T Item { get; set; }
            public int Index { get; set; }
        }

        private class HeapNodeObjectPool : ObjectPoolBase<HeapNode>
        {
            /// <inheritdoc />
            protected override HeapNode CreateInstance() => new HeapNode();

            /// <inheritdoc />
            protected override HeapNode ResetInstance(HeapNode instance)
            {
                instance.Index = default;
                instance.Item = default;
                return instance;
            }

            public HeapNode Get(int index, T item)
            {
                var instance = Get();
                instance.Index = index;
                instance.Item = item;
                return instance;
            }
        }

        #endregion

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => 
            _binaryHeap.Select(n => n.Item).GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => 
            ((IEnumerable<T>) this).GetEnumerator();
    }
}