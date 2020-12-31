using Sammlung.Utilities;

namespace Sammlung.Queues
{
    public abstract class DequeBase<T> : IDeque<T>
    {
        /// <inheritdoc />
        public abstract int Count { get; }

        /// <inheritdoc />
        public abstract void PushLeft(T element);

        /// <inheritdoc />
        public T PopRight() =>
            TryPopRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public abstract bool TryPopRight(out T element);

        /// <inheritdoc />
        public T PeekRight()=>
            TryPeekRight(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public abstract bool TryPeekRight(out T element);

        /// <inheritdoc />
        public abstract void PushRight(T element);

        /// <inheritdoc />
        public T PopLeft() =>
            TryPopLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public abstract bool TryPopLeft(out T element);

        /// <inheritdoc />
        public T PeekLeft() =>
            TryPeekLeft(out var element) ? element : throw ExceptionsHelper.NewEmptyCollectionException();

        /// <inheritdoc />
        public abstract bool TryPeekLeft(out T element);
    }
}