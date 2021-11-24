namespace Sammlung.Collections.Queues.Concurrent.LockFreePrimitives
{
    /// <summary>
    /// The <see cref="State"/> enum represents the consistency state of the lock-free deque.
    /// </summary>
    internal enum State
    {
        Stable,
        RightPush,
        LeftPush
    }
}