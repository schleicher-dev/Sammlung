namespace Sammlung.Utilities.Concurrent
{
    internal interface IUpgradableLockHandle : ILockHandle
    {
        public void Upgrade();
    }
}