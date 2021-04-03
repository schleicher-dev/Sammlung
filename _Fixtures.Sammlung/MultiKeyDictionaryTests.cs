using System;
using System.Collections.Generic;
using System.Linq;
using _Fixtures.Sammlung.Extras;
using NUnit.Framework;
using Sammlung.Dictionaries;
using Sammlung.Dictionaries.Concurrent;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class MultiKeyDictionaryTests
    {
        public static readonly MultiKeyDictConstructors<int, string>[] CtorTuples =
        {
            new MultiKeyDictConstructors<int, string>(
                () => new MultiKeyDictionary<int, string>(), 
                d => new MultiKeyDictionary<int, string>(d),
                c => new MultiKeyDictionary<int, string>(c)),
            new MultiKeyDictConstructors<int, string>(
                () => new BlockingMultiKeyDictionary<int, string>(), 
                d => new BlockingMultiKeyDictionary<int, string>(d),
                c => new BlockingMultiKeyDictionary<int, string>(c)),
        };
        
        [TestCaseSource(nameof(CtorTuples))]
        public void AddMultipleKeysAndRetrieveThem(MultiKeyDictConstructors<int, string> multiKeyDictConstructors)
        {
            var defCtor = multiKeyDictConstructors.Item1;
            var dictCtor = multiKeyDictConstructors.Item2;
            var capacityCtor = multiKeyDictConstructors.Item3;
            
            var mkDict = defCtor();
            mkDict[1, 2, 3] = "Hello";
            mkDict.Add(new [] {4, 5}, "World");
            mkDict[6] = "Goodbye";
            mkDict.Add(7, "Earth");
            mkDict.Add(new KeyValuePair<int, string>(8, "Blue"));
            
            Assert.AreEqual("Hello", mkDict[1]);
            Assert.AreEqual("Hello", mkDict[2]);
            Assert.AreEqual("Hello", mkDict[3]);
            Assert.AreEqual("World", mkDict[4]);
            Assert.AreEqual("World", mkDict[5]);
            Assert.AreEqual("Goodbye", mkDict[6]);
            Assert.AreEqual("Earth", mkDict[7]);
            Assert.AreEqual("Blue", mkDict[8]);
            Assert.AreEqual(8, mkDict.Count);
            Assert.IsFalse(mkDict.IsReadOnly);
            
            CollectionAssert.AreEquivalent(Enumerable.Range(1, 8), mkDict.Keys);
            CollectionAssert.AreEquivalent(
                Enumerable.Repeat("Hello", 3)
                    .Concat(Enumerable.Repeat("World", 2))
                    .Concat(Enumerable.Repeat("Goodbye", 1))
                    .Concat(Enumerable.Repeat("Earth", 1))
                    .Concat(Enumerable.Repeat("Blue", 1)), mkDict.Values);
            
            Assert.IsTrue(mkDict.ContainsKey(1));
            Assert.IsTrue(mkDict.ContainsKey(8));
            Assert.IsFalse(mkDict.ContainsKey(0));
            
            Assert.IsTrue(mkDict.Contains(new KeyValuePair<int, string>(1, "Hello")));
            Assert.IsTrue(mkDict.Contains(new KeyValuePair<int, string>(2, "Hello")));
            Assert.IsTrue(mkDict.Contains(new KeyValuePair<int, string>(8, "Blue")));
            
            Assert.IsTrue(mkDict.Remove(new KeyValuePair<int, string>(2, "Hello")));
            Assert.IsFalse(mkDict.Remove(new KeyValuePair<int, string>(1, "Planet")));
            Assert.IsTrue(mkDict.Remove(1));
            
            Assert.IsFalse(mkDict.TryGetValue(1, out _));
            Assert.IsTrue(mkDict.TryGetValue(3, out _));

            using (var enumerator = mkDict.GetEnumerator())
            {
                var list = new List<KeyValuePair<int, string>>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }
                CollectionAssert.AreEquivalent(
                    new []
                    {
                        new KeyValuePair<int, string>(3, "Hello"),
                        new KeyValuePair<int, string>(4, "World"),
                        new KeyValuePair<int, string>(5, "World"),
                        new KeyValuePair<int, string>(6, "Goodbye"),
                        new KeyValuePair<int, string>(7, "Earth"),
                        new KeyValuePair<int, string>(8, "Blue")
                    },
                    list);
            }
            
            var array = new KeyValuePair<int, string>[mkDict.Count];
            mkDict.CopyTo(array, 0);
            
            CollectionAssert.AreEquivalent(
                new []
                {
                    new KeyValuePair<int, string>(3, "Hello"),
                    new KeyValuePair<int, string>(4, "World"),
                    new KeyValuePair<int, string>(5, "World"),
                    new KeyValuePair<int, string>(6, "Goodbye"),
                    new KeyValuePair<int, string>(7, "Earth"),
                    new KeyValuePair<int, string>(8, "Blue")
                },
                array);

            var dmkDict = dictCtor(mkDict);
            CollectionAssert.AreEquivalent(mkDict, dmkDict);
            
            Assert.IsNotNull(capacityCtor(200));
            
            mkDict.Clear();
            CollectionAssert.IsEmpty(mkDict);
        }
    }
}