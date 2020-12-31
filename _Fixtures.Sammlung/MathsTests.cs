using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Sammlung.Utilities;
using Sammlung.Utilities.Math;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
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