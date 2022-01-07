using System.Diagnostics.CodeAnalysis;
using System.Threading;
using NUnit.Framework;
using Sammlung.Werkzeug.Concurrent;

namespace Fixtures.Sammlung.Werkzeug
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EnhancedReaderWriterLockTests
    {
        [Test]
        public void UseReadLock_NoRecursion()
        {
            using var rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
            using (rwLock.UseReadLock())
            {
                Assert.Throws<LockRecursionException>(() => rwLock.UseReadLock());
                Assert.Throws<LockRecursionException>(() => rwLock.UseWriteLock());
                Assert.Throws<LockRecursionException>(() => rwLock.UseUpgradableReadLock());
            }

            using (rwLock.UseReadLock()) { }
        }
        
        [Test]
        public void UseWriteLock_NoRecursion()
        {
            using var rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
            using (rwLock.UseWriteLock())
            {
                Assert.Throws<LockRecursionException>(() => rwLock.UseReadLock());
                Assert.Throws<LockRecursionException>(() => rwLock.UseWriteLock());
                Assert.Throws<LockRecursionException>(() => rwLock.UseUpgradableReadLock());
            }

            using (rwLock.UseWriteLock()) { }
        }
        
        [Test]
        public void UseUpgradableReadLock_NoRecursion()
        {
            using var rwLock = new EnhancedReaderWriterLock(LockRecursionPolicy.NoRecursion);
            using (var upgradeLock = rwLock.UseUpgradableReadLock())
            {
                upgradeLock.Upgrade();
            }

            using (rwLock.UseUpgradableReadLock()) { }
        }
    }
}