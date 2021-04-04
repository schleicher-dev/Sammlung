using System.Collections.Generic;
using NUnit.Framework;
using Sammlung.Dictionaries.Concurrent;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class BlockingDictionaryTests
    {
        [Test]
        public void CreateBlockingDictionary()
        {
            Assert.DoesNotThrow(() => _ = new BlockingDictionary<string, int>());
            Assert.DoesNotThrow(() => _ = new BlockingDictionary<string, int>(1));
            Assert.DoesNotThrow(() => _ = new BlockingDictionary<string, int>(EqualityComparer<string>.Default));
            Assert.DoesNotThrow(() => _ = new BlockingDictionary<string, int>(1, EqualityComparer<string>.Default));
            Assert.DoesNotThrow(() => _ = new BlockingDictionary<string, int>(new Dictionary<string, int>()));
            Assert.DoesNotThrow(() => _ = new BlockingDictionary<string, int>(new Dictionary<string, int>(), EqualityComparer<string>.Default));
        }
    }
}