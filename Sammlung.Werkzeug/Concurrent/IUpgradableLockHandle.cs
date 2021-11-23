using System;

namespace Sammlung.Werkzeug.Concurrent
{
    /// <summary>
    /// The <see cref="IUpgradableLockHandle"/> exposes a possibility to upgrade a read lock to a write lock. 
    /// </summary>
    public interface IUpgradableLockHandle : IDisposable
    {
        /// <summary>
        /// Upgrades the read-lock to a write lock.
        /// </summary>
        public void Upgrade();
    }
}