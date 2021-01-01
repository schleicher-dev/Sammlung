using System;
using System.Threading;

namespace Sammlung.Queues.Concurrent.LockFreePrimitives
{
    internal sealed class Anchor<T>
    {
        public static Anchor<T> Create() => new Anchor<T>();
        public static Anchor<T> Create(Node<T> left, Node<T> right, State state) => 
            new Anchor<T>{ LeftMost = left, RightMost = right, State = state};

        public Node<T> LeftMost;
        public Node<T> RightMost;
        public State State;
        
        private Anchor() {}
    }
}