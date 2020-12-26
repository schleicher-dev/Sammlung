using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlung;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class StaticCircularBufferTests
    {
        [Test]
        public void Fill_CheckContent()
        {
            var buffer = new StaticCircularBuffer<int>(10);
            foreach (var i in Enumerable.Range(1, 11))
                buffer.PushBack(i);
            
            CollectionAssert.AreEqual(Enumerable.Range(2, 10), buffer);
        }
        
        
        [Test]
        public void FillAlternating_Match()
        {
            var buffer = new StaticCircularBuffer<int>(11);
            foreach (var i in Enumerable.Range(1, 20))
            {
                var pushFunc = i % 2 == 0 ?  buffer.PushBack : (Action<int>) buffer.PushFront;
                pushFunc(i);
            }
            
            CollectionAssert.AreEqual(new [] {9, 7, 5, 3, 1, 2, 4, 6, 8, 10, 20}, buffer);
        }
        
        
        [Test]
        public void Fill_PopAndPushSequences()
        {
            var buffer = new StaticCircularBuffer<int>(11);
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

        [Test]
        public void Fill_PushAndPopBack()
        {
            var buffer = new StaticCircularBuffer<int>(10);
            foreach (var i in Enumerable.Range(1, 100)) buffer.PushBack(i);
            foreach (var i in Enumerable.Range(91, 10).Reverse().Take(10))
                Assert.AreEqual(i, buffer.PopBack());
            Assert.IsEmpty(buffer);
        }

        [Test]
        public void Fill_PushAndPopFront()
        {
            var buffer = new StaticCircularBuffer<int>(10);
            foreach (var i in Enumerable.Range(1, 100)) buffer.PushFront(i);
            foreach (var i in Enumerable.Range(91, 10).Reverse().Take(10))
                Assert.AreEqual(i, buffer.PopFront());
            Assert.IsEmpty(buffer);
        }

        [Test]
        public void Pop_EmptyBuffer()
        {
            var buffer = new StaticCircularBuffer<int>(10);
            Assert.Throws<InvalidOperationException>(() => buffer.PopFront());
            Assert.Throws<InvalidOperationException>(() => buffer.PopBack());
            Assert.IsFalse(buffer.TryPopFront(out _));
            Assert.IsFalse(buffer.TryPopBack(out _));
        }

        [Test]
        public void Enumerator_Reset()
        {
            var buffer = new StaticCircularBuffer<int>(5);
            buffer.PushBack(1);
            buffer.PushBack(2);
            buffer.PushBack(3);
            buffer.PushBack(4);
            buffer.PushBack(5);
            buffer.PushBack(6);

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