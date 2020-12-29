using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.CompilerServices;
using Sammlung.Exceptions;
using Sammlung.Resources;

namespace Sammlung.Utilities
{
    public static class ExceptionsHelper
    {
        private static string GetString(string name)
        {
            var originalMsg = ErrorMessages.ResourceManager.GetString(name, CultureInfo.InvariantCulture);
            if (Equals(CultureInfo.CurrentUICulture, CultureInfo.InvariantCulture)) return originalMsg;

            var translatedMsg = ErrorMessages.ResourceManager.GetString(name);
            return $"{translatedMsg} ({originalMsg})";
        }

        private static string GetFormattedString(string name, params object[] args) =>
            string.Format(GetString(name), args);

        public static DuplicateKeyException NewDuplicateKeyException(object key, Exception innerException = null,
            [CallerMemberName] string methodName = null)
        {
            var msg = GetFormattedString(nameof(ErrorMessages.DuplicateKeyFound), key, methodName);
            return new DuplicateKeyException(msg, innerException);
        }

        public static InvalidOperationException NewEmptyCollectionException(Exception innerException = null,
            [CallerMemberName] string methodName = null)
        {
            var msg = GetFormattedString(nameof(ErrorMessages.RemoveOnEmptyCollection), methodName);
            return new InvalidOperationException(msg, innerException);
        }
        
        public static InvalidOperationException NewHeapUpdateFailedException(Exception innerException = null,
            [CallerMemberName] string methodName = null)
        {
            var msg = GetFormattedString(nameof(ErrorMessages.HeapUpdateFailed), methodName);
            return new InvalidOperationException(msg, innerException);
        }

        public static NotSupportedException NewCallToMethodNotSupportedException(Exception innerException = null,
            [CallerMemberName] string methodName = null)
        {
            var msg = GetFormattedString(nameof(ErrorMessages.CallToUnsupportedMethod), methodName);
            return new NotSupportedException(msg, innerException);
        }

        public static ArgumentException ValuesNotFittingIntoArray(string argName, Exception innerException = null)
        {
            var msg = GetFormattedString(nameof(ErrorMessages.ValuesNotFittingIntoArray));
            return new ArgumentException(argName, msg, innerException);
        }
    }
}