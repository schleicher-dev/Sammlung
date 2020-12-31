using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Fixtures.Sammlung.Extras;
using NUnit.Framework;
using Sammlung.Queues;
using Sammlung.Queues.Concurrent;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    public class ConcurrencyDequeTests
    {
        public static readonly DequeConstructors<int>[] Buffers =
        {
            new DequeConstructors<int>(c => BlockingDeque.Wrap(new ArrayDeque<int>(c))),
            new DequeConstructors<int>(c => new LockFreeDeque<int>())
        };

        [TestCaseSource(nameof(Buffers))]
        public void ConcurrentlyPush(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(1);
            Parallel.For(0, 1_000, new ParallelOptions {MaxDegreeOfParallelism = 4}, i => buffer.PushLeft(i));
            Parallel.For(1_000, 2_000, new ParallelOptions {MaxDegreeOfParallelism = 4}, i => buffer.PushRight(i));
            Assert.AreEqual(2_000, buffer.Count);

            var list = new List<int>(buffer.Count);
            while(0 < buffer.Count) list.Add(buffer.PopRight());
            CollectionAssert.AreEquivalent(Enumerable.Range(0, 2_000), list);
        }
    }
}