using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _Fixtures.Sammlung.Extras;
using NUnit.Framework;
using Sammlung.Queues;
using Sammlung.Queues.Concurrent;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ConcurrencyDequeTests
    {
        public static readonly DequeConstructors<int>[] Buffers =
        {
            new DequeConstructors<int>("BlockingDeque", c => BlockingDeque.Wrap(new ArrayDeque<int>(c))),
            new DequeConstructors<int>("LockFreeLinkedDeque", c => new LockFreeLinkedDeque<int>())
        };

        [TestCaseSource(nameof(Buffers))]
        public void ConcurrentlyPush(DequeConstructors<int> constructors)
        {
            const int numItems = 5_000;
            var capCtor = constructors.Item1;
            var buffer = capCtor(1);
            Parallel.For(0, numItems / 2, i => buffer.PushLeft(i));
            Parallel.For(numItems / 2, numItems, i => buffer.PushRight(i));
            Assert.AreEqual(numItems, buffer.Count);

            var list = new List<int>(buffer.Count);
            while (0 < buffer.Count) list.Add(buffer.PopRight());
            CollectionAssert.AreEquivalent(Enumerable.Range(0, numItems), list);
        }

        [TestCaseSource(nameof(Buffers))]
        [Repeat(5)]
        public void ConcurrentlyPushPopWhateverOrder(DequeConstructors<int> constructors)
        {
            const int numItems = 100_000;
            var capCtor = constructors.Item1;
            var buffer = capCtor(1);

            var random = new Random(0);
            var bag = new ConcurrentBag<int>();
            for (var i = 0; i < numItems; i++)
                bag.Add(random.Next(0, 6));

            var numElements = 0;
            Parallel.For(0, bag.Count, i =>
            {
                if (!bag.TryTake(out var j)) return;
                switch (j)
                {
                    case 0:
                        buffer.PushLeft(i);
                        Interlocked.Increment(ref numElements);
                        break;
                    case 1:
                        buffer.PushRight(i);
                        Interlocked.Increment(ref numElements);
                        break;
                    case 2 when buffer.TryPopLeft(out _):
                        Interlocked.Decrement(ref numElements);
                        break;
                    case 3 when buffer.TryPopRight(out _):
                        Interlocked.Decrement(ref numElements);
                        break;
                    case 4:
                        buffer.TryPeekLeft(out _);
                        break;
                    case 5:
                        buffer.TryPeekRight(out _);
                        break;
                }
            });

            Assert.AreEqual(numElements, buffer.Count);
        }
    }
}