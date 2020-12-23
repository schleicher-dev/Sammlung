using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlungen.Collections;
using Sammlungen.Collections.Concurrent;
using Sammlungen.Exceptions;

namespace _Fixtures.Sammlungen
{
    public class FuncTuple
    {
        public static FuncTuple Create(DefCreatorFunc zf, DictCreatorFunc df, EnumCreatorFunc ef)
        {
            return new FuncTuple {DefaultCreatorFunc = zf, DictionaryCreatorFunc = df, EnumerableCreatorFunc = ef};
        }

        public delegate IBidiDictionary<int, int> DefCreatorFunc();
        public delegate IBidiDictionary<int, int> DictCreatorFunc(Dictionary<int, int> dict);

        public delegate IBidiDictionary<int, int> EnumCreatorFunc(IEnumerable<KeyValuePair<int, int>> enumerable);
        
        public DefCreatorFunc DefaultCreatorFunc { get; set; }
        public DictCreatorFunc DictionaryCreatorFunc { get; set; }
        public EnumCreatorFunc EnumerableCreatorFunc { get; set; }

        public void Deconstruct(out DefCreatorFunc zf, out DictCreatorFunc df, out EnumCreatorFunc ef)
        {
            zf = DefaultCreatorFunc;
            df = DictionaryCreatorFunc;
            ef = EnumerableCreatorFunc;
        }
    }
    
        
    
    [ExcludeFromCodeCoverage]
    public class BidiDictionaryTests
    {
        [SetUp]
        public void Setup() { }
        
        public static readonly FuncTuple[] BidiDicts =
        {
            FuncTuple.Create(() => new BidiDictionary<int, int>(), 
                d => new BidiDictionary<int, int>(d),
                e => new BidiDictionary<int, int>(e)), 
            FuncTuple.Create(() => new ConcurrentBidiDictionary<int, int>(),
                d => new ConcurrentBidiDictionary<int, int>(1, d),
                e => new ConcurrentBidiDictionary<int, int>(1, e)),
        };
        
        [TestCaseSource(nameof(BidiDicts))]
        public void InsertPairsFindPairs(FuncTuple tuple)
        {
            var (zf, _, _) = tuple;
            var pairs = Enumerable.Range(1, 100).Zip(Enumerable.Range(100, 100).Reverse()).ToArray();
            var bDict = zf();
            foreach (var (a, b) in pairs)
                bDict[a] = b;

            Assert.AreEqual(100, bDict.Count);

            foreach (var (a, b) in pairs)
            {
                Assert.AreEqual(a, bDict.ReverseMap[b]);
                Assert.AreEqual(b, bDict.ForwardMap[a]);
            }
        }

        [TestCaseSource(nameof(BidiDicts))]
        public void ClearClearsAllMaps(FuncTuple tuple)
        {
            var (zf, _, _) = tuple;
            var pairs = Enumerable.Range(1, 100).Zip(Enumerable.Range(100, 100).Reverse()).ToArray();
            var bDict = zf();
            foreach (var (a, b) in pairs)
                bDict[a] = b;
            
            bDict.Clear();
            Assert.AreEqual(0, bDict.Count);
            Assert.AreEqual(0, bDict.ForwardMap.Count);
            Assert.AreEqual(0, bDict.ReverseMap.Count);
        }
        
        [TestCaseSource(nameof(BidiDicts))]
        public void ConstructorTests(FuncTuple tuple)
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
    }
}