using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Fixtures.Sammlung.Collections.Dictionaries.Extras;
using NUnit.Framework;
using Sammlung.Collections.Dictionaries;
using Sammlung.Collections.Dictionaries.Concurrent;

namespace Fixtures.Sammlung.Collections.Dictionaries
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class MultiKeyDictionaryTests
    {
        public static readonly MultiKeyDictConstructors<int, string>[] CtorTuples =
        {
            new(
                () => new MultiKeyDictionary<int, string>(), 
                d => new MultiKeyDictionary<int, string>(d),
                c => new MultiKeyDictionary<int, string>(c)),
            new(
                () => new BlockingMultiKeyDictionary<int, string>(), 
                d => new BlockingMultiKeyDictionary<int, string>(d),
                c => new BlockingMultiKeyDictionary<int, string>(c))
        };

        [Test]
        public void AllConstructor()
        {
            Assert.DoesNotThrow(() => _ = new MultiKeyDictionary<string, string>());
            Assert.DoesNotThrow(() => _ = new MultiKeyDictionary<string, string>(EqualityComparer<string>.Default));
            Assert.DoesNotThrow(() => _ = new MultiKeyDictionary<string, string>(new Dictionary<string, string>()));
            Assert.DoesNotThrow(() => _ = new MultiKeyDictionary<string, string>(new Dictionary<string, string>(), EqualityComparer<string>.Default));
            Assert.DoesNotThrow(() => _ = new MultiKeyDictionary<string, string>(new List<KeyValuePair<string, string>>()));
            Assert.DoesNotThrow(() => _ = new MultiKeyDictionary<string, string>(50));
            Assert.DoesNotThrow(() => _ = new MultiKeyDictionary<string, string>(50, EqualityComparer<string>.Default));
            Assert.DoesNotThrow(() => _ = new MultiKeyDictionary<string, string>(new List<KeyValuePair<string, string>>(), EqualityComparer<string>.Default));
            
            Assert.DoesNotThrow(() => _ = new BlockingMultiKeyDictionary<string, string>());
            Assert.DoesNotThrow(() => _ = new BlockingMultiKeyDictionary<string, string>(5));
            Assert.DoesNotThrow(() => _ = new BlockingMultiKeyDictionary<string, string>(5, EqualityComparer<string>.Default));
            Assert.DoesNotThrow(() => _ = new BlockingMultiKeyDictionary<string, string>(new Dictionary<string, string>(), EqualityComparer<string>.Default));
        }
        
        [TestCaseSource(nameof(CtorTuples))]
        public void AddMultipleKeysAndRetrieveThem(MultiKeyDictConstructors<int, string> multiKeyDictConstructors)
        {
            var (defCtor, dictCtor, capacityCtor) = multiKeyDictConstructors;

            var mkDict = defCtor();
            mkDict[1, 2, 3] = "Hello";
            mkDict.Add(new [] {4, 5}, "World");
            mkDict[6] = "Goodbye";
            mkDict.Add(7, "Earth");
            mkDict.Add(new KeyValuePair<int, string>(8, "Blue"));

            Assert.That(mkDict[1], Is.EqualTo("Hello"));
            Assert.That(mkDict[2], Is.EqualTo("Hello"));
            Assert.That(mkDict[3], Is.EqualTo("Hello"));
            Assert.That(mkDict[4], Is.EqualTo("World"));
            Assert.That(mkDict[5], Is.EqualTo("World"));
            Assert.That(mkDict[6], Is.EqualTo("Goodbye"));
            Assert.That(mkDict[7], Is.EqualTo("Earth"));
            Assert.That(mkDict[8], Is.EqualTo("Blue"));
            Assert.That(mkDict.Count, Is.EqualTo(8));
            Assert.That(mkDict.IsReadOnly, Is.False);

            Assert.That(mkDict.Keys, Is.EquivalentTo(Enumerable.Range(1, 8)));
            Assert.That(
                mkDict.Values, Is.EquivalentTo(Enumerable.Repeat("Hello", 3)
                    .Concat(Enumerable.Repeat("World", 2))
                    .Concat(Enumerable.Repeat("Goodbye", 1))
                    .Concat(Enumerable.Repeat("Earth", 1))
                    .Concat(Enumerable.Repeat("Blue", 1))));

            Assert.That(mkDict.ContainsKey(1), Is.True);
            Assert.That(mkDict.ContainsKey(8), Is.True);
            Assert.That(mkDict.ContainsKey(0), Is.False);

            Assert.That(mkDict.Contains(new KeyValuePair<int, string>(1, "Hello")), Is.True);
            Assert.That(mkDict.Contains(new KeyValuePair<int, string>(2, "Hello")), Is.True);
            Assert.That(mkDict.Contains(new KeyValuePair<int, string>(8, "Blue")), Is.True);

            Assert.That(mkDict.Remove(new KeyValuePair<int, string>(2, "Hello")), Is.True);
            Assert.That(mkDict.Remove(new KeyValuePair<int, string>(1, "Planet")), Is.False);
            Assert.That(mkDict.Remove(1), Is.True);

            Assert.That(mkDict.TryGetValue(1, out _), Is.False);
            Assert.That(mkDict.TryGetValue(3, out _), Is.True);

            using (var enumerator = mkDict.GetEnumerator())
            {
                var list = new List<KeyValuePair<int, string>>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }
                Assert.That(
                    list,
                    Is.EquivalentTo(new[]
                    {
                        new KeyValuePair<int, string>(3, "Hello"),
                        new KeyValuePair<int, string>(4, "World"),
                        new KeyValuePair<int, string>(5, "World"),
                        new KeyValuePair<int, string>(6, "Goodbye"),
                        new KeyValuePair<int, string>(7, "Earth"),
                        new KeyValuePair<int, string>(8, "Blue")
                    }));
            }
            
            var array = new KeyValuePair<int, string>[mkDict.Count];
            mkDict.CopyTo(array, 0);

            Assert.That(
                array,
                Is.EquivalentTo(new[]
                {
                    new KeyValuePair<int, string>(3, "Hello"),
                    new KeyValuePair<int, string>(4, "World"),
                    new KeyValuePair<int, string>(5, "World"),
                    new KeyValuePair<int, string>(6, "Goodbye"),
                    new KeyValuePair<int, string>(7, "Earth"),
                    new KeyValuePair<int, string>(8, "Blue")
                }));

            var dmkDict = dictCtor(mkDict);
            Assert.That(dmkDict, Is.EquivalentTo(mkDict));
            
            Assert.That(capacityCtor(200), Is.Not.Null);
            
            mkDict.Clear();
            Assert.That(mkDict, Is.Empty);
        }
    }
}