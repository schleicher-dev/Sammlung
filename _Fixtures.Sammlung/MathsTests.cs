using NUnit.Framework;
using Sammlung.Utilities;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    public class MathsTests
    {
        [Test]
        public void NextPowerOf2()
        {
            for (var i = 1; i < 10_000; ++i)
            {
                int logValue;
                for (logValue = 1; logValue < i; logValue *= 2) {}
                Assert.AreEqual(logValue, Maths.NextPowerOf2(i));
            }
        }
    }
}