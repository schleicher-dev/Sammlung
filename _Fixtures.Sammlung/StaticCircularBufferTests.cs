using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using _Fixtures.Sammlung.Extras;
using NUnit.Framework;
using Sammlung;
using Sammlung.Deques;
using Sammlung.Deques.Concurrent;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class StaticCircularBufferTests
    {
        public static readonly CircularBufferConstructors<int>[] Buffers =
        {
            new CircularBufferConstructors<int>(c => new StaticContinuousDeque<int>(c)),
            new CircularBufferConstructors<int>(c => new ConcurrentStaticContinuousDeque<int>(c)), 
        };
        
        [TestCaseSource(nameof(Buffers))]
        public void Fill_CheckContent(CircularBufferConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(10);
            foreach (var i in Enumerable.Range(1, 11))
                buffer.PushBack(i);
            
            CollectionAssert.AreEqual(Enumerable.Range(2, 10), buffer);
        }
        
        [TestCaseSource(nameof(Buffers))]
        public void FillAlternating_Match(CircularBufferConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(11);
            foreach (var i in Enumerable.Range(1, 20))
            {
                var pushFunc = i % 2 == 0 ?  buffer.PushBack : (Action<int>) buffer.PushFront;
                pushFunc(i);
            }
            
            CollectionAssert.AreEqual(new [] {9, 7, 5, 3, 1, 2, 4, 6, 8, 10, 20}, buffer);
        }
        
        [TestCaseSource(nameof(Buffers))]
        public void Fill_PopAndPushSequences(CircularBufferConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(11);
            foreach (var i in Enumerable.Range(1, 11))
            {
                var pushFunc = i % 2 == 0 ?  buffer.PushBack : (Action<int>) buffer.PushFront;
                pushFunc(i);
            }

            foreach (var i in Enumerable.Range(1, 11).Reverse())
            {
                var popFunc = i % 2 == 0 ?  buffer.PopBack : (Func<int>) buffer.PopFront;
                Assert.AreEqual(i, popFunc());
            }
            
            Assert.IsEmpty(buffer);
        }

        [TestCaseSource(nameof(Buffers))]
        public void Fill_PushAndPopBack(CircularBufferConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(10);
            Assert.AreEqual(10, buffer.Capacity);
            foreach (var i in Enumerable.Range(1, 100)) buffer.PushBack(i);
            Assert.AreEqual(10, buffer.Count);
            foreach (var i in Enumerable.Range(91, 10).Reverse().Take(10))
                Assert.AreEqual(i, buffer.PopBack());
            Assert.IsEmpty(buffer);
        }

        [TestCaseSource(nameof(Buffers))]
        public void Fill_PushAndPopFront(CircularBufferConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(10);
            foreach (var i in Enumerable.Range(1, 100)) buffer.PushFront(i);
            foreach (var i in Enumerable.Range(91, 10).Reverse().Take(10))
                Assert.AreEqual(i, buffer.PopFront());
            Assert.IsEmpty(buffer);
        }

        [TestCaseSource(nameof(Buffers))]
        public void Pop_EmptyBuffer(CircularBufferConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(10);
            Assert.Throws<InvalidOperationException>(() => buffer.PopFront());
            Assert.Throws<InvalidOperationException>(() => buffer.PopBack());
            Assert.IsFalse(buffer.TryPopFront(out _));
            Assert.IsFalse(buffer.TryPopBack(out _));
        }

        [TestCaseSource(nameof(Buffers))]
        public void Enumerator_Reset(CircularBufferConstructors<int> constructors)
        {
            var capCtor = constructors.Item1;
            var buffer = capCtor(5);
            
            Assert.AreEqual(5, buffer.Capacity);
            buffer.PushBack(1);
            buffer.PushBack(2);
            buffer.PushBack(3);
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(5, buffer.Capacity);
            buffer.PushBack(4);
            buffer.PushBack(5);
            Assert.AreEqual(5, buffer.Count);
            buffer.PushBack(6);
            Assert.AreEqual(5, buffer.Count);
            Assert.AreEqual(5, buffer.Capacity);

            using var enumerator = buffer.GetEnumerator();
            {
                var list = new List<int>();
                while (enumerator.MoveNext())
                    list.Add(enumerator.Current);
                CollectionAssert.AreEqual(new [] {2, 3, 4, 5, 6}, list);
            }
                
            Assert.Throws<NotSupportedException>(() => enumerator.Reset());
        }
    }
}