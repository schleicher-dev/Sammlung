namespace Sammlung.Queues.Concurrent.LockFreePrimitives
{
    internal enum State
    {
        Stable,
        RightPush,
        LeftPush
    }
}