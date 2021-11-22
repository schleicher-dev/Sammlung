using Sammlung.Queues.Concurrent;

namespace Sammlung.Queues
{
    /// <summary>
    /// The <see cref="BlockingDequeExtensions"/> is a static class which can decorate a <see cref="IDeque{T}"/>.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class BlockingDequeExtensions
    {
        public static IDeque<T> Wrap<T>(this IDeque<T> inner) => new BlockingDeque<T>(inner);
    }
}