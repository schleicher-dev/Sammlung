using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Werkzeug;
using Sammlung.Werkzeug.Patterns;

namespace Sammlung.Collections.Heaps
{
    /// <summary>
    /// The <see cref="BinaryHeap{T,TPriority}"/> class implements a <seealso cref="IHeap{T,TPriority}"/> data structure.
    /// </summary>
    /// <typeparam name="T">the element type</typeparam>
    /// <typeparam name="TPriority">the priority type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public sealed class BinaryHeap<T, TPriority> : IHeap<T, TPriority>
        where TPriority : IComparable<TPriority>
    {
        private static readonly HeapNodeObjectPool HeapNodePool = new HeapNodeObjectPool();

        private readonly List<HeapNode> _binaryHeap;
        private readonly IComparer<TPriority> _comparer;
        private readonly Dictionary<T, HeapNode> _lookup;

        private static List<HeapNode> HeapOrderedListFromEnumerable(IComparer<TPriority> comparer,
            IEnumerable<KeyValuePair<T, TPriority>> enumerable)
        {
            comparer = comparer.RequireNotNull(nameof(comparer));
            enumerable = enumerable.RequireNotNull(nameof(enumerable));
            
            return enumerable.OrderBy(i => i.Value, comparer)
                .Select((item, i) => HeapNodePool.Get(i, item.Key, item.Value)).ToList();
        }

        /// <summary>
        /// Creates an empty <see cref="BinaryHeap{T,TPriority}"/>.
        /// </summary>
        public BinaryHeap() : this(Comparer<TPriority>.Default)
        {
        }

        /// <summary>
        /// Creates a new <see cref="BinaryHeap{T,TPriority}"/> from a list of elements.
        /// </summary>
        /// <param name="elements">the elements</param>
        public BinaryHeap(IEnumerable<KeyValuePair<T, TPriority>> elements) :
            this(Comparer<TPriority>.Default, elements.RequireNotNull(nameof(elements)))
        {
        }

        /// <summary>
        /// Creates a new <see cref="BinaryHeap{T,TPriority}"/> using a comparer and a list of elements.
        /// </summary>
        /// <param name="comparer">the comparer</param>
        /// <param name="elements">the elements</param>
        public BinaryHeap(IComparer<TPriority> comparer, IEnumerable<KeyValuePair<T, TPriority>> elements) :
            this(comparer.RequireNotNull(nameof(comparer)), HeapOrderedListFromEnumerable(comparer, elements))
        {
        }

        /// <summary>
        /// Creates a new <see cref="BinaryHeap{T,TPriority}"/> using a comparer and a capacity value.
        /// </summary>
        /// <param name="comparer">the comparer</param>
        /// <param name="capacity">the capacity</param>
        public BinaryHeap(IComparer<TPriority> comparer, int capacity = 0) : this(comparer,
            new List<HeapNode>(capacity))
        {
        }

        private BinaryHeap(IComparer<TPriority> comparer, List<HeapNode> binaryHeap)
        {
            _comparer = comparer.RequireNotNull(nameof(comparer));
            _binaryHeap = binaryHeap.RequireNotNull(nameof(binaryHeap));
            _lookup = new Dictionary<T, HeapNode>();
        }

        /// <inheritdoc />
        public int Count => _binaryHeap.Count;

        /// <inheritdoc />
        public bool IsEmpty => !_binaryHeap.Any();

        /// <inheritdoc />
        public bool TryPeek(out HeapPair<T, TPriority> value)
        {
            value = default;
            if (!_binaryHeap.Any()) return false;

            var element = _binaryHeap[0];
            value = HeapPair.Create(element.Value, element.Priority);
            return true;
        }

        /// <inheritdoc />
        public bool TryPop(out HeapPair<T, TPriority> value)
        {
            value = default;
            if (IsEmpty) return false;

            // Swap then remove.
            var root = _binaryHeap[0];
            Swap(0, Count - 1);
            _binaryHeap.RemoveAt(Count - 1);

            // Assign item and remove it from mapping.
            value = HeapPair.Create(root.Value, root.Priority);
            _lookup.Remove(root.Value);
            HeapNodePool.Return(root);

            // If there aren't any elements left, no sifting is needed.
            if (1 < Count) SiftDown(0);

            return true;
        }

        /// <inheritdoc />
        public void Push(T value, TPriority priority)
        {
            var node = HeapNodePool.Get(Count, value, priority);
            _binaryHeap.Add(node);
            _lookup.Add(value, node);
            SiftUp(Count - 1);
        }

        /// <inheritdoc />
        public bool TryReplace(T newValue, TPriority priority, out HeapPair<T, TPriority> oldValue)
        {
            oldValue = default;
            if (IsEmpty) return false;

            var node = _binaryHeap[0];
            oldValue = HeapPair.Create(node.Value, node.Priority);
            _lookup.Remove(node.Value);
            _lookup.Add(newValue, node);
            node.Value = newValue;
            node.Priority = priority;
            SiftDown(node.Index);

            return true;
        }

        /// <inheritdoc />
        public bool TryUpdate(T oldValue, TPriority priority)
        {
            if (!_lookup.TryGetValue(oldValue, out var node)) return false;

            node.Priority = priority;
            var index = SiftUp(node.Index);
            SiftDown(index);
            return true;
        }

        private bool SmallerThan(TPriority lhs, TPriority rhs) => _comparer.Compare(lhs, rhs) < 0;

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

                if (!SmallerThan(node.Priority, parent.Priority)) return nodeIndex;
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
                    if (SmallerThan(node.Priority, leftNode.Priority))
                        return;
                    Swap(nodeIndex, leftIndex);
                    nodeIndex = leftIndex;
                    continue;
                }

                // Compare both nodes.
                var leftValue = _binaryHeap[leftIndex];
                var rightValue = _binaryHeap[rightIndex];
                if (SmallerThan(node.Priority, leftValue.Priority) && SmallerThan(node.Priority, rightValue.Priority))
                    return;

                var swapIndex = SmallerThan(leftValue.Priority, rightValue.Priority) ? leftIndex : rightIndex;
                Swap(nodeIndex, swapIndex);
                nodeIndex = swapIndex;
            }
        }

        #endregion

        #region HeapNode

        private class HeapNode
        {
            public T Value { get; set; }
            public TPriority Priority { get; set; }
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
                instance.Value = default;
                instance.Priority = default;
                return instance;
            }

            public HeapNode Get(int index, T item, TPriority priority)
            {
                var instance = Get();
                instance.Index = index;
                instance.Priority = priority;
                instance.Value = item;
                return instance;
            }
        }

        #endregion

        /// <inheritdoc />
        IEnumerator<HeapPair<T, TPriority>> IEnumerable<HeapPair<T, TPriority>>.GetEnumerator() =>
            _binaryHeap.Select(n => HeapPair.Create(n.Value, n.Priority)).GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable<HeapPair<T, TPriority>>) this).GetEnumerator();
    }
}