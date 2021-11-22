using Sammlung.CircularBuffers.Concurrent;

namespace Sammlung.CircularBuffers
{
    /// <summary>
    /// The <see cref="CircularBufferExtensions"/> extends any <see cref="ICircularBuffer{T}"/> with useful methods.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class CircularBufferExtensions
    {
        /// <summary>
        /// Decorates a <see cref="ICircularBuffer{T}"/> as a <see cref="BlockingCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="decorated">the heap to be decorated</param>
        /// <typeparam name="T">the type</typeparam>
        /// <returns>the blocking heap</returns>
        public static BlockingCircularBuffer<T> Wrap<T>(this ICircularBuffer<T> decorated)
            => new BlockingCircularBuffer<T>(decorated);
    }
}