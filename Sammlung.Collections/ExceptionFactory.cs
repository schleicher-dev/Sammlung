using System;
using System.Globalization;
using Sammlung.Collections.Resources;
using Sammlung.Werkzeug;

namespace Sammlung.Collections
{
    internal static class ExceptionFactory
    {
        public static InvalidOperationException NewEmptyCollectionException(Exception innerException = null) => 
            new InvalidOperationException(ErrorMessages.EmptyCollectionError, innerException);

        public static ArgumentException NewElementNotFoundException<T>(T element, string paramName, Exception innerException = null)
        {
            var msg = string.Format(ErrorMessages.ElementNotFoundError, element);
            return new ArgumentException(msg, paramName, innerException);
        }
    }
}