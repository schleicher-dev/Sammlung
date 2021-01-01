using System;
using System.Collections.Concurrent;
using Sammlung.Queues;
using Sammlung.Queues.Concurrent;
using Sammlung.Resources;

namespace Sammlung.Utilities.Patterns
{
    internal abstract class ObjectPoolBase<T> : IObjectPool<T> where T : class
    {
        private const int DefaultMaxPoolSize = 256;
        private readonly int _maxPoolSize;
        private readonly ConcurrentBag<T> _pool;

        protected ObjectPoolBase(int maxPoolSize = DefaultMaxPoolSize)
        {
            _maxPoolSize = 0 < maxPoolSize
                ? maxPoolSize
                : throw new ArgumentOutOfRangeException(nameof(maxPoolSize), maxPoolSize,
                    string.Format(ErrorMessages.ValueMustBeStrictlyPositive, maxPoolSize));

            _pool = new ConcurrentBag<T>();
        }

        protected abstract T CreateInstance();

        protected abstract T ResetInstance(T instance);

        /// <inheritdoc />
        public T Get()
        {
            return _pool.TryTake(out var reference) ? reference: CreateInstance();
        }

        /// <inheritdoc />
        public void Return(T instance)
        {
            if (_maxPoolSize <= _pool.Count) return;
            var resetInstance = Reset(instance);
            _pool.Add(resetInstance);
        }

        /// <inheritdoc />
        public T Reset(T instance) => ResetInstance(instance);
    }
}