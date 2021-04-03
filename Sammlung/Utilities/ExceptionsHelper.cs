using System;
using System.Globalization;
using Sammlung.Exceptions;
using Sammlung.Resources;

namespace Sammlung.Utilities
{
    public static class ExceptionsHelper
    {
        private static string GetString(string name)
        {
            var originalMsg = ErrorMessages.ResourceManager.GetString(name, CultureInfo.InvariantCulture);
            if (Equals(CultureInfo.CurrentUICulture, CultureInfo.InvariantCulture) ||
                Equals(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, "en")) return originalMsg;

            var translatedMsg = ErrorMessages.ResourceManager.GetString(name);
            return $"{translatedMsg} ({originalMsg})";
        }

        private static string GetFormattedString(string name, params object[] args) =>
            string.Format(GetString(name), args);

        public static DuplicateKeyException NewDuplicateKeyException(object key, string methodName, Exception innerException = null)
        {
            var msg = GetFormattedString(nameof(ErrorMessages.DuplicateKeyFound), key, methodName);
            return new DuplicateKeyException(msg, innerException);
        }

        public static InvalidOperationException NewEmptyCollectionException(string methodName, Exception innerException = null)
        {
            var msg = GetFormattedString(nameof(ErrorMessages.RemoveOnEmptyCollection), methodName);
            return new InvalidOperationException(msg, innerException);
        }
        
        public static InvalidOperationException NewHeapUpdateFailedException(string methodName, Exception innerException = null)
        {
            var msg = GetFormattedString(nameof(ErrorMessages.HeapUpdateFailed), methodName);
            return new InvalidOperationException(msg, innerException);
        }
    }
}