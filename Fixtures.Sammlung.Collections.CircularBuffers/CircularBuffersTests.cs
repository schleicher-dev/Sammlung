using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlung.Collections.CircularBuffers;

namespace Fixtures.Sammlung.Collections.CircularBuffers
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    public class CircularBuffersTests
    {
        private static void OffsetBufferBy(ICircularBuffer<byte> buffer, int offset)
        {
            var putItems = Enumerable.Range(0, offset).Select(i => (byte)i).ToArray();
            Assert.That(buffer.TryPut(putItems), Is.True);

            var takeItems = new byte[offset];
            Assert.That(buffer.TryTake(takeItems, 0, offset), Is.True);
            Assert.That(takeItems, Is.EqualTo(putItems).AsCollection);
        }
        
        [Test]
        public void StaticBufferTest([Values] bool blocking)
        {
            const byte offset = 5;
            const byte capacity = 20;
            
            ICircularBuffer<byte> buffer = new StaticCircularBuffer<byte>(capacity);
            buffer = !blocking ? buffer : buffer.Wrap();
            
            OffsetBufferBy(buffer, offset);
            Assert.That(buffer.Count, Is.EqualTo(0));
            
            for (byte i = 0; i < capacity + 5; ++i)
            {
                if (i < capacity)
                {
                    Assert.That(buffer.TryPut(i), Is.True);
                    continue;
                }

                Assert.That(buffer.TryPut(i), Is.False);
            }
            Assert.That(buffer.Count, Is.EqualTo(capacity));

            var items = new byte[capacity];
            Assert.That(buffer.TryTake(items, 0, capacity + 1), Is.False);
            Assert.That(buffer.TryTake(items, 0, capacity), Is.True);
            Assert.That(items, Is.EqualTo(Enumerable.Range(0, capacity).Select(i => (byte)i)).AsCollection);
        }

        [Test]
        public void DynamicBufferTest([Values] bool blocking)
        {
            const byte capacity = 12;
            ICircularBuffer<byte> buffer = new DynamicCircularBuffer<byte>(capacity);
            buffer = !blocking ? buffer : buffer.Wrap();
            Assert.That(buffer.Capacity, Is.EqualTo(16));
            
            OffsetBufferBy(buffer, 12);
            Assert.That(buffer.Count, Is.EqualTo(0));
            var residentItems = Enumerable.Range(0, 5).Select(i => (byte)i).ToArray();
            Assert.That(buffer.TryPut(residentItems), Is.True);

            var putItems = Enumerable.Range(5, 20).Select(i => (byte)i).ToArray();
            Assert.That(buffer.TryPut(putItems), Is.True);
            
            var takeItems = new byte[25];
            Assert.That(buffer.TryPeek(takeItems, 1, 25), Is.False);
            Assert.That(buffer.TryTake(takeItems, 1, 25), Is.False);

            Assert.That(buffer.TryPeek(takeItems, 0, 25), Is.True);
            Assert.That(takeItems, Is.EqualTo(Enumerable.Range(0, 25).Select(i => (byte)i)).AsCollection);
            Assert.That(buffer.TryTake(takeItems, 0, 25), Is.True);
            Assert.That(takeItems, Is.EqualTo(Enumerable.Range(0, 25).Select(i => (byte)i)).AsCollection);
        }
    }
}