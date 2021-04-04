using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Sammlung.Queues.Concurrent.LockFreePrimitives;

namespace Sammlung.Queues.Concurrent
{
    /// <summary>
    /// The <see cref="LockFreeLinkedDeque{T}"/> is a <seealso cref="IDeque{T}"/> type which does not use any locks to achieve
    /// concurrency.
    /// </summary>
    /// <remarks>
    /// Inspiration from: http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.93.7492&amp;rep=rep1&amp;type=pdf
    /// DOI: 10.1.1.93.7492
    /// </remarks>
    /// <typeparam name="T">the type</typeparam>
    public class LockFreeLinkedDeque<T> : IDeque<T>
    {
        private Anchor<T> _anchor;
        private int _count;

        /// <summary>
        /// Creates a new <see cref="LockFreeLinkedDeque{T}"/>.
        /// </summary>
        public LockFreeLinkedDeque()
        {
            _anchor = Anchor<T>.Create();
        }

        /// <inheritdoc />
        public int Count => _count;

        private static Node<T> CreateNode(T value) => new Node<T>(value);

        private static bool CompareAndSwap<TPtr>(ref TPtr field, TPtr exchange, TPtr compare) where TPtr : class =>
            Interlocked.CompareExchange(ref field, exchange, compare) == compare;

        private static void UpdateAnchor(Anchor<T> anchor, Node<T> leftMost, Node<T> rightMost, State state)
        {
            anchor.LeftMost = leftMost;
            anchor.RightMost = rightMost;
            anchor.State = state;
        }

        private static void BackOff() => Thread.SpinWait(100);

        private void Stabilize(Anchor<T> anchor)
        {
            if (anchor.State == State.LeftPush) StabilizeLeft(anchor);
            else StabilizeRight(anchor);
        }

        private void StabilizeLeft(Anchor<T> anchor)
        {
            if (_anchor != anchor) return;
            var newNode = anchor.LeftMost;
            var next = newNode.Right;
            var nextPrev = next.Left;
            if (nextPrev != newNode)
            {
                if (_anchor != anchor) return;
                if (!CompareAndSwap(ref next.Left, newNode, nextPrev)) return;
            }

            var swapAnchor = Anchor<T>.Create(anchor.LeftMost, anchor.RightMost, State.Stable);
            CompareAndSwap(ref _anchor, swapAnchor, anchor);
        }

        private void StabilizeRight(Anchor<T> anchor)
        {
            if (_anchor != anchor) return;
            var newNode = anchor.RightMost;
            var prev = newNode.Left;
            var prevNext = prev.Right;
            if (prevNext != newNode)
            {
                if (_anchor != anchor) return;
                if (!CompareAndSwap(ref prev.Right, newNode, prevNext)) return;
            }

            var swapAnchor = Anchor<T>.Create(anchor.LeftMost, anchor.RightMost, State.Stable);
            CompareAndSwap(ref _anchor, swapAnchor, anchor);
        }

        /// <inheritdoc />
        public void PushLeft(T element)
        {
            var node = CreateNode(element);
            var swapAnchor = Anchor<T>.Create();
            while (true)
            {
                var anchor = _anchor;
                if (anchor.LeftMost == null)
                {
                    UpdateAnchor(swapAnchor, node, node, State.Stable);
                    if (CompareAndSwap(ref _anchor, swapAnchor, anchor)) break;
                }
                else if (anchor.State == State.Stable)
                {
                    node.Right = anchor.LeftMost;
                    UpdateAnchor(swapAnchor, node, anchor.RightMost, State.LeftPush);
                    if (!CompareAndSwap(ref _anchor, swapAnchor, anchor)) continue;

                    StabilizeLeft(swapAnchor);
                    break;
                }
                else
                {
                    Stabilize(anchor);
                }
                
                BackOff();
            }
            
            Interlocked.Increment(ref _count);
        }

        /// <inheritdoc />
        public bool TryPopRight(out T element)
        {
            element = default;
            var swapAnchor = Anchor<T>.Create();
            Anchor<T> anchor;
            while (true)
            {
                anchor = _anchor;
                if (anchor.RightMost == null) return false;
                if (anchor.LeftMost == anchor.RightMost)
                {
                    UpdateAnchor(swapAnchor, null, null, State.Stable);
                    if (CompareAndSwap(ref _anchor, swapAnchor, anchor)) break;
                }
                else if (anchor.State == State.Stable)
                {
                    UpdateAnchor(swapAnchor, anchor.LeftMost, anchor.RightMost.Left, State.Stable);
                    if (CompareAndSwap(ref _anchor, swapAnchor, anchor)) break;
                }
                else
                {
                    Stabilize(anchor);
                }

                BackOff();
            }

            Interlocked.Decrement(ref _count);
            element = anchor.RightMost.Value;
            return true;
        }

        /// <inheritdoc />
        public bool TryPeekRight(out T element)
        {
            var anchor = _anchor;
            if (anchor.RightMost == null)
            {
                element = default;
                return false;
            }

            element = anchor.RightMost.Value;
            return true;
        }

        /// <inheritdoc />
        public void PushRight(T element)
        {
            var node = CreateNode(element);
            var swapAnchor = Anchor<T>.Create();
            while (true)
            {
                var anchor = _anchor;
                if (anchor.RightMost == null)
                {
                    UpdateAnchor(swapAnchor, node, node, State.Stable);
                    if (CompareAndSwap(ref _anchor, swapAnchor, anchor)) break;
                }
                else if (anchor.State == State.Stable)
                {
                    node.Left = anchor.RightMost;
                    UpdateAnchor(swapAnchor, anchor.LeftMost, node, State.RightPush);
                    if (!CompareAndSwap(ref _anchor, swapAnchor, anchor)) continue;

                    StabilizeRight(swapAnchor);
                    break;
                }
                else
                {
                    Stabilize(anchor);
                }
                
                BackOff();
            }

            Interlocked.Increment(ref _count);
        }

        /// <inheritdoc />
        public bool TryPopLeft(out T element)
        {
            element = default;
            var swapAnchor = Anchor<T>.Create();
            Anchor<T> anchor;
            while (true)
            {
                anchor = _anchor;
                if (anchor.LeftMost == null) return false;
                if (anchor.LeftMost == anchor.RightMost)
                {
                    UpdateAnchor(swapAnchor, null, null, State.Stable);
                    if (CompareAndSwap(ref _anchor, swapAnchor, anchor)) break;
                }
                else if (anchor.State == State.Stable)
                {
                    UpdateAnchor(swapAnchor, anchor.LeftMost.Right, anchor.RightMost, State.Stable);
                    if (CompareAndSwap(ref _anchor, swapAnchor, anchor)) break;
                }
                else
                {
                    Stabilize(anchor);
                }
                
                BackOff();
            }

            Interlocked.Decrement(ref _count);
            element = anchor.LeftMost.Value;
            return true;
        }

        /// <inheritdoc />
        public bool TryPeekLeft(out T element)
        {
            var anchor = _anchor;
            if (anchor.LeftMost == null)
            {
                element = default;
                return false;
            }

            element = anchor.LeftMost.Value;
            return true;
        }

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => 
            throw new NotSupportedException("Enumeration on a lock-free linked deque is not supported.");

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>) this).GetEnumerator();
    }
}