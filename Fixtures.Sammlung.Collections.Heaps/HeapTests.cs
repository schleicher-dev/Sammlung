using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlung.Collections.Heaps;

namespace Fixtures.Sammlung.Collections.Heaps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    public class HeapTests
    {
        public enum HeapType
        {
            BinaryHeap,
        }

        public enum HeapBehaviour
        {
            NonBlocking,
            Blocking
        }

        private static IHeap<T1, T2> CreateHeap<T1, T2>(HeapType type, HeapBehaviour behaviour) where T2 : IComparable<T2>
        {
            IHeap<T1, T2> heap = type switch
            {
                HeapType.BinaryHeap => new BinaryHeap<T1, T2>(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return behaviour switch
            {
                HeapBehaviour.NonBlocking => heap,
                HeapBehaviour.Blocking => heap.Wrap(),
                _ => throw new ArgumentOutOfRangeException(nameof(behaviour), behaviour, null)
            };
        }

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
        public void PushAndPop_SunnyPath([Values] HeapType type, [Values] HeapBehaviour behaviour)
        {
            var heap = CreateHeap<string, int>(type, behaviour);
            var items = Enumerable.Range(1, 10_000).Zip(GetNames(), Tuple.Create).ToList();
            foreach (var (priority, value) in items.AsEnumerable().Reverse())
            {
                heap.Push(value, priority);
            }

            var resultList = new List<string>();
            while (!heap.IsEmpty) resultList.Add(heap.Pop().Value);
            Assert.That(resultList, Is.EqualTo(items.Select(kv => kv.Item2)).AsCollection);
        }

        [Test]
        public void PushAndPop_Randomized_SunnyPath([Values] HeapType type, [Values] HeapBehaviour behaviour)
        {
            var heap = CreateHeap<int, int>(type, behaviour);
            var random = new Random(0);

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
        public void CheckAllCases_Of_InvalidOperationException([Values] HeapType type, [Values] HeapBehaviour behaviour)
        {
            var heap = CreateHeap<string, int>(type, behaviour);
            Assert.That(heap.Count, Is.EqualTo(0));
            Assert.That(heap.IsEmpty, Is.True);
            Assert.That(heap.TryPop(out _), Is.False);
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
            Assert.That(heap.TryPeek(out _), Is.False);
            Assert.Throws<InvalidOperationException>(() => heap.Peek());
            Assert.That(heap.TryUpdate(HeapPair.Create("A", 100), 0), Is.False);
            Assert.Throws<ArgumentException>(() => heap.Update(HeapPair.Create("A", 100), 0));
            Assert.That(heap.TryReplace("A", 0, out _), Is.False);
            Assert.Throws<ArgumentException>(() => heap.Replace("A", 0));
        }

        [Test]
        public void UpdateReplace_SunnyPath([Values] HeapType type, [Values] HeapBehaviour behaviour)
        {
            var heap = CreateHeap<string, int>(type, behaviour);
            heap.Push("A", 100);
            heap.Push("B", 50);
            heap.Push("C", 150);
            heap.Push("D", 25);
            heap.Push("D", 105);
            heap.Push("E", 200);

            heap.Push("F", 12);
            heap.Update(HeapPair.Create("D", 25), 400);
            heap.Update(HeapPair.Create("D", 105), 1000);

            heap.Replace("F", 500);

            Assert.That(heap.Peek().Value, Is.EqualTo("B"));

            var list = new List<string>();
            while (!heap.IsEmpty)
            {
                list.Add(heap.Pop().Value);
            }

            Assert.That(list, Is.EqualTo(new[] { "B", "A", "C", "E", "D", "F", "D" }).AsCollection);
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
            Assert.That(listA, Is.Ordered.Using((IComparer<int>)comparerA));
        }

        [Test]
        public void EnumerateHeap([Values] HeapType type, [Values] HeapBehaviour behaviour)
        {
            var heap = CreateHeap<int, int>(type, behaviour);
            heap.Push(1, 200);
            heap.Push(2, 300);
            heap.Push(3, 400);
            heap.Push(4, 100);

            Assert.That(heap.Select(v => v.Value).ToArray(), Is.EquivalentTo(new[] { 1, 2, 3, 4 }));
            Assert.That(heap.Select(v => v.Priority).ToArray(), Is.EquivalentTo(new[] { 100, 200, 300, 400 }));

            var result = new List<int>();
            var enumerator = ((System.Collections.IEnumerable)heap).GetEnumerator();
            // ReSharper disable once PossibleNullReferenceException
            while (enumerator.MoveNext()) result.Add(((HeapPair<int, int>)enumerator.Current).Value);
            Assert.That(result, Is.EquivalentTo(new[] { 1, 2, 3, 4 }));
        }
    }
}