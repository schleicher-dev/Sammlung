using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Fixtures.Sammlung.Collections.Dictionaries.Extras;
using NUnit.Framework;
using Sammlung.Collections.Dictionaries;
using Sammlung.Collections.Dictionaries.Concurrent;

namespace Fixtures.Sammlung.Collections.Dictionaries
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    public class BidiDictionaryTests
    {
        [SetUp]
        public void Setup() { }
        
        public static readonly BidiDictConstructors[] BidiDictionaries =
        {
            new(() => new BidiDictionary<int, int>(), 
                d => new BidiDictionary<int, int>(d),
                e => new BidiDictionary<int, int>(e)), 
            new(() => new BlockingBidiDictionary<int, int>(),
                d => new BlockingBidiDictionary<int, int>(d),
                e => new BlockingBidiDictionary<int, int>(e))
        };
        
        [TestCaseSource(nameof(BidiDictionaries))]
        public void InsertPairsFindPairs(BidiDictConstructors tuple)
        {
            var (zf, _, _) = tuple;
            var pairs = Enumerable.Range(1, 100).Zip(Enumerable.Range(100, 100).Reverse(), Tuple.Create).ToArray();
            var bDict = zf();
            Assert.That(bDict.IsReadOnly, Is.False);
            foreach (var (a, b) in pairs)
                bDict[a] = b;

            Assert.That(bDict.Count, Is.EqualTo(100));

            foreach (var (a, b) in pairs)
            {
                Assert.That(bDict[a], Is.EqualTo(b));
                Assert.That(bDict.ReverseMap[b], Is.EqualTo(a));
                Assert.That(bDict.ForwardMap[a], Is.EqualTo(b));
                Assert.That(bDict.Contains(new KeyValuePair<int, int>(a, b)), Is.True);
                Assert.That(bDict.ContainsKey(a), Is.True);
                Assert.That(bDict.ForwardMap.ContainsKey(a), Is.True);
                Assert.That(bDict.ReverseMap.ContainsKey(b), Is.True);
            }

            Assert.That(bDict.Keys, Is.EquivalentTo(Enumerable.Range(1, 100)));
            Assert.That(bDict.Values, Is.EquivalentTo(Enumerable.Range(100, 100)));
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
            Assert.That(bDict.Count, Is.EqualTo(0));
            Assert.That(bDict.ForwardMap.Count, Is.EqualTo(0));
            Assert.That(bDict.ReverseMap.Count, Is.EqualTo(0));
        }

        [TestCaseSource(nameof(BidiDictionaries))]
        public void DifferentMethodsCovering(BidiDictConstructors tuple)
        {
            var (zf, _, _) = tuple;
            var bDict = zf();
            bDict.Add(new KeyValuePair<int, int>(1, 2));
            bDict.Add(new KeyValuePair<int, int>(2, 3));

            var bdEnum = ((IEnumerable) bDict).GetEnumerator();
            while (bdEnum.MoveNext())
            {
                Assert.IsNotNull(bdEnum);
                var kvPair = (KeyValuePair<int, int>) bdEnum.Current;
                var fwd = kvPair.Key;
                var rev = kvPair.Value;
                Assert.That(fwd == 1 && rev == 2 || fwd == 2 && rev == 3, Is.True);
            }

            var fwdEnum = ((IEnumerable) bDict.ForwardMap).GetEnumerator();
            while (fwdEnum.MoveNext())
            {
                Assert.IsNotNull(fwdEnum);
                var kvPair = (KeyValuePair<int, int>) fwdEnum.Current;
                var fwd = kvPair.Key;
                var rev = kvPair.Value;
                Assert.That(fwd == 1 && rev == 2 || fwd == 2 && rev == 3, Is.True);
            }

            Assert.That(bDict.ForwardMap.Keys, Is.EquivalentTo(new[] { 1, 2 }));
            Assert.That(bDict.ForwardMap.Values, Is.EquivalentTo(new[] { 2, 3 }));
            Assert.That(bDict.ReverseMap.Keys, Is.EquivalentTo(new[] { 2, 3 }));
            Assert.That(bDict.ReverseMap.Values, Is.EquivalentTo(new[] { 1, 2 }));
            Assert.That(bDict.ForwardMap,
                Is.EquivalentTo(new[] { new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(2, 3) }));
            Assert.That(bDict.ReverseMap,
                Is.EquivalentTo(new[] { new KeyValuePair<int, int>(2, 1), new KeyValuePair<int, int>(3, 2) }));

            Assert.That(bDict.ForwardMap.TryGetValue(1, out var fwdValue), Is.True);
            Assert.That(fwdValue, Is.EqualTo(2));
            Assert.That(bDict.ReverseMap.TryGetValue(2, out var revValue), Is.True);
            Assert.That(revValue, Is.EqualTo(1));

            Assert.That(bDict.ForwardMap[1], Is.EqualTo(2));
            Assert.That(bDict.ReverseMap[2], Is.EqualTo(1));
            Assert.That(bDict.TryGetValue(1, out var value), Is.True);
            Assert.That(value, Is.EqualTo(2));
            Assert.That(bDict.TryGetValue(3, out _), Is.False);
            Assert.That(bDict.Remove(3), Is.False);
            Assert.That(bDict.Remove(2), Is.True);
            Assert.That(bDict.Remove(new KeyValuePair<int, int>(1, 2)), Is.True);
            Assert.That(bDict.Count, Is.EqualTo(0));
        }

        [TestCaseSource(nameof(BidiDictionaries))]
        public void ConstructorTests(BidiDictConstructors tuple)
        {
            var (_, df, ef) = tuple;
            var d1 = new Dictionary<int, int> {[0] = 100, [1] = 100};
            Assert.Throws<ArgumentException>(() =>df(d1));
            Assert.Throws<ArgumentException>(() => ef(d1.AsEnumerable()));
            
            var d2 = new Dictionary<int, int> {[0] = 100, [1] = 101};
            var b2 = df(d2);
            Assert.That(b2.ForwardMap[0], Is.EqualTo(100));
            Assert.That(b2.ForwardMap[1], Is.EqualTo(101));
            Assert.That(b2.ReverseMap[100], Is.EqualTo(0));
            Assert.That(b2.ReverseMap[101], Is.EqualTo(1));
            Assert.That(b2.ForwardRemove(0), Is.True);
            Assert.That(b2.ReverseRemove(101), Is.True);
            Assert.That(b2.Count, Is.EqualTo(0));
            
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
            Assert.That(array, Is.EquivalentTo(bDict));
        }
    }
}