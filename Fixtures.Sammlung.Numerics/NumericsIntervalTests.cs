using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Sammlung.Numerics;

namespace Fixtures.Sammlung.Numerics
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    public class NumericsIntervalTests
    {
        [Test]
        public void ConstructUnboundedIntervals()
        {
            var lowerUnbounded = Interval.Create(Bound.Empty<int>(), Bound.Exclusive(0));
            Assert.That(lowerUnbounded.Contains(int.MinValue), Is.True);
            Assert.That(lowerUnbounded.Contains(0), Is.False);
            Assert.That(lowerUnbounded.ToString(), Is.EqualTo("(-Inf; 0)"));

            var upperUnbounded = Interval.Create(Bound.Inclusive(0), Bound.Empty<int>());
            Assert.That(upperUnbounded.Contains(int.MaxValue), Is.True);
            Assert.That(upperUnbounded.Contains(0), Is.True);
            Assert.That(upperUnbounded.Contains(-1), Is.False);
            Assert.That(upperUnbounded.ToString(), Is.EqualTo("[0; +Inf)"));
        }

        [TestCase(1, true, 1, true)]
        [TestCase(1f, true, 1f, true)]
        [TestCase(1d, true, 1d, true)]
        [TestCase(1, true, 5, true)]
        [TestCase(1f, true, 5f, true)]
        [TestCase(1d, true, 5d, true)]
        public void ConstructingValidIntervals<T>(T lowerBound, bool lowerInclusive, T upperBound, bool upperInclusive)
            where T : struct, IComparable<T>
        {
            Assert.DoesNotThrow(() =>
                _ = Interval.Create(Bound.Create(lowerBound, lowerInclusive),
                    Bound.Create(upperBound, upperInclusive)));
        }

        [TestCase(1, false, 1, true)]
        [TestCase(1, true, 1, false)]
        [TestCase(1f, false, 1f, true)]
        [TestCase(1f, true, 1f, false)]
        [TestCase(1d, false, 1d, true)]
        [TestCase(1d, true, 1d, false)]
        [TestCase(1, true, -1, true)]
        [TestCase(1f, true, -1f, true)]
        [TestCase(1d, true, -1d, true)]
        public void ConstructingInvalidIntervals<T>(T lowerBound, bool lowerInclusive, T upperBound,
            bool upperInclusive) where T : struct, IComparable<T>
        {
            Assert.Throws<ArgumentException>(() =>
                _ = Interval.Create(Bound.Create(lowerBound, lowerInclusive),
                    Bound.Create(upperBound, upperInclusive)));
        }

        [TestCase(1, false, 2, true, 2, 1)]
        [TestCase(1f, false, 2f, true, 2f, 1f)]
        [TestCase(1d, false, 2d, true, 2d, 1d)]
        [TestCase(1, false, 3, false, 2, 3)]
        [TestCase(1f, false, 2f, false, 1.5f, 1f)]
        [TestCase(1d, false, 2d, false, 1.5d, 1d)]
        [TestCase(1, true, 3, false, 1, 3)]
        [TestCase(1f, true, 2f, false, 1f, 2f)]
        [TestCase(1d, true, 2d, false, 1d, 2d)]
        [TestCase(1, true, 1, true, 1, 2)]
        [TestCase(1f, true, 1f, true, 1f, 2f)]
        [TestCase(1d, true, 1d, true, 1d, 2d)]
        [TestCase(1, true, 5, true, 3, 6)]
        [TestCase(1f, true, 5f, true, 3f, 6f)]
        [TestCase(1d, true, 5d, true, 3d, 6d)]
        public void IntervalAndContainsCheck<T>(T lowerBound, bool lowerInclusive, T upperBound, bool upperInclusive,
            T inside, T outside) where T : struct, IComparable<T>
        {
            var interval = Interval.Create(Bound.Create(lowerBound, lowerInclusive),
                Bound.Create(upperBound, upperInclusive));
            Assert.That(interval.Contains(inside), Is.True);
            Assert.That(interval.Contains(outside), Is.False);
        }

        [Test]
        public void RequireInsideInterval()
        {
            var interval = Interval.CreateWithInclusiveAndExclusiveBounds(1f, 2f);
            Assert.DoesNotThrow(() => 1.5f.RequireElementOf(interval, "pName"));
            Assert.Throws<ArgumentOutOfRangeException>(() => 2f.RequireElementOf(interval, "pName"));
        }

        [TestCase(1, 2, "(1; 2)", "[1; 2]", "(1; 2]", "[1; 2)")]
        public void CheckIntervalRepresentation(int lower, int upper, string exEx, string inIn, string exIn, string inEx)
        {
            Assert.That(Interval.CreateWithExclusiveBounds(lower, upper).ToString(), Is.EqualTo(exEx));
            Assert.That(Interval.CreateWithInclusiveBounds(lower, upper).ToString(), Is.EqualTo(inIn));
            Assert.That(Interval.CreateWithExclusiveAndInclusiveBounds(lower, upper).ToString(), Is.EqualTo(exIn));
            Assert.That(Interval.CreateWithInclusiveAndExclusiveBounds(lower, upper).ToString(), Is.EqualTo(inEx));
        }
    }
}