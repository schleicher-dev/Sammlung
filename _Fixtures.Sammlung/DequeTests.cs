using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using _Fixtures.Sammlung.Extras;
using NUnit.Framework;
using Sammlung.Queues;
using Sammlung.Queues.Concurrent;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DequeTests
    {
        public static readonly DequeConstructors<int>[] Buffers =
        {
            new DequeConstructors<int>(c => new ArrayDeque<int>(c)),
            new DequeConstructors<int>(c => BlockingArrayDeque.Wrap(new ArrayDeque<int>(c)))
        };
        
        [TestCaseSource(nameof(Buffers))]
        public void Fill_And_Grow(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;

            var buffer = capCtor(1);
            Assert.IsFalse(buffer.IsReadOnly);
            buffer.Enqueue(1);
            buffer.Enqueue(2);
            buffer.Enqueue(3);
            buffer.Enqueue(4);
            buffer.Enqueue(5);
            Assert.AreEqual(Enumerable.Range(1, 5), buffer);
            Assert.IsTrue(buffer.Contains(1));
            Assert.IsTrue(buffer.Contains(2));
            Assert.IsTrue(buffer.Contains(3));
            Assert.IsTrue(buffer.Contains(4));
            Assert.IsTrue(buffer.Contains(5));
            Assert.IsFalse(buffer.Contains(0));
        }

        [TestCaseSource(nameof(Buffers))]
        public void Enqueue_SunnyPath(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;

            var buffer = capCtor(1);
            foreach (var i in Enumerable.Range(1, 100))
            {
                buffer.Enqueue(i);
                CollectionAssert.AreEqual(Enumerable.Range(1, i), buffer);
            }
        }

        [TestCaseSource(nameof(Buffers))]
        public void InverseEnqueue_SunnyPath(DequeConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;

            var buffer = capCtor(1);
            foreach (var i in Enumerable.Range(1, 100))
            {
                buffer.InverseEnqueue(i);
                CollectionAssert.AreEqual(Enumerable.Range(1, i).Reverse(), buffer);
            }
        }

        [TestCaseSource(nameof(Buffers))]
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
                    buffer.Enqueue(items.Current);
                for (var i = 0; i < popFront; ++i)
                    list.Add(buffer.InverseDequeue());
                for (var i = 0; i < popBack; ++i)
                    list.Add(buffer.Dequeue());
            }

            CollectionAssert.AreEqual(new[] {20, 23}, buffer);
            CollectionAssert.AreEqual(
                new[] {4, 1, 6, 2, 10, 9, 3, 5, 18, 17, 16, 15, 7, 8, 11, 12, 22, 21, 13, 14, 24, 19, 25}, list);
            buffer.Clear();
            CollectionAssert.IsEmpty(buffer);
        }
        
        [TestCaseSource(nameof(Buffers))]
        public void EmptyBufferTests(DequeConstructors<int> constructors) {
            var capCtor = constructors.Item1;
            var buffer = capCtor(1);
            Assert.IsFalse(buffer.TryDequeue(out _));
            Assert.Throws<InvalidOperationException>(() => buffer.Dequeue());
            Assert.IsFalse(buffer.TryInverseDequeue(out _));
            Assert.Throws<InvalidOperationException>(() => buffer.InverseDequeue());
            CollectionAssert.IsEmpty(buffer);
        }
        
        [TestCaseSource(nameof(Buffers))]
        public void CopyToWrongLocationInArray(DequeConstructors<int> constructors) {
            var capCtor = constructors.Item1;
            var buffer = capCtor(1);
            buffer.Enqueue(0);
            var array = new int[buffer.Count];
            Assert.Throws<ArgumentException>(() => buffer.CopyTo(array, 1));
            buffer.CopyTo(array, 0);
            CollectionAssert.AreEqual(buffer, array);
        }


        [TestCaseSource(nameof(Buffers))]
        public void BufferDoesNotSupportAddAndRemove(DequeConstructors<int> constructors) { 
            var capCtor = constructors.Item1;
            var buffer = capCtor(1);
            Assert.Throws<NotSupportedException>(() => buffer.Add(0));
            Assert.Throws<NotSupportedException>(() => buffer.Remove(0));
        }
    }
}