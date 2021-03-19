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
}