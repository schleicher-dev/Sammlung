using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Sammlung.DesignPatterns;

namespace Fixtures.Sammlung.DesignPatterns
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    public class ObjectPoolTests
    {
        [SetUp]
        public void SetUp()
        {
        }
        
        [Test]
        public void Get_NoError()
        {
            var objectPool = new ObjectPool(2);
            Assert.That(objectPool.Get(), Is.Not.Null);
        }
        
        [Test]
        public void GetReturn_NoError()
        {
            var objectPool = new ObjectPool(2);
            var item = objectPool.Get();
            var intermediate = item;
            Assert.DoesNotThrow(() => objectPool.Return(ref item));
            Assert.That(item, Is.Null);
            var nextItem = objectPool.Get();
            Assert.That(nextItem, Is.SameAs(intermediate));
        }

        [Test]
        public void MaxPoolSizeExceeded()
        {
            var objectPool = new ObjectPool(2);
            var item1 = objectPool.Get();
            var item2 = objectPool.Get();
            var item3 = objectPool.Get();

            var inter1 = item1;
            var inter2 = item2;
            
            objectPool.Return(ref item1);
            objectPool.Return(ref item2);
            objectPool.Return(ref item3);

            Assert.That(objectPool.Get(), Is.SameAs(inter2));
            Assert.That(objectPool.Get(), Is.SameAs(inter1));
        }

        [ExcludeFromCodeCoverage]
        private class ObjectPool : ObjectPoolBase<object>
        {
            public ObjectPool(int maxPoolSize) : base(maxPoolSize) { }
            protected override object CreateInstance() => new();

            protected override object ResetInstance(object instance) => instance;
        }
    }
}