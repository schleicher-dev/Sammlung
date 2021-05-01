using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using _Fixtures.Sammlung.Extras;
using NUnit.Framework;
using Sammlung.Dictionaries;
using Sammlung.Dictionaries.Concurrent;
using Sammlung.Exceptions;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    public class BidiDictionaryTests
    {
        [SetUp]
        public void Setup() { }
        
        public static readonly BidiDictConstructors[] BidiDictionaries =
        {
            new BidiDictConstructors(() => new BidiDictionary<int, int>(), 
                d => new BidiDictionary<int, int>(d),
                e => new BidiDictionary<int, int>(e)), 
            new BidiDictConstructors(() => new BlockingBidiDictionary<int, int>(),
                d => new BlockingBidiDictionary<int, int>(d),
                e => new BlockingBidiDictionary<int, int>(e))
        };
        
        [TestCaseSource(nameof(BidiDictionaries))]
        public void InsertPairsFindPairs(BidiDictConstructors tuple)
        {
            var (zf, _, _) = tuple;
            var pairs = Enumerable.Range(1, 100).Zip(Enumerable.Range(100, 100).Reverse(), Tuple.Create).ToArray();
            var bDict = zf();
            Assert.IsFalse(bDict.IsReadOnly);
            foreach (var (a, b) in pairs)
                bDict[a] = b;

            Assert.AreEqual(100, bDict.Count);

            foreach (var (a, b) in pairs)
            {
                Assert.AreEqual(b, bDict[a]);
                Assert.AreEqual(a, bDict.ReverseMap[b]);
                Assert.AreEqual(b, bDict.ForwardMap[a]);
                Assert.IsTrue(bDict.Contains(new KeyValuePair<int, int>(a, b)));
                Assert.IsTrue(bDict.ContainsKey(a));
                Assert.IsTrue(bDict.ForwardMap.ContainsKey(a));
                Assert.IsTrue(bDict.ReverseMap.ContainsKey(b));
            }
            
            CollectionAssert.AreEquivalent(Enumerable.Range(1, 100), bDict.Keys);
            CollectionAssert.AreEquivalent(Enumerable.Range(100, 100), bDict.Values);
        }

        [TestCaseSource(nameof(BidiDictionaries))]
        public void ClearClearsAllMaps(BidiDictConstructors tuple)
        {
            var (zf, _, _) = tuple;
            var pairs = Enumerable.Range(1, 100).Zip(Enumerable.Range(100, 100).Reverse(), Tuple.Create).ToArray();
            var bDict = zf();
            foreach (var (a, b) in pairs)
                bDict[a] = b;
            
            bDict.Clear();
            Assert.AreEqual(0, bDict.Count);
            Assert.AreEqual(0, bDict.ForwardMap.Count);
            Assert.AreEqual(0, bDict.ReverseMap.Count);
        }

        [TestCaseSource(nameof(BidiDictionaries))]
        [SuppressMessage("ReSharper", "UseDeconstruction", Justification = "Not de-constructable")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Is not null")]
        public void DifferentMethodsCovering(BidiDictConstructors tuple)
        {
            var (zf, _, _) = tuple;
            var bDict = zf();
            bDict.Add(new KeyValuePair<int, int>(1, 2));
            bDict.Add(new KeyValuePair<int, int>(2, 3));

            var bdEnum = ((IEnumerable) bDict).GetEnumerator();
            while (bdEnum.MoveNext())
            {
                var kvPair = (KeyValuePair<int, int>) bdEnum.Current;
                var fwd = kvPair.Key;
                var rev = kvPair.Value;
                Assert.IsTrue(fwd == 1 && rev == 2 || fwd == 2 && rev == 3);
            }

            var fwdEnum = ((IEnumerable) bDict.ForwardMap).GetEnumerator();
            while (fwdEnum.MoveNext())
            {
                var kvPair = (KeyValuePair<int, int>) fwdEnum.Current;
                var fwd = kvPair.Key;
                var rev = kvPair.Value;
                Assert.IsTrue(fwd == 1 && rev == 2 || fwd == 2 && rev == 3);
            }
            
            CollectionAssert.AreEquivalent(new [] {1, 2}, bDict.ForwardMap.Keys);
            CollectionAssert.AreEquivalent(new [] {2, 3}, bDict.ForwardMap.Values);
            CollectionAssert.AreEquivalent(new [] {2, 3}, bDict.ReverseMap.Keys);
            CollectionAssert.AreEquivalent(new [] {1, 2}, bDict.ReverseMap.Values);
            CollectionAssert.AreEquivalent(new[] {new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(2, 3)},
                bDict.ForwardMap);
            CollectionAssert.AreEquivalent(new[] {new KeyValuePair<int, int>(2, 1), new KeyValuePair<int, int>(3, 2)},
                bDict.ReverseMap);
            
            Assert.IsTrue(bDict.ForwardMap.TryGetValue(1, out var fwdValue));
            Assert.AreEqual(2, fwdValue);
            Assert.IsTrue(bDict.ReverseMap.TryGetValue(2, out var revValue));
            Assert.AreEqual(1, revValue);
            
            Assert.AreEqual(2, bDict.ForwardMap[1]);
            Assert.AreEqual(1, bDict.ReverseMap[2]);
            Assert.IsTrue(bDict.TryGetValue(1, out var value));
            Assert.AreEqual(2, value);
            Assert.IsFalse(bDict.TryGetValue(3, out _));
            Assert.IsFalse(bDict.Remove(3));
            Assert.IsTrue(bDict.Remove(2));
            Assert.IsTrue(bDict.Remove(new KeyValuePair<int, int>(1, 2)));
            Assert.AreEqual(0, bDict.Count);
        }

        [TestCaseSource(nameof(BidiDictionaries))]
        public void ConstructorTests(BidiDictConstructors tuple)
        {
            var (_, df, ef) = tuple;
            var d1 = new Dictionary<int, int> {[0] = 100, [1] = 100};
            Assert.Throws<DuplicateKeyException>(() =>df(d1));
            Assert.Throws<DuplicateKeyException>(() => ef(d1.AsEnumerable()));
            
            var d2 = new Dictionary<int, int> {[0] = 100, [1] = 101};
            var b2 = df(d2);
            Assert.AreEqual(100, b2.ForwardMap[0]);
            Assert.AreEqual(101, b2.ForwardMap[1]);
            Assert.AreEqual(0, b2.ReverseMap[100]);
            Assert.AreEqual(1, b2.ReverseMap[101]);
            Assert.IsTrue(b2.ForwardRemove(0));
            Assert.IsTrue(b2.ReverseRemove(101));
            Assert.AreEqual(0, b2.Count);
            
            var _ = ef(d2.AsEnumerable());
        }

        [TestCaseSource(nameof(BidiDictionaries))]
        public void CopyTo_SunnyPath(BidiDictConstructors tuple)
        {
            var (_, _, ef) = tuple;
            var pairs = Enumerable.Range(1, 100).Zip(Enumerable.Range(100, 100).Reverse(), Tuple.Create).ToArray();
            var bDict = ef(pairs.Select(t => new KeyValuePair<int, int>(t.Item1, t.Item2)));
            var array = new KeyValuePair<int, int>[100];
            bDict.CopyTo(array, 0);
            CollectionAssert.AreEquivalent(bDict, array);
        }
    }
}