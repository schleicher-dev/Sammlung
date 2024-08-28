using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Fixtures.Common
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionAssert
    {
        public static void ProperImplementation(Type excType)
        {
            Assert.That(typeof(Exception).IsAssignableFrom(excType), Is.True,
                $"Cannot assign '{excType.FullName}' to '{typeof(Exception).FullName}'");

            const string msgContent = "Message content";

            // Default Constructor
            var defaultCtor = excType.GetConstructor(Type.EmptyTypes);
            Assert.That(defaultCtor, Is.Not.Null);
            var defaultInst = (Exception)defaultCtor.Invoke(Array.Empty<object>());
            Assert.That(defaultInst, Is.Not.Null);

            // Message Constructor
            var messageCtor = excType.GetConstructor(new[] { typeof(string) });
            Assert.That(messageCtor, Is.Not.Null);
            var messageInst = (Exception)messageCtor.Invoke(new object[] { msgContent });
            Assert.That(messageInst, Is.Not.Null);
            Assert.That(messageInst.Message, Is.EqualTo(msgContent));

            // Message + InnerExc Constructor
            var innerExcCtor = excType.GetConstructor(new[] { typeof(string), typeof(Exception) });
            Assert.That(innerExcCtor, Is.Not.Null);
            var innerExcInst = (Exception)innerExcCtor.Invoke(new object[] { msgContent, defaultInst });
            Assert.That(innerExcInst, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(innerExcInst.Message, Is.EqualTo(msgContent));
                Assert.That(innerExcInst.InnerException, Is.EqualTo(defaultInst));
            });
        }
    }
}