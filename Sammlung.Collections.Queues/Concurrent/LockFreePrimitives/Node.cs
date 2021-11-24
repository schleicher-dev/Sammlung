namespace Sammlung.Collections.Queues.Concurrent.LockFreePrimitives
{
    /// <summary>
    /// The <see cref="Node{T}"/> is a type needed for the implementation of a lock-free deque.
    /// </summary>
    /// <typeparam name="T">the element type</typeparam>
    internal sealed class Node<T>
    {
        /// <summary>
        /// The left node.
        /// </summary>
        public Node<T> Left;
        
        /// <summary>
        /// The right node.
        /// </summary>
        public Node<T> Right;
        
        /// <summary>
        /// The value of the node.
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// Creates a new <see cref="Node{T}"/> using a value of the node.
        /// </summary>
        /// <param name="value">the value</param>
        public Node(T value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }
}