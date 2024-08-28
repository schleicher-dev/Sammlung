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
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    public class DequeTests
    {
        public static readonly DequeConstructors<int>[] Buffers =
        {
            new("ArrayDeque", c => new ArrayDeque<int>(c)),
            new("BlockingDeque", c => new ArrayDeque<int>(c).ToBlockingDeque()),
            new("LinkedDeque", c => new LinkedDeque<int>()),
            new("LockFreeLinkedDeque", c => new LockFreeLinkedDeque<int>())
        };
        public static readonly DequeConstructors<int>[] NormalBuffers =
        {
            new("ArrayDeque", c => new ArrayDeque<int>(c)),
            new("BlockingDeque", c => new ArrayDeque<int>(c).ToBlockingDeque()),
            new("LinkedDeque", c => new LinkedDeque<int>())
        };

        [TestCaseSource(nameof(Buffers))]
        [Repeat(5)]
        public void Fill_And_Grow(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;

            var buffer = capCtor(1);
            buffer.PushLeft(1);
            Assert.That(buffer.Count, Is.EqualTo(1));
            buffer.PushLeft(2);
            Assert.That(buffer.Count, Is.EqualTo(2));
            buffer.PushLeft(3);
            Assert.That(buffer.Count, Is.EqualTo(3));
            buffer.PushLeft(4);
            Assert.That(buffer.Count, Is.EqualTo(4));
            buffer.PushLeft(5);
            Assert.That(buffer.Count, Is.EqualTo(5));
            Assert.That(buffer.PopRight(), Is.EqualTo(1));
            Assert.That(buffer.Count, Is.EqualTo(4));
            Assert.That(buffer.PopRight(), Is.EqualTo(2));
            Assert.That(buffer.Count, Is.EqualTo(3));
            Assert.That(buffer.PopRight(), Is.EqualTo(3));
            Assert.That(buffer.Count, Is.EqualTo(2));
            Assert.That(buffer.PopRight(), Is.EqualTo(4));
            Assert.That(buffer.Count, Is.EqualTo(1));
            Assert.That(buffer.PopRight(), Is.EqualTo(5));
            Assert.That(buffer.Count, Is.EqualTo(0));
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
                Assert.That(buffer.PeekRight(), Is.EqualTo(i));
                Assert.That(buffer.PeekLeft(), Is.EqualTo(1));
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
                Assert.That(buffer.PeekLeft(), Is.EqualTo(i));
                Assert.That(buffer.PeekRight(), Is.EqualTo(1));
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

            Assert.That(buffer.PopRight(), Is.EqualTo(15));
            Assert.That(buffer.PopRight(), Is.EqualTo(16));

            var expected = new[]
            {
                1, 6, 2, 10, 3, 4, 18, 17, 5, 7, 8, 9, 22, 21, 20, 19, 11, 12, 24, 23, 13, 25, 14
            };
            Assert.That(list, Is.EqualTo(expected).AsCollection);
        }

        [TestCaseSource(nameof(Buffers))]
        [Repeat(5)]
        public void EmptyBufferTests(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(1);
            Assert.That(buffer.TryPopRight(out _), Is.False);
            Assert.Throws<InvalidOperationException>(() => buffer.PopRight());
            Assert.That(buffer.TryPopLeft(out _), Is.False);
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

            Assert.That(deque, Is.Empty);
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

            Assert.That(deque.ToArray(), Is.EqualTo(new[] { 4, 2, 0, 1, 3, 5 }).AsCollection);
        }

        [TestCaseSource(nameof(NormalBuffers))]
        [Repeat(5)]
        public void Deque_ContiguousEnumeration(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var deque = capCtor.Invoke(6);

            for (var i = 0; i < 5; i++) deque.PushRight(i);
            deque.PopLeft();

            Assert.That(deque.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }).AsCollection);

            var result = new List<int>();
            var enumerator = ((System.Collections.IEnumerable) deque).GetEnumerator();
            // ReSharper disable once PossibleNullReferenceException
            while (enumerator.MoveNext()) result.Add((int) enumerator.Current);
            Assert.That(result, Is.EqualTo(new[] { 1, 2, 3, 4 }).AsCollection);
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

            Assert.That(deque, Is.EqualTo(array).AsCollection);
        }
        
        [Test]
        public void ArrayDeque_RTL_Construction()
        {
            var array = Enumerable.Range(0, 10).ToArray();
            var deque = new ArrayDeque<int>(array, ConstructionDirection.RightToLeft);

            Assert.That(deque, Is.EqualTo(array.Reverse()).AsCollection);
        }
        
        [Test]
        public void ArrayDeque_Strange_ConstructionDirection_ThrowsArgumentOutOfRange()
        {
            var array = Enumerable.Range(0, 10).ToArray();
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new ArrayDeque<int>(array, (ConstructionDirection)(-1)));
        }
    }
}