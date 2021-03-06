using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Sammlung.Exceptions
{
    /// <summary>
    /// The <see cref="DuplicateKeyException"/> indicates that a duplicate key was found in a mapping where it is not
    /// allowed.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "PublicAPI")]
    [Serializable]
    public class DuplicateKeyException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="DuplicateKeyException"/>.
        /// </summary>
        public DuplicateKeyException() { }
        
        /// <summary>
        /// Creates a new <see cref="DuplicateKeyException"/> using a message.
        /// </summary>
        /// <param name="message">the message</param>
        public DuplicateKeyException(string message) : base(message) { }
        
        /// <summary>
        /// Creates a new <see cref="DuplicateKeyException"/> using a message and an inner exception.
        /// </summary>
        /// <param name="message">the message</param>
        /// <param name="inner">the inner exception</param>
        public DuplicateKeyException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Creates a new <see cref="DuplicateKeyException"/> using serialization.
        /// </summary>
        /// <param name="info">the serialization info</param>
        /// <param name="context">the streaming context</param>
        protected DuplicateKeyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}