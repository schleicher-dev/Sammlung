using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Sammlung.DesignPatterns;

namespace Fixtures.Sammlung.DesignPatterns
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ObjectPoolTests
    {
        private ObjectPool _objectPool;

        [SetUp]
        public void SetUp()
        {
            _objectPool = new ObjectPool(2);
        }
        
        [Test]
        public void Get_NoError()
        {
            Assert.IsNotNull(_objectPool.Get());
        }
        
        [Test]
        public void GetReturn_NoError()
        {
            var item = _objectPool.Get();
            var intermediate = item;
            Assert.DoesNotThrow(() => _objectPool.Return(ref item));
            Assert.IsNull(item);
            var nextItem = _objectPool.Get();
            Assert.AreSame(intermediate, nextItem);
        }

        [Test]
        public void MaxPoolSizeExceeded()
        {
            var item1 = _objectPool.Get();
            var item2 = _objectPool.Get();
            var item3 = _objectPool.Get();

            var inter1 = item1;
            var inter2 = item2;
            
            _objectPool.Return(ref item1);
            _objectPool.Return(ref item2);
            _objectPool.Return(ref item3);
            
            Assert.AreSame(inter2, _objectPool.Get());
            Assert.AreSame(inter1, _objectPool.Get());
        }

        [ExcludeFromCodeCoverage]
        private class ObjectPool : ObjectPoolBase<object>
        {
            public ObjectPool(int maxPoolSize) : base(maxPoolSize) { }
            protected override object CreateInstance() => new object();

            protected override object ResetInstance(object instance) => instance;
        }
    }
}