using System;
using System.Collections.Concurrent;
using Sammlung.Queues;
using Sammlung.Queues.Concurrent;

namespace Sammlung.Utilities.Patterns
{
    internal abstract class ObjectPoolBase<T> : IObjectPool<T> where T : class
    {
        protected const int DefaultMaxPoolSize = 256;
        private readonly int _maxPoolSize;
        private readonly ConcurrentBag<T> _pool;

        protected ObjectPoolBase(int maxPoolSize = DefaultMaxPoolSize)
        {
            _maxPoolSize = 0 < maxPoolSize
                ? maxPoolSize
                : throw new ArgumentOutOfRangeException(nameof(maxPoolSize), maxPoolSize,
                    $"{maxPoolSize} must be strictly positive");

            _pool = new ConcurrentBag<T>();
        }

        protected abstract T CreateInstance();

        protected abstract T ResetInstance(T instance);

        /// <inheritdoc />
        public T Get()
        {
            var count = _pool.Count;
            for (var i = 0; count == 0 && i < System.Math.Min(16, _maxPoolSize / 2); i++)
                _pool.Add(CreateInstance());
            return _pool.TryTake(out var item) ? item : CreateInstance();
        }

        /// <inheritdoc />
        public void Return(T instance)
        {
            if (_maxPoolSize <= _pool.Count) return;
            _pool.Add(ResetInstance(instance));
        }

        /// <inheritdoc />
        public void Reset(T instance) => ResetInstance(instance);
    }
}