using System.Reflection;
using _Fixtures.Sammlung.Extras;
using NUnit.Framework;
using Sammlung.Exceptions;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void CheckAllExceptions()
        {
            var exceptionTypes =
                ExceptionTestHelper.CollectExceptionTypes(Assembly.GetAssembly(typeof(DuplicateKeyException)));
            foreach (var excType in exceptionTypes)
            {
                ExceptionAssert.ProperImplementation(excType);
            }
        }
    }
}