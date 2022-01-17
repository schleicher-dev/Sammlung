using Sammlung.Collections.Queues.Concurrent;

namespace Sammlung.Collections.Queues
{
    /// <summary>
    /// The <see cref="BlockingDequeExtensions"/> is a static class which can decorate a <see cref="IDeque{T}"/>.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class BlockingDequeExtensions
    {
        public static IDeque<T> ToBlockingDeque<T>(this IDeque<T> inner) => new BlockingDeque<T>(inner);
    }
}