using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Sammlung.CommandLine.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class GenericException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GenericException()
        {
        }

        public GenericException(string message) : base(message)
        {
        }

        public GenericException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GenericException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}