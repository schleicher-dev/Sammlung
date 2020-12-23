using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlungen.Collections;

namespace _Fixtures.Sammlungen
{
    [ExcludeFromCodeCoverage]
    public class BidiDictionaryTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void InsertPairsFindPairs()
        {
            var pairs = Enumerable.Range(1, 100).Zip(Enumerable.Range(100, 100).Reverse()).ToArray();
            var bDict = new BidiDictionary<int, int>();
            foreach (var (a, b) in pairs)
                bDict[a] = b;

            Assert.AreEqual(100, bDict.Count);

            foreach (var (a, b) in pairs)
            {
                Assert.AreEqual(a, bDict.ReverseMap[b]);
                Assert.AreEqual(b, bDict.ForwardMap[a]);
            }
        }

        [Test]
        public void ClearClearsAllMaps()
        {
            var pairs = Enumerable.Range(1, 100).Zip(Enumerable.Range(100, 100).Reverse()).ToArray();
            var bDict = new BidiDictionary<int, int>();
            foreach (var (a, b) in pairs)
                bDict[a] = b;
            
            bDict.Clear();
            Assert.AreEqual(0, bDict.Count);
            Assert.AreEqual(0, bDict.ForwardMap.Count);
            Assert.AreEqual(0, bDict.ReverseMap.Count);
        }

        [Test]
        public void ConstructorTests()
        {
            
        }
    }
}