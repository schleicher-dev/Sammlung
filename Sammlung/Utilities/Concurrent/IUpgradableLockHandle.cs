using System;

namespace Sammlung.Utilities.Concurrent
{
    /// <summary>
    /// The <see cref="IUpgradableLockHandle"/> exposes a possibility to upgrade a read lock to a write lock. 
    /// </summary>
    internal interface IUpgradableLockHandle : IDisposable
    {
        /// <summary>
        /// Upgrades the read-lock to a write lock.
        /// </summary>
        public void Upgrade();
    }
}