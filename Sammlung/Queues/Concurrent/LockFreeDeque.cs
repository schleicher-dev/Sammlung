using System.Threading;

namespace Sammlung.Queues.Concurrent
{
    /// <summary>
    /// The <see cref="LockFreeDeque{T}"/> is a <seealso cref="IDeque{T}"/> type which does not use any locks to achieve
    /// concurrency.
    /// </summary>
    /// <remarks>
    /// Inspiration from: http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.93.7492&amp;rep=rep1&amp;type=pdf
    /// DOI: 10.1.1.93.7492
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class LockFreeDeque<T> : DequeBase<T>
    {
        private Anchor _anchor;
        private int _count;
        private SpinWait _spinWait;

        public LockFreeDeque()
        {
            _anchor = new Anchor();
            _spinWait = new SpinWait();
        }

        /// <inheritdoc />
        public override int Count => _count;

        private static Node CreateNode(T value) => new Node(value);

        private static bool CompareAndSwap<TPtr>(ref TPtr field, TPtr exchange, TPtr compare) where TPtr : class =>
            Interlocked.CompareExchange(ref field, exchange, compare) == compare;

        private static void UpdateAnchor(Anchor anchor, Node leftMost, Node rightMost, State state)
        {
            anchor.LeftMost = leftMost;
            anchor.RightMost = rightMost;
            anchor.State = state;
        }

        private void BackOff()
        {
            _spinWait.SpinOnce();
        }

        private void Stabilize(Anchor anchor)
        {
            if (anchor.State == State.LeftPush) StabilizeLeft(anchor);
            else StabilizeRight(anchor);
        }

        private void StabilizeLeft(Anchor anchor)
        {
            if (_anchor != anchor) return;
            var newNode = anchor.LeftMost;
            var next = newNode?.Right;
            if (next == null) return;
            var nextPrev = next.Left;
            if (nextPrev != newNode)
            {
                if (_anchor != anchor) return;
                if (!CompareAndSwap(ref next.Left, newNode, nextPrev)) return;
            }

            var swapAnchor = new Anchor
                {LeftMost = anchor.LeftMost, RightMost = anchor.RightMost, State = State.Stable};
            CompareAndSwap(ref _anchor, swapAnchor, anchor);
        }

        private void StabilizeRight(Anchor anchor)
        {
            if (_anchor != anchor) return;
            var newNode = anchor.RightMost;
            var prev = newNode.Left;
            if (prev == null) return;
            var prevNext = prev.Right;
            if (prevNext != newNode)
            {
                if (_anchor != anchor) return;
                if (!CompareAndSwap(ref prev.Right, newNode, prevNext)) return;
            }

            var swapAnchor = new Anchor
                {LeftMost = anchor.LeftMost, RightMost = anchor.RightMost, State = State.Stable};
            CompareAndSwap(ref _anchor, swapAnchor, anchor);
        }

        /// <inheritdoc />
        public override void PushLeft(T element)
        {
            var node = CreateNode(element);
            var swapAnchor = new Anchor();
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
        public override bool TryPopRight(out T element)
        {
            element = default;
            var swapAnchor = new Anchor();
            Anchor anchor;
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
        public override bool TryPeekRight(out T element)
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
        public override void PushRight(T element)
        {
            var node = CreateNode(element);
            var swapAnchor = new Anchor();
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
        public override bool TryPopLeft(out T element)
        {
            element = default;
            var swapAnchor = new Anchor();
            Anchor anchor;
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
        public override bool TryPeekLeft(out T element)
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

        #region InnerClases

        private enum State
        {
            Stable,
            RightPush,
            LeftPush
        }

        private sealed class Node
        {
            public Node Left;
            public Node Right;
            public readonly T Value;

            public Node(T value)
            {
                Value = value;
                Left = null;
                Right = null;
            }
        }

        private sealed class Anchor
        {
            public Node LeftMost;
            public Node RightMost;
            public State State;
        }

        #endregion
    }
}