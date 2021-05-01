using System;
using System.Threading;

namespace Sammlung.Utilities.Concurrent
{
    /// <summary>
    /// The <see cref="EnhancedReaderWriterLock"/> extends the <seealso cref="ReaderWriterLockSlim"/> entity with
    /// the RAII pattern. (Resource acquisition is instantiation)
    /// </summary>
    internal sealed class EnhancedReaderWriterLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// Creates a new <see cref="EnhancedReaderWriterLock"/> using a <seealso cref="LockRecursionPolicy"/>.
        /// </summary>
        /// <param name="policy">the policy</param>
        public EnhancedReaderWriterLock(LockRecursionPolicy policy)
        {
            _rwLock = new ReaderWriterLockSlim(policy);
        }

        /// <summary>
        /// Creates a read lock handle.
        /// </summary>
        /// <returns>the read lock handle</returns>
        public IDisposable UseReadLock() => InternalHandle.CreateReadHandle(_rwLock);

        /// <summary>
        /// Creates a write lock handle.
        /// </summary>
        /// <returns>the write lock handle</returns>
        public IDisposable UseWriteLock() => InternalHandle.CreateWriteHandle(_rwLock);

        /// <summary>
        /// Creates an upgradable read lock handle.
        /// </summary>
        /// <returns>the upgradable read lock handle.</returns>
        public IUpgradableLockHandle UseUpgradableReadLock() => InternalHandle.CreateUpgradableWriteHandle(_rwLock);
        
        /// <inheritdoc />
        public void Dispose()
        {
            _rwLock?.Dispose();
        }
        
        private sealed class InternalHandle : IUpgradableLockHandle
        {
            public static IDisposable CreateReadHandle(ReaderWriterLockSlim rwLock)
                => new InternalHandle(rwLock.EnterReadLock, rwLock.ExitReadLock);
            
            public static IDisposable CreateWriteHandle(ReaderWriterLockSlim rwLock)
                => new InternalHandle(rwLock.EnterWriteLock, rwLock.ExitWriteLock);

            public static IUpgradableLockHandle CreateUpgradableWriteHandle(ReaderWriterLockSlim rwLock)
                => new InternalHandle(rwLock.EnterUpgradeableReadLock, rwLock.ExitUpgradeableReadLock, () => CreateWriteHandle(rwLock));

            private readonly Action _exitAction;
            private readonly Func<IDisposable> _createUpgradedHandle;
            private IDisposable _upgradedHandle;

            private InternalHandle(Action enterAction, Action exitAction, Func<IDisposable> createUpgradedHandle = null)
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