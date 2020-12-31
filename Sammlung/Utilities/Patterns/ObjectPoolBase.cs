using System;
using System.Collections.Concurrent;
using Sammlung.Queues;
using Sammlung.Queues.Concurrent;

namespace Sammlung.Utilities.Patterns
{
    internal abstract class ObjectPoolBase<T> : IObjectPool<T> where T : class
    {
        private const int DefaultMaxPoolSize = 256;
        private readonly int _maxPoolSize;
        private readonly ConcurrentBag<WeakReference<T>> _pool;

        protected ObjectPoolBase(int maxPoolSize = DefaultMaxPoolSize)
        {
            _maxPoolSize = 0 < maxPoolSize
                ? maxPoolSize
                : throw new ArgumentOutOfRangeException(nameof(maxPoolSize), maxPoolSize,
                    $"{maxPoolSize} must be strictly positive");

            _pool = new ConcurrentBag<WeakReference<T>>();
        }

        protected abstract T CreateInstance();

        protected abstract T ResetInstance(T instance);

        /// <inheritdoc />
        public T Get()
        {
            while (_pool.TryTake(out var reference))
            {
                if (!reference.TryGetTarget(out var instance)) continue;
                return instance;
            }
            
            return CreateInstance();
        }

        /// <inheritdoc />
        public void Return(T instance)
        {
            if (_maxPoolSize <= _pool.Count) return;
            var resetInstance = Reset(instance);
            _pool.Add(new WeakReference<T>(resetInstance));
        }

        /// <inheritdoc />
        public T Reset(T instance) => ResetInstance(instance);
    }
}