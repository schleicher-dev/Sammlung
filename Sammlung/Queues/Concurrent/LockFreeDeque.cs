using System;
using System.Net.NetworkInformation;
using System.Threading;
using Sammlung.Utilities;
using Sammlung.Utilities.Patterns;

namespace Sammlung.Queues.Concurrent
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Ideas taken from: http://www.non-blocking.com/download/sunt04_deque_tr.pdf</remarks>
    /// <typeparam name="T"></typeparam>
    public class LockFreeDeque<T> : IDeque<T>
    {
        private readonly Node _head;
        private readonly Node _tail;

        /// <inheritdoc />
        public int Count { get; }

        public LockFreeDeque()
        {
            _head = CreateNode(default);
            _tail = CreateNode(default);

            _head.Link = new Link(null, _tail, false);
            _tail.Link = new Link(_head, null, false);
        }

        private static Node CreateNode(T value) => new Node {Value = value};

        private static Node CopyNode(Node node) => node;

        private static void ReleaseNode(Node node) { }

        private static void ReleaseReferences(Node node)
        {
            ReleaseNode(node.Link?.Prev);
            ReleaseNode(node.Link?.Next);
        }
        
        private static Node ReadPrev(Link link) => !link.DeleteMark ? link.Prev : null;
        private static Node ReadNext(Link link) => !link.DeleteMark ? link.Next : null;
        private static Node ReadPrevDel(Link link) => link.Prev;
        private static Node ReadNextDel(Link link) => link.Next;
        private static bool CompareAndSwap(ref Link subject, Link compare, Link exchange)
        {
            return Interlocked.CompareExchange(ref subject, exchange, compare) == compare;
        }

        private static void BackOff() => Thread.Yield();

        private static void RemoveCrossReference(Node node)
        {
            while (true)
            {
                var link1 = node.Link;
                { // Remove prev reference
                    var prev = link1.Prev;
                    if (prev.Link.DeleteMark)
                    {
                        var prev2 = ReadPrevDel(prev.Link);
                        node.Link = new Link(prev2, link1.Next, true);
                        ReleaseNode(prev);
                        continue;
                    }
                }
                { // Remove next reference
                    var next = link1.Next;
                    if (next.Link.DeleteMark)
                    {
                        var next2 = ReadNextDel(next.Link);
                        node.Link = new Link(link1.Prev, next2, true);
                        ReleaseNode(next);
                        continue;
                    }
                }

                break;
            }
        }

        private static void DeleteNext(Node node)
        {
            var lastDeleteMark = true;
            var prev = ReadPrevDel(node.Link);
            var next = ReadNextDel(node.Link);
            while (true)
            {
                if (ReferenceEquals(prev, next)) break;
                if (next.Link.DeleteMark)
                {
                    var next2 = ReadNextDel(next.Link);
                    ReleaseNode(next);
                    next = next2;
                    continue;
                }

                var prev2 = ReadNext(prev.Link);
                if (ReferenceEquals(prev2, null))
                {
                    if (!lastDeleteMark)
                    {
                        DeleteNext(prev);
                        lastDeleteMark = true;
                    }

                    prev2 = ReadPrevDel(prev.Link);
                    ReleaseNode(prev);
                    prev = prev2;
                    continue;
                }

                var link1 = new Link(prev.Link.Prev, prev2, false);
                if (!ReferenceEquals(prev2, node))
                {
                    lastDeleteMark = false;
                    ReleaseNode(prev);
                    prev = prev2;
                    continue;
                }
                ReleaseNode(prev2);

                var link2 = new Link(link1.Prev, node.Link.Next, false);
                if (CompareAndSwap(ref prev.Link, link1, link2))
                {
                    CopyNode(link2.Next);
                    ReleaseNode(node);
                    break;
                }

                BackOff();
            }
            ReleaseNode(prev);
            ReleaseNode(next);
        }

        private static Node HelpInsert(Node prev, Node node)
        {
            var lastDeleteMark = true;
            while (true)
            {
                var prev2 = ReadNext(prev.Link);
                if (ReferenceEquals(prev2, null))
                {
                    if (!lastDeleteMark)
                    {
                        DeleteNext(prev);
                        lastDeleteMark = true;
                    }

                    prev2 = ReadPrevDel(prev.Link);
                    ReleaseNode(prev);
                    prev = prev2;
                    continue;
                }

                var link1 = node.Link;
                if (link1.DeleteMark)
                {
                    ReleaseNode(prev2);
                    break;
                }

                if (!ReferenceEquals(prev2, node))
                {
                    lastDeleteMark = false;
                    ReleaseNode(prev);
                    prev = prev2;
                    continue;
                }
                ReleaseNode(prev2);

                var link2 = new Link(prev, link1.Next, false);
                if (CompareAndSwap(ref node.Link, link1, link2))
                {
                    CopyNode(prev);
                    ReleaseNode(link1.Prev);
                    if (prev.Link.DeleteMark) continue;
                    break;
                }
                
                BackOff();
            }

            return prev;
        }

        private static void PushCommon(Node node, Node next)
        {
            while (true)
            {
                var link1 = next.Link;
                var link2 = new Link(node, link1.Next, false);
                if (link1.DeleteMark || node.Link.DeleteMark || !ReferenceEquals(node.Link.Next, next))
                    break;
                if (CompareAndSwap(ref next.Link, link1, link2))
                {
                    CopyNode(node);
                    ReleaseNode(link1.Prev);
                    if (node.Link.DeleteMark)
                    {
                        var prev2 = CopyNode(node);
                        prev2 = HelpInsert(prev2, next);
                        ReleaseNode(prev2);
                    }
                    break;
                }
                
                BackOff();
            }
            ReleaseNode(next);
            ReleaseNode(node);
        }

        /// <inheritdoc />
        public void PushLeft(T element)
        {
            var node = CreateNode(element);
            var prev = CopyNode(_head);
            var next = ReadNext(prev.Link);
            while (true)
            {
                var link1 = prev.Link;
                if (!ReferenceEquals(link1.Next, next))
                {
                    ReleaseNode(next);
                    next = ReadNext(prev.Link);
                    continue;
                }

                node.Link = new Link(prev, link1.Next, false);
                var link2 = new Link(link1.Prev, node, false);
                if (CompareAndSwap(ref prev.Link, link1, link2))
                {
                    CopyNode(node);
                    break;
                }
                
                BackOff();
            }
            
            PushCommon(node, next);
        }

        /// <inheritdoc />
        public T PopRight() =>
            TryPopRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryPopRight(out T element)
        {
            element = default;
            
            var next = CopyNode(_tail);
            while (true)
            {
                var node = ReadPrev(next.Link);
                var link1 = node.Link;
                if (!ReferenceEquals(link1.Next, next) || link1.DeleteMark)
                {
                    HelpInsert(node, next);
                    ReleaseNode(node);
                    continue;
                }

                if (ReferenceEquals(node, _head))
                {
                    ReleaseNode(next);
                    ReleaseNode(node);
                    return false;
                }

                var prev = CopyNode(link1.Prev);
                var link2 = new Link(link1.Prev, link1.Next, true);
                if (CompareAndSwap(ref node.Link, link1, link2))
                {
                    DeleteNext(node);
                    prev = HelpInsert(prev, next);
                    ReleaseNode(prev);
                    ReleaseNode(next);

                    element = node.Value;
                    RemoveCrossReference(node);
                    ReleaseNode(node);
                    return true;
                }
                
                ReleaseNode(prev);
                ReleaseNode(node);
                BackOff();
            }
        }

        /// <inheritdoc />
        public T PeekRight()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool TryPeekRight(out T element)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void PushRight(T element)
        {
            var node = CreateNode(element);
            var next = CopyNode(_tail);
            var prev = ReadPrev(next.Link);
            while (true)
            {
                var link1 = prev.Link;
                if (!ReferenceEquals(link1.Next, next) || prev.Link.DeleteMark)
                {
                    prev = HelpInsert(prev, next);
                    continue;
                }

                node.Link = new Link(prev, link1.Next, false);
                var link2 = new Link(link1.Prev, node, false);
                if (CompareAndSwap(ref prev.Link, link1, link2))
                {
                    CopyNode(node);
                    break;
                }
                
                BackOff();
            }
            
            PushCommon(node, next);
        }

        /// <inheritdoc />
        public T PopLeft() =>
            TryPopLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public bool TryPopLeft(out T element)
        {
            element = default;
            
            var prev = CopyNode(_head);
            while (true)
            {
                var node = ReadNext(prev.Link);
                if (ReferenceEquals(node, _tail))
                {
                    ReleaseNode(node);
                    ReleaseNode(prev);
                    return false;
                }

                var link1 = node.Link;
                if (link1.DeleteMark)
                {
                    DeleteNext(node);
                    ReleaseNode(node);
                    continue;
                }

                var next = CopyNode(link1.Next);
                var link2 = new Link(link1.Prev, link1.Next, true);
                if (CompareAndSwap(ref node.Link, link1, link2))
                {
                    DeleteNext(node);
                    prev = HelpInsert(prev, next);
                    ReleaseNode(prev);
                    ReleaseNode(next);
                    
                    element = node.Value;
                    RemoveCrossReference(node);
                    ReleaseNode(node);
                    return true;
                }
                
                ReleaseNode(node);
                ReleaseNode(next);
                BackOff();
            }
        }

        /// <inheritdoc />
        public T PeekLeft()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool TryPeekLeft(out T element)
        {
            throw new System.NotImplementedException();
        }

        #region Internal Classes

        private class Link
        {
            public Node Prev;
            public Node Next;
            public bool DeleteMark;

            public Link() : this(null, null, false) { }

            public Link(Node prev, Node next, bool deleteMark)
            {
                Prev = prev;
                Next = next;
                DeleteMark = deleteMark;
            }

            /// <inheritdoc />
            public override bool Equals(object? obj) => Equals(obj as Link);

            protected bool Equals(Link other)
            {
                return ReferenceEquals(this, other) || !ReferenceEquals(other, null) &&
                       Equals(Prev, other.Prev) && Equals(Next, other.Next) && DeleteMark == other.DeleteMark;
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return HashCode.Combine(Prev, Next, DeleteMark);
            }

            public static bool operator ==(Link left, Link right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Link left, Link right)
            {
                return !Equals(left, right);
            }
        }

        private class Node
        {
            public T Value;
            public Link Link;
        }

        
        private class NodeObjectPool : ObjectPoolBase<Node>
        {
            /// <inheritdoc />
            public NodeObjectPool(int maxPoolSize = DefaultMaxPoolSize) : base(maxPoolSize) { }
        
            /// <inheritdoc />
            protected override Node CreateInstance() => new Node();
        
            /// <inheritdoc />
            protected override Node ResetInstance(Node instance)
            {
                instance.Value = default;
                instance.Link = default;
                return instance;
            }
        }

        #endregion
    }
}