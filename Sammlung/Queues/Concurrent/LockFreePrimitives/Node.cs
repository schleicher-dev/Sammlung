namespace Sammlung.Queues.Concurrent.LockFreePrimitives
{
    internal sealed class Node<T>
    {
        public Node<T> Left;
        public Node<T> Right;
        public readonly T Value;

        public Node(T value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }
}