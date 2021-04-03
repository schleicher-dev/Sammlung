using System;
using System.Runtime.Serialization;

namespace Sammlung.Exceptions
{
    [Serializable]
    public class VertexNotFoundException : Exception
    {
        public VertexNotFoundException()
        {
        }

        public VertexNotFoundException(string message) : base(message)
        {
        }

        public VertexNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected VertexNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}