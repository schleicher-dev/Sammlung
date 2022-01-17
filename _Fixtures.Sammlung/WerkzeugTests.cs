using System;
using System.Linq;
using NUnit.Framework;
using Sammlung.Werkzeug;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class WerkzeugTests
    {
        private static void InternalRequire<T>(Func<T, T, string, T> func, T value, T expected, params T[] wrongValues)
            where T : IComparable<T>
        {
            const string paramName = "ParameterName";
            Assert.DoesNotThrow(() => func(value, expected, paramName));
            foreach (var wrongValue in wrongValues)
                Assert.Throws<ArgumentOutOfRangeException>(() => func(value, wrongValue, paramName));
        }

        private static void InternalRequireGreater<T>(T value, T expected, params T[] wrongValues)
            where T : IComparable<T> =>
            InternalRequire(ParamValidationExtensions.RequireGreater, value, expected, wrongValues);

        private static void InternalRequireGreaterEqual<T>(T value, T expected, params T[] wrongValues)
            where T : IComparable<T> =>
            InternalRequire(ParamValidationExtensions.RequireGreaterEqual, value, expected, wrongValues);

        private static void InternalRequireLess<T>(T value, T expected, params T[] wrongValues)
            where T : IComparable<T> =>
            InternalRequire(ParamValidationExtensions.RequireLess, value, expected, wrongValues);

        private static void InternalRequireLessEqual<T>(T value, T expected, params T[] wrongValues)
            where T : IComparable<T> =>
            InternalRequire(ParamValidationExtensions.RequireLessEqual, value, expected, wrongValues);

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

        [Test]
        public void RequireNotNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(object).RequireNotNull("ParamName"));
            Assert.DoesNotThrow(() => new object().RequireNotNull("ParamName"));
        }

        [Test]
        public void RequireNotNullOrEmpty()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => default(string).RequireNotNullOrEmpty("ParamName"));
            Assert.Throws<ArgumentOutOfRangeException>(() => string.Empty.RequireNotNullOrEmpty("ParamName"));
            Assert.Throws<ArgumentOutOfRangeException>(() => "".RequireNotNullOrEmpty("ParamName"));
            Assert.DoesNotThrow(() => "ANY".RequireNotNull("ParamName"));
        }

        [Test]
        public void ComparisonExtensions_IsEqual([Range(0, 50)] int value)
        {
            Assert.IsTrue(value.IsEqual(value));
            Assert.IsFalse(value.IsEqual(-value - 1));
        }

        [Test]
        public void RequireNumElements()
        {
            var emptyArray = Array.Empty<double>();
            Assert.DoesNotThrow(() => emptyArray.RequireNumElements(0, "ParamName"));
            Assert.Throws<ArgumentException>(() => emptyArray.RequireNumElements(1, "ParamName"));
        }
    }
}