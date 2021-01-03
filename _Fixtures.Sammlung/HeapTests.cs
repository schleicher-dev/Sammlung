using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlung;
using Sammlung.Heaps;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    internal class HeapValue<T> : IComparable<HeapValue<T>> where T : IComparable<T>
    {
        public static HeapValue<T> Create(int key, T value) => new HeapValue<T> {Key = key, Value = value};

        private HeapValue() {}
        
        public int Key { get; set; }
        public T Value { get; set; }

        /// <inheritdoc />
        public int CompareTo(HeapValue<T> other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var keyComparison = Key.CompareTo(other.Key);
            if (keyComparison != 0) return keyComparison;
            return Value.CompareTo(other.Value);
        }
    }
    
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
            var heap = new BinaryHeap<HeapValue<string>>();
            var items = Enumerable.Range(1, 10_000).Zip(GetNames()).ToList();
            foreach (var (key, value) in items.AsEnumerable().Reverse())
            {
                heap.Push(HeapValue<string>.Create(key, value));
            }

            var resultList = new List<string>();
            while(!heap.IsEmpty) resultList.Add(heap.Pop().Value);
            CollectionAssert.AreEqual(items.Select(kv => kv.Second), resultList);
        }
        
        [Test]
        public void PushAndPop_Randomized_SunnyPath()
        {
            var random = new Random(0);
            var heap = new BinaryHeap<HeapValue<int>>();

            for (var i = 0; i < 10_000; ++i)
            {
                var k = random.Next(int.MinValue, int.MaxValue);
                heap.Push(HeapValue<int>.Create(k, k));
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
            var heap = new BinaryHeap<HeapValue<string>>();
            Assert.IsTrue(heap.IsEmpty);
            Assert.IsFalse(heap.TryPop(out _));
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
            Assert.IsFalse(heap.TryPeek(out _));
            Assert.Throws<InvalidOperationException>(() => heap.Peek());
            Assert.IsFalse(heap.TryUpdate(HeapValue<string>.Create(0, "A"), HeapValue<string>.Create(1, "B")));
            Assert.Throws<InvalidOperationException>(() => heap.Update(HeapValue<string>.Create(0, "A"), HeapValue<string>.Create(1, "B")));
            Assert.IsFalse(heap.TryReplace(HeapValue<string>.Create(0, "A"), out _));
            Assert.Throws<InvalidOperationException>(() => heap.Replace(HeapValue<string>.Create(0, "A")));
        }

        [Test]
        public void UpdateReplace_SunnyPath()
        {

            var fValue = HeapValue<string>.Create(12, "F");
            var fPrimeValue = HeapValue<string>.Create(500, "F");

            var dValue = HeapValue<string>.Create(25, "D");
            var dPrimeValue = HeapValue<string>.Create(400, "D");
                
            var heap = new BinaryHeap<HeapValue<string>>();
            heap.Push(HeapValue<string>.Create(100, "A"));
            heap.Push(HeapValue<string>.Create(50, "B"));
            heap.Push(HeapValue<string>.Create(150, "C"));
            heap.Push(dValue);
            heap.Push(HeapValue<string>.Create(200, "E"));

            heap.Push(fValue);
            heap.Update(dValue, dPrimeValue);

            heap.Replace(fPrimeValue);
            
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

            var list = new List<HeapValue<int>>();
            for (var i = 0; i < 10_000; ++i)
            {
                var k = random.Next(int.MinValue, int.MaxValue);
                list.Add(HeapValue<int>.Create(k, k));
            }

            var comparerA = Comparer<int>.Default;
            var heapA = new BinaryHeap<HeapValue<int>>(list);
            
            var listA = new List<int>();
            var listB = new List<int>();
            while (!heapA.IsEmpty)
            {
                listA.Add(heapA.Pop().Value);
            }
            CollectionAssert.IsOrdered(listA, comparerA);
        }
    }
}