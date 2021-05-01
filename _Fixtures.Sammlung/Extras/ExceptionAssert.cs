using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace _Fixtures.Sammlung.Extras
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionAssert
    {
        public static void ProperImplementation(Type excType)
        {
            Assert.IsTrue(typeof(Exception).IsAssignableFrom(excType),
                $"Cannot assign '{excType.FullName}' to '{typeof(Exception).FullName}'");

            const string msgContent = "Message content";

            // Default Constructor
            var defaultCtor = excType.GetConstructor(new Type[0]);
            Assert.IsNotNull(defaultCtor);
            var defaultInst = (Exception) defaultCtor.Invoke(new object[0]);
            Assert.IsNotNull(defaultInst);

            // Message Constructor
            var messageCtor = excType.GetConstructor(new[] {typeof(string)});
            Assert.IsNotNull(messageCtor);
            var messageInst = (Exception) messageCtor.Invoke(new object[] {msgContent});
            Assert.IsNotNull(messageInst);
            Assert.AreEqual(msgContent, messageInst.Message);

            // Message + InnerExc Constructor
            var innerExcCtor = excType.GetConstructor(new[] {typeof(string), typeof(Exception)});
            Assert.IsNotNull(innerExcCtor);
            var innerExcInst = (Exception) innerExcCtor.Invoke(new object[] {msgContent, defaultInst});
            Assert.IsNotNull(innerExcInst);
            Assert.AreEqual(msgContent, innerExcInst.Message);
            Assert.AreEqual(defaultInst, innerExcInst.InnerException);
            
            // Serialization + Deserialization
            var memStream = new MemoryStream();
            var binFormatter = new BinaryFormatter();
            binFormatter.Serialize(memStream, messageInst);

            memStream.Position = 0;
            var reconMessageInst = (Exception) binFormatter.Deserialize(memStream);
            Assert.AreEqual(messageInst.Message, reconMessageInst.Message);
        }
    }
}