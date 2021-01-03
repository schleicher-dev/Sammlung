using Sammlung.Utilities;

namespace Sammlung.Heaps
{
    public abstract class HeapBase<T> : IHeap<T>
    {
        /// <inheritdoc />
        public abstract int Count { get; }
        
        /// <inheritdoc />
        public abstract bool IsEmpty { get; }

        /// <inheritdoc />
        public abstract bool TryPeek(out T value);

        /// <inheritdoc />
        public abstract bool TryPop(out T value);

        /// <inheritdoc />
        public abstract void Push(T value);

        /// <inheritdoc />
        public abstract bool TryReplace(T newValue, out T oldValue);

        /// <inheritdoc />
        public abstract bool TryUpdate(T oldValue, T newValue);
    }
}