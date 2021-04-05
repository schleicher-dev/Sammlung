using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sammlung.Heaps;

namespace _Fixtures.Sammlung
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
            var heap = new BinaryHeap<string, int>();
            var items = Enumerable.Range(1, 10_000).Zip(GetNames(), Tuple.Create).ToList();
            foreach (var (priority, value) in items.AsEnumerable().Reverse())
            {
                heap.Push(value, priority);
            }

            var resultList = new List<string>();
            while(!heap.IsEmpty) resultList.Add(heap.Pop().Value);
            CollectionAssert.AreEqual(items.Select(kv => kv.Item2), resultList);
        }
        
        [Test]
        public void PushAndPop_Randomized_SunnyPath()
        {
            var random = new Random(0);
            var heap = new BinaryHeap<int, int>();

            for (var i = 0; i < 10_000; ++i)
            {
                var k = random.Next(int.MinValue, int.MaxValue);
                heap.Push(k, k);
            }

            var last = heap.Pop();
            while (!heap.IsEmpty)
            {
                var current = heap.Pop();
                Assert.LessOrEqual(last.Value.CompareTo(current.Value), 0);
                last = current;
            }
        }

        [Test]
        public void CheckAllCases_Of_InvalidOperationException()
        {
            var heap = new BinaryHeap<string, int>();
            Assert.IsTrue(heap.IsEmpty);
            Assert.IsFalse(heap.TryPop(out _));
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
            Assert.IsFalse(heap.TryPeek(out _));
            Assert.Throws<InvalidOperationException>(() => heap.Peek());
            Assert.IsFalse(heap.TryUpdate("A", 0));
            Assert.Throws<InvalidOperationException>(() => heap.Update("A", 0));
            Assert.IsFalse(heap.TryReplace("A", 0, out _));
            Assert.Throws<InvalidOperationException>(() => heap.Replace("A", 0));
        }

        [Test]
        public void UpdateReplace_SunnyPath()
        {
            var heap = new BinaryHeap<string, int>();
            heap.Push("A", 100);
            heap.Push("B", 50);
            heap.Push("C", 150);
            heap.Push("D", 25);
            heap.Push("E", 200);

            heap.Push("F", 12);
            heap.Update("D", 400);

            heap.Replace("F", 500);
            
            Assert.AreEqual("B", heap.Peek().Value);

            var list = new List<string>();
            while (!heap.IsEmpty)
            {
                list.Add(heap.Pop().Value);
            }
            
            CollectionAssert.AreEqual(new [] {"B", "A", "C", "E", "D", "F"}, list);
        }

        [Test]
        public void FromEnumerableToHeap()
        {
            var random = new Random(0);

            var list = new List<KeyValuePair<int, int>>();
            for (var i = 0; i < 10_000; ++i)
            {
                var k = random.Next(int.MinValue, int.MaxValue);
                list.Add(new KeyValuePair<int, int>(k, k));
            }

            var comparerA = Comparer<int>.Default;
            var heapA = new BinaryHeap<int, int>(list);
            
            var listA = new List<int>();
            while (!heapA.IsEmpty)
            {
                listA.Add(heapA.Pop().Value);
            }
            CollectionAssert.IsOrdered(listA, comparerA);
        }

        [Test]
        public void EnumerateHeap()
        {
            var heap = new BinaryHeap<int, int>();
            heap.Push(1, 200);
            heap.Push(2, 300);
            heap.Push(3, 400);
            heap.Push(4, 100);
            
            CollectionAssert.AreEquivalent(new[] {1, 2, 3, 4}, heap.Select(v => v.Value).ToArray());
            CollectionAssert.AreEquivalent(new[] {100, 200, 300, 400}, heap.Select(v => v.Priority).ToArray());

            var result = new List<int>();
            var enumerator = ((System.Collections.IEnumerable) heap).GetEnumerator();
            // ReSharper disable once PossibleNullReferenceException
            while (enumerator.MoveNext()) result.Add(((HeapPair<int, int>) enumerator.Current).Value);
            CollectionAssert.AreEquivalent(new[] {1, 2, 3, 4}, result);
        }
    }
}