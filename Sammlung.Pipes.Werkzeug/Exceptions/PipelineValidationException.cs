using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Sammlung.Pipes.Werkzeug.Exceptions
{
    /// <summary>
    /// The <see cref="PipelineValidationException"/> indicates an exception which occurred during a pipeline validation.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PipelineValidationException : Exception
    {
        /// <inheritdoc />
        public PipelineValidationException()
        {
        }

        /// <inheritdoc />
        public PipelineValidationException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public PipelineValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <inheritdoc />
        protected PipelineValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}