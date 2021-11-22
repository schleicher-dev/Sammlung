using System;
using System.Collections.Generic;
using System.Threading;
using Sammlung.Resources;
using Sammlung.Utilities.Concurrent;

namespace Sammlung.Utilities.Patterns
{
    internal abstract class ObjectPoolBase<T> where T : class
    {
        private const int DefaultMaxPoolSize = 256;
        private readonly int _maxPoolSize;
        private readonly Stack<T> _pool;
        private readonly EnhancedReaderWriterLock _rwLock;

        protected ObjectPoolBase(int maxPoolSize = DefaultMaxPoolSize)
        {
            _maxPoolSize = maxPoolSize.RequireStrictlyPositive(nameof(maxPoolSize));
            _rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
            _pool = new Stack<T>();
        }

        protected abstract T CreateInstance();

        protected abstract T ResetInstance(T instance);

        protected T Get()
        {
            using var _ = _rwLock.UseWriteLock();
            return _pool.Count != 0 ? _pool.Pop(): CreateInstance();
        }

        public void Return(T instance)
        {
            using var upgradableLockHandle = _rwLock.UseUpgradableReadLock();
            if (_maxPoolSize <= _pool.Count) return;
            upgradableLockHandle.Upgrade();
            var resetInstance = Reset(instance);
            _pool.Push(resetInstance);
        }

        private T Reset(T instance) => ResetInstance(instance);
    }
}