using System.Linq;
using NUnit.Framework;
using Sammlung.Collections.CircularBuffers;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CircularBuffersTests
    {
        private static void OffsetBufferBy(ICircularBuffer<byte> buffer, int offset)
        {
            var putItems = Enumerable.Range(0, offset).Select(i => (byte)i).ToArray();
            Assert.IsTrue(buffer.TryPut(putItems));

            var takeItems = new byte[offset];
            Assert.IsTrue(buffer.TryTake(takeItems, 0, offset));
            CollectionAssert.AreEqual(putItems, takeItems);
        }
        
        [Test]
        public void StaticBufferTest([Values] bool blocking)
        {
            const byte offset = 5;
            const byte capacity = 20;
            
            ICircularBuffer<byte> buffer = new StaticCircularBuffer<byte>(capacity);
            buffer = !blocking ? buffer : buffer.Wrap();
            
            OffsetBufferBy(buffer, offset);
            Assert.AreEqual(0, buffer.Count);
            
            for (byte i = 0; i < capacity + 5; ++i)
            {
                if (i < capacity)
                {
                    Assert.IsTrue(buffer.TryPut(i));
                    continue;
                }
                
                Assert.IsFalse(buffer.TryPut(i));
            }
            Assert.AreEqual(capacity, buffer.Count);

            var items = new byte[capacity];
            Assert.IsFalse(buffer.TryTake(items, 0, capacity + 1));
            Assert.IsTrue(buffer.TryTake(items, 0, capacity));
            CollectionAssert.AreEqual(Enumerable.Range(0, capacity).Select(i => (byte)i), items);
        }

        [Test]
        public void DynamicBufferTest([Values] bool blocking)
        {
            const byte capacity = 12;
            ICircularBuffer<byte> buffer = new DynamicCircularBuffer<byte>(capacity);
            buffer = !blocking ? buffer : buffer.Wrap();
            Assert.AreEqual(16, buffer.Capacity);
            
            OffsetBufferBy(buffer, 12);
            Assert.AreEqual(0, buffer.Count);
            var residentItems = Enumerable.Range(0, 5).Select(i => (byte)i).ToArray();
            Assert.IsTrue(buffer.TryPut(residentItems));

            var putItems = Enumerable.Range(5, 20).Select(i => (byte)i).ToArray();
            Assert.IsTrue(buffer.TryPut(putItems));
            
            var takeItems = new byte[25];
            Assert.IsFalse(buffer.TryPeek(takeItems, 1, 25));
            Assert.IsFalse(buffer.TryTake(takeItems, 1, 25));
            
            Assert.IsTrue(buffer.TryPeek(takeItems, 0, 25));
            CollectionAssert.AreEqual(Enumerable.Range(0, 25).Select(i => (byte)i), takeItems);
            Assert.IsTrue(buffer.TryTake(takeItems, 0, 25));
            CollectionAssert.AreEqual(Enumerable.Range(0, 25).Select(i => (byte)i), takeItems);
        }
    }
}