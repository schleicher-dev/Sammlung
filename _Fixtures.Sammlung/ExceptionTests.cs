using System.Globalization;
using System.Reflection;
using _Fixtures.Sammlung.Extras;
using NUnit.Framework;
using Sammlung.Exceptions;
using Sammlung.Resources;
using Sammlung.Utilities;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
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

        [Test]
        [SetCulture("de-de")]
        [SetUICulture("de-de")]
        public void ErrorMessage_DoesNotMatchDefaultMessage_WhenNotInvariantCulture()
        {
            var defaultMessage = ErrorMessages.ResourceManager
                .GetString(nameof(ErrorMessages.DuplicateKeyFound), CultureInfo.InvariantCulture);
            defaultMessage = string.Format(defaultMessage, "key", nameof(ErrorMessage_DoesNotMatchDefaultMessage_WhenNotInvariantCulture));
            Assert.AreNotEqual(defaultMessage, ExceptionsHelper.NewDuplicateKeyException("key", nameof(ErrorMessage_DoesNotMatchDefaultMessage_WhenNotInvariantCulture)).Message);
        }

        [Test]
        [SetCulture("en-us")]
        [SetUICulture("en-us")]
        public void ErrorMessage_DoesMatchDefaultMessage()
        {
            var defaultMessage = ErrorMessages.ResourceManager
                .GetString(nameof(ErrorMessages.DuplicateKeyFound), CultureInfo.InvariantCulture);
            defaultMessage = string.Format(defaultMessage, "key", nameof(ErrorMessage_DoesMatchDefaultMessage));
            Assert.AreEqual(defaultMessage, ExceptionsHelper.NewDuplicateKeyException("key", nameof(ErrorMessage_DoesMatchDefaultMessage)).Message);
        }
    }
}