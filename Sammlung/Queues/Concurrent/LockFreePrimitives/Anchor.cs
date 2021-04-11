using JetBrains.Annotations;

namespace Sammlung.Queues.Concurrent.LockFreePrimitives
{
    /// <summary>
    /// The <see cref="Anchor{T}"/> is a special data structure needed for the implementation of a lock-free deque.
    /// </summary>
    /// <typeparam name="T">the element type</typeparam>
    internal sealed class Anchor<T>
    {
        public static Anchor<T> Create() => new Anchor<T> { ReferenceCount = 0 };
        public static Anchor<T> Create([NotNull] Node<T> left, [NotNull] Node<T> right, State state) => 
            new Anchor<T>{ LeftMost = left, RightMost = right, State = state, ReferenceCount = 0};

        public int ReferenceCount;
        public Node<T> LeftMost;
        public Node<T> RightMost;
        public State State;
        
        private Anchor() {}
    }
}