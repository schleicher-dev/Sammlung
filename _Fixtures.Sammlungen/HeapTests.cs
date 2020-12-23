using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlungen.Collections;

namespace _Fixtures.Sammlungen
{
    [ExcludeFromCodeCoverage]
    public class HeapTests
    {
        [SetUp]
        public void Setup() { }

        private static IEnumerable<string> GetNames()
        {
            for (var c = 'A'; c <= 'Z'; ++c)
                yield return $"{c}";
            for (var c = 'A'; c <= 'Z'; ++c)
                foreach (var name in GetNames())
                    yield return $"{c}{name}";
        }

        [Test]
        public void PushAndPop_SunnyPath()
        {
            var heap = new BinaryHeap<int, string>();
            var items = Enumerable.Range(1, 10_000).Zip(GetNames()).ToList();
            foreach (var (key, value) in items.AsEnumerable().Reverse())
            {
                heap.Push(key, value);
            }

            var resultList = new List<string>();
            while(heap.Any()) resultList.Add(heap.Pop());
            CollectionAssert.AreEqual(items.Select(kv => kv.Second), resultList);
        }
        
        [Test]
        public void PushAndPop_Randomized_SunnyPath()
        {
            var random = new Random(0);
            var comparer = Comparer<int>.Default;
            var valueComparer = EqualityComparer<int>.Default;
            var heap = new BinaryHeap<int, int>(comparer, valueComparer);

            for (var i = 0; i < 10_000; ++i)
            {
                var k = random.Next(int.MinValue, int.MaxValue);
                heap.Push(k, k);
            }

            var last = heap.Replace(int.MaxValue, int.MaxValue);
            while (heap.Any())
            {
                var current = heap.Pop();
                Assert.LessOrEqual(comparer.Compare(last, current), 0);
                last = current;
            }
        }

        [Test]
        public void CheckAllCases_Of_InvalidOperationException()
        {
            var heap = new BinaryHeap<int, string>();
            Assert.IsFalse(heap.TryPop(out _));
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
            Assert.IsFalse(heap.TryPeek(out _));
            Assert.Throws<InvalidOperationException>(() => heap.Peek());
            Assert.IsFalse(heap.TryReplace(1, "A", out _));
            Assert.Throws<InvalidOperationException>(() => heap.Replace(1, "A"));
            Assert.IsFalse(heap.TryUpdate("A", 2));
            Assert.Throws<InvalidOperationException>(() => heap.Update("A", 2));
        }

        [Test]
        public void Update_SunnyPath()
        {
            var heap = new BinaryHeap<int, string>();
            heap.Push(100, "A");
            heap.Push(50, "B");
            heap.Push(150, "C");
            heap.Push(25, "D");
            heap.Push(200, "E");
            heap.Push(12, "F");
            
            Assert.AreEqual("F", heap.Peek());
            heap.Update("F", 500);
            Assert.AreEqual("D", heap.Peek());
            heap.Update("F", 12);
            Assert.AreEqual("F", heap.Peek());
            heap.Update("F", 500);

            var list = new List<string>();
            while (heap.Any())
            {
                list.Add(heap.Pop());
            }
            
            CollectionAssert.AreEqual(new [] {"D", "B", "A", "C", "E", "F"}, list);
        }

        [Test]
        public void FromEnumerableToHeap()
        {
            var random = new Random(0);

            var list = new List<KeyValuePair<int, int>>();
            for (var i = 0; i < 10_000; ++i)
            {
                var k = random.Next(int.MinValue, int.MaxValue);
                list.Add(KeyValuePair.Create(k, k));
            }

            var comparerA = Comparer<int>.Default;
            var heapA = new BinaryHeap<int, int>(list);
            
            var comparerB = Comparer<int>.Create((a, b) => Comparer<int>.Default.Compare(b, a));
            var valueComparerB = EqualityComparer<int>.Default;
            var heapB = new BinaryHeap<int, int>(list, comparerB, valueComparerB);

            var listA = new List<int>();
            var listB = new List<int>();
            while (heapA.Any() && heapB.Any())
            {
                listA.Add(heapA.Pop());
                listB.Add(heapB.Pop());
            }
            CollectionAssert.IsOrdered(listA, comparerA);
            CollectionAssert.IsOrdered(listB, comparerB);
        }
    }
}