using System;
using System.Threading;
using Sammlung.Utilities.Patterns;

namespace Sammlung.Queues.Concurrent.LockFreePrimitives
{
    internal sealed class AnchorObjectPool<T> : ObjectPoolBase<Anchor<T>>
    {
        /// <inheritdoc />
        protected override Anchor<T> CreateInstance() => Anchor<T>.Create();

        /// <inheritdoc />
        protected override Anchor<T> ResetInstance(Anchor<T> instance)
        {
            instance.LeftMost = default;
            instance.RightMost = default;
            instance.State = default;
            instance.ReferenceCount = default;
            return instance;
        }
    }
    
    internal sealed class Anchor<T>
    {
        public static Anchor<T> Create() => new Anchor<T> { ReferenceCount = 0 };
        public static Anchor<T> Create(Node<T> left, Node<T> right, State state) => 
            new Anchor<T>{ LeftMost = left, RightMost = right, State = state, ReferenceCount = 0};

        public int ReferenceCount;
        public Node<T> LeftMost;
        public Node<T> RightMost;
        public State State;
        
        private Anchor() {}
    }
}