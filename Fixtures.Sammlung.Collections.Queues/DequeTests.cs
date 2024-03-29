using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Fixtures.Sammlung.Collections.Queues.Extras;
using NUnit.Framework;
using Sammlung.Collections.Queues;
using Sammlung.Collections.Queues.Concurrent;

namespace Fixtures.Sammlung.Collections.Queues
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DequeTests
    {
        public static readonly DequeConstructors<int>[] Buffers =
        {
            new DequeConstructors<int>("ArrayDeque", c => new ArrayDeque<int>(c)),
            new DequeConstructors<int>("BlockingDeque", c => new ArrayDeque<int>(c).ToBlockingDeque()),
            new DequeConstructors<int>("LinkedDeque", c => new LinkedDeque<int>()),
            new DequeConstructors<int>("LockFreeLinkedDeque", c => new LockFreeLinkedDeque<int>())
        };
        public static readonly DequeConstructors<int>[] NormalBuffers =
        {
            new DequeConstructors<int>("ArrayDeque", c => new ArrayDeque<int>(c)),
            new DequeConstructors<int>("BlockingDeque", c => new ArrayDeque<int>(c).ToBlockingDeque()),
            new DequeConstructors<int>("LinkedDeque", c => new LinkedDeque<int>())
        };

        [TestCaseSource(nameof(Buffers))]
        [Repeat(5)]
        public void Fill_And_Grow(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;

            var buffer = capCtor(1);
            buffer.PushLeft(1);
            Assert.AreEqual(1, buffer.Count);
            buffer.PushLeft(2);
            Assert.AreEqual(2, buffer.Count);
            buffer.PushLeft(3);
            Assert.AreEqual(3, buffer.Count);
            buffer.PushLeft(4);
            Assert.AreEqual(4, buffer.Count);
            buffer.PushLeft(5);
            Assert.AreEqual(5, buffer.Count);
            Assert.AreEqual(1, buffer.PopRight());
            Assert.AreEqual(4, buffer.Count);
            Assert.AreEqual(2, buffer.PopRight());
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(3, buffer.PopRight());
            Assert.AreEqual(2, buffer.Count);
            Assert.AreEqual(4, buffer.PopRight());
            Assert.AreEqual(1, buffer.Count);
            Assert.AreEqual(5, buffer.PopRight());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestCaseSource(nameof(Buffers))]
        [Repeat(5)]
        public void Enqueue_SunnyPath(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;

            var buffer = capCtor(1);
            foreach (var i in Enumerable.Range(1, 100))
            {
                buffer.PushRight(i);
                Assert.AreEqual(i, buffer.PeekRight());
                Assert.AreEqual(1, buffer.PeekLeft());
            }
        }

        [TestCaseSource(nameof(Buffers))]
        [Repeat(5)]
        public void InverseEnqueue_SunnyPath(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;

            var buffer = capCtor(1);
            foreach (var i in Enumerable.Range(1, 100))
            {
                buffer.PushLeft(i);
                Assert.AreEqual(i, buffer.PeekLeft());
                Assert.AreEqual(1, buffer.PeekRight());
            }
        }

        [TestCaseSource(nameof(Buffers))]
        [Repeat(5)]
        public void SequencesOfPopsAndPushes(DequeConstructors<int> constructors)
        {
            var sequences = new[]
            {
                (4, 0, 1),
                (2, 1, 1),
                (4, 1, 2),
                (8, 2, 4),
                (4, 4, 2),
                (2, 2, 1),
                (1, 1, 1)
            };

            using var items = Enumerable.Range(1, 100).GetEnumerator();

            var capCtor = constructors.Item1;
            var buffer = capCtor(1);
            var list = new List<int>();
            foreach (var (push, popFront, popBack) in sequences)
            {
                for (var i = 0; i < push && items.MoveNext(); ++i)
                    buffer.PushLeft(items.Current);
                for (var i = 0; i < popFront; ++i)
                    list.Add(buffer.PopLeft());
                for (var i = 0; i < popBack; ++i)
                    list.Add(buffer.PopRight());
            }

            Assert.AreEqual(15, buffer.PopRight());
            Assert.AreEqual(16, buffer.PopRight());

            var expected = new[]
            {
                1, 6, 2, 10, 3, 4, 18, 17, 5, 7, 8, 9, 22, 21, 20, 19, 11, 12, 24, 23, 13, 25, 14
            };
            CollectionAssert.AreEqual(expected, list);
        }

        [TestCaseSource(nameof(Buffers))]
        [Repeat(5)]
        public void EmptyBufferTests(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(1);
            Assert.IsFalse(buffer.TryPopRight(out _));
            Assert.Throws<InvalidOperationException>(() => buffer.PopRight());
            Assert.IsFalse(buffer.TryPopLeft(out _));
            Assert.Throws<InvalidOperationException>(() => buffer.PopLeft());
        }

        [TestCaseSource(nameof(NormalBuffers))]
        public void EmptyDequeEnumeration(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var deque = capCtor.Invoke(6);
            
            for(var i = 0; i < 6; ++i)
                deque.PushLeft(i);
            for(var i = 0; i < 6; ++i)
                deque.PopLeft();
            
            CollectionAssert.IsEmpty(deque);
        }

        [TestCaseSource(nameof(NormalBuffers))]
        [Repeat(5)]
        public void Deque_Enumeration(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var deque = capCtor.Invoke(6);

            for (var i = 0; i < 6; i++)
            {
                if (i % 2 == 0)
                {
                    deque.PushLeft(i);
                    continue;
                }
                
                deque.PushRight(i);
            }
            
            CollectionAssert.AreEqual(new[] {4, 2, 0, 1, 3, 5}, deque.ToArray());
        }

        [TestCaseSource(nameof(NormalBuffers))]
        [Repeat(5)]
        public void Deque_ContiguousEnumeration(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var deque = capCtor.Invoke(6);

            for (var i = 0; i < 5; i++) deque.PushRight(i);
            deque.PopLeft();

            CollectionAssert.AreEqual(new[] {1, 2, 3, 4}, deque.ToArray());

            var result = new List<int>();
            var enumerator = ((System.Collections.IEnumerable) deque).GetEnumerator();
            // ReSharper disable once PossibleNullReferenceException
            while (enumerator.MoveNext()) result.Add((int) enumerator.Current);
            CollectionAssert.AreEqual(new[] {1, 2, 3, 4}, result);
        }

        [Test]
        public void LockFreeDeque_DoesNotSupportEnumeration()
        {
            var lfDeque = new LockFreeLinkedDeque<int>();
            lfDeque.PushRight(1);
            lfDeque.PushRight(2);
            lfDeque.PushRight(3);

            Assert.Throws<NotSupportedException>(() => _ = lfDeque.ToArray());
            Assert.Throws<NotSupportedException>(() => ((IEnumerable) lfDeque).GetEnumerator());
        }

        [Test]
        public void ArrayDeque_LTR_Construction()
        {
            var array = Enumerable.Range(0, 10).ToArray();
            var deque = new ArrayDeque<int>(array, ConstructionDirection.LeftToRight);
            
            CollectionAssert.AreEqual(array, deque);
        }
        
        [Test]
        public void ArrayDeque_RTL_Construction()
        {
            var array = Enumerable.Range(0, 10).ToArray();
            var deque = new ArrayDeque<int>(array, ConstructionDirection.RightToLeft);
            
            CollectionAssert.AreEqual(array.Reverse(), deque);
        }
        
        [Test]
        public void ArrayDeque_Strange_ConstructionDirection_ThrowsArgumentOutOfRange()
        {
            var array = Enumerable.Range(0, 10).ToArray();
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new ArrayDeque<int>(array, (ConstructionDirection)(-1)));
        }
    }
}