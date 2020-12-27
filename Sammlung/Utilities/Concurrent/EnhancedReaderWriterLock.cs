using System;
using System.Threading;

namespace Sammlung.Utilities.Concurrent
{
    internal sealed class EnhancedReaderWriterLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        public EnhancedReaderWriterLock(LockRecursionPolicy policy)
        {
            _rwLock = new ReaderWriterLockSlim(policy);
        }

        public ILockHandle UseReadLock() => InternalHandle.CreateReadHandle(_rwLock);

        public ILockHandle UseWriteLock() => InternalHandle.CreateWriteHandle(_rwLock);

        public IUpgradableLockHandle UseUpgradableReadLock() => InternalHandle.CreateUpgradableWriteHandle(_rwLock);
        
        public void Dispose()
        {
            _rwLock?.Dispose();
        }
        
        private sealed class InternalHandle : IUpgradableLockHandle
        {
            public static ILockHandle CreateReadHandle(ReaderWriterLockSlim rwLock)
                => new InternalHandle(rwLock.EnterReadLock, rwLock.ExitReadLock);
            
            public static ILockHandle CreateWriteHandle(ReaderWriterLockSlim rwLock)
                => new InternalHandle(rwLock.EnterWriteLock, rwLock.ExitWriteLock);

            public static IUpgradableLockHandle CreateUpgradableWriteHandle(ReaderWriterLockSlim rwLock)
                => new InternalHandle(rwLock.EnterUpgradeableReadLock, rwLock.ExitUpgradeableReadLock, () => CreateWriteHandle(rwLock));

            private readonly Action _exitAction;
            private readonly Func<ILockHandle> _createUpgradedHandle;
            private ILockHandle _upgradedHandle;

            private InternalHandle(Action enterAction, Action exitAction, Func<ILockHandle> createUpgradedHandle = null)
            {
                enterAction = enterAction ?? throw new ArgumentNullException(nameof(enterAction));
                _exitAction = exitAction ?? throw new ArgumentNullException(nameof(exitAction));
                _createUpgradedHandle = createUpgradedHandle;
                _upgradedHandle = null;

                enterAction.Invoke();
            }

            public void Upgrade()
            {
                _upgradedHandle = _createUpgradedHandle?.Invoke();
            }

            public void Dispose()
            {
                _upgradedHandle?.Dispose();
                _exitAction.Invoke();
            }
        }
    }
}