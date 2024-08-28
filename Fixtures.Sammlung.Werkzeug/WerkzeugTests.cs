using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlung.Werkzeug;

namespace Fixtures.Sammlung.Werkzeug
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class WerkzeugTests
    {
        private static void InternalRequire<T>(Func<T, T, string, T> func, T value, T expected, params T[] wrongValues)
        {
            const string paramName = "ParameterName";
            Assert.DoesNotThrow(() => func(value, expected, paramName));
            foreach (var wrongValue in wrongValues)
                Assert.Throws<ArgumentOutOfRangeException>(() => func(value, wrongValue, paramName));
        }

        private static void InternalRequireGreater<T>(T value, T expected, params T[] wrongValues)
            where T : IComparable<T> =>
            InternalRequire(GuardExtensions.RequireGreater, value, expected, wrongValues);

        private static void InternalRequireGreaterEqual<T>(T value, T expected, params T[] wrongValues)
            where T : IComparable<T> =>
            InternalRequire(GuardExtensions.RequireGreaterEqual, value, expected, wrongValues);

        private static void InternalRequireLess<T>(T value, T expected, params T[] wrongValues)
            where T : IComparable<T> =>
            InternalRequire(GuardExtensions.RequireLess, value, expected, wrongValues);

        private static void InternalRequireLessEqual<T>(T value, T expected, params T[] wrongValues)
            where T : IComparable<T> =>
            InternalRequire(GuardExtensions.RequireLessEqual, value, expected, wrongValues);
        
        private static void InternalRequireEqual<T>(T value, T expected, params T[] wrongValues)
            where T : IEquatable<T> =>
            InternalRequire(GuardExtensions.RequireEqual, value, expected, wrongValues);

        [TestCase(5, 4, 5, 6)]
        public void RequireGreater(int value, int expected, params int[] wrongValues)
        {
            InternalRequireGreater(value, expected, wrongValues);
            InternalRequireGreater(value, expected, wrongValues.Select(Convert.ToSingle).ToArray());
            InternalRequireGreater(value, expected, wrongValues.Select(Convert.ToDouble).ToArray());
            InternalRequireGreater(value, expected, wrongValues.Select(Convert.ToDecimal).ToArray());
        }

        [TestCase(5, 4, 6)]
        public void RequireGreaterEqual(int value, int expected, params int[] wrongValues)
        {
            InternalRequireGreaterEqual(value, expected, wrongValues);
            InternalRequireGreaterEqual(value, expected, wrongValues.Select(Convert.ToSingle).ToArray());
            InternalRequireGreaterEqual(value, expected, wrongValues.Select(Convert.ToDouble).ToArray());
            InternalRequireGreaterEqual(value, expected, wrongValues.Select(Convert.ToDecimal).ToArray());
        }

        [TestCase(5, 6, 5, 3)]
        public void RequireLess(int value, int expected, params int[] wrongValues)
        {
            InternalRequireLess(value, expected, wrongValues);
            InternalRequireLess(value, expected, wrongValues.Select(Convert.ToSingle).ToArray());
            InternalRequireLess(value, expected, wrongValues.Select(Convert.ToDouble).ToArray());
            InternalRequireLess(value, expected, wrongValues.Select(Convert.ToDecimal).ToArray());
        }

        [TestCase(5, 6, 2, 3)]
        public void RequireLessEqual(int value, int expected, params int[] wrongValues)
        {
            InternalRequireLessEqual(value, expected, wrongValues);
            InternalRequireLessEqual(value, expected, wrongValues.Select(Convert.ToSingle).ToArray());
            InternalRequireLessEqual(value, expected, wrongValues.Select(Convert.ToDouble).ToArray());
            InternalRequireLessEqual(value, expected, wrongValues.Select(Convert.ToDecimal).ToArray());
        }
        
        [TestCase(5, 5, 2, 3, -10)]
        public void RequireEqual(int value, int expected, params int[] wrongValues)
        {
            InternalRequireEqual(value, expected, wrongValues);
            InternalRequireEqual(value, expected, wrongValues.Select(Convert.ToSingle).ToArray());
            InternalRequireEqual(value, expected, wrongValues.Select(Convert.ToDouble).ToArray());
            InternalRequireEqual(value, expected, wrongValues.Select(Convert.ToDecimal).ToArray());
        }

        [Test]
        public void RequireNotNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(object).RequireNotNull("ParamName"));
            Assert.DoesNotThrow(() => new object().RequireNotNull("ParamName"));
        }

        [Test]
        public void RequireNotNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => default(string).RequireNotNullOrEmpty("ParamName"));
            Assert.Throws<ArgumentException>(() => string.Empty.RequireNotNullOrEmpty("ParamName"));
            Assert.Throws<ArgumentException>(() => "".RequireNotNullOrEmpty("ParamName"));
            Assert.DoesNotThrow(() => "ANY".RequireNotNull("ParamName"));
        }

        [Test]
        public void ComparisonExtensions_IsEqual([Range(0, 50)] int value)
        {
            Assert.That(value.IsEqual(value), Is.True);
            Assert.That(value.IsEqual(-value - 1), Is.False);
        }

        [Test]
        public void RequireNumElements()
        {
            var emptyArray = Array.Empty<double>();
            Assert.DoesNotThrow(() => emptyArray.RequireAtLeastNumElements(0, "ParamName"));
            Assert.Throws<ArgumentException>(() => emptyArray.RequireAtLeastNumElements(1, "ParamName"));
        }
    }
}