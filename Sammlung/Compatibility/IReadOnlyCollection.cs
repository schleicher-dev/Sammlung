namespace Sammlung.Compatibility
{
    /// <summary>
    /// The <see cref="IReadOnlyCollection{T}"/> interface is a read-only variant of a collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyCollection<out T> : System.Collections.Generic.IEnumerable<T>
    {
        /// <summary>
        /// Returns the number of elements in the collection.
        /// </summary>
        int Count { get; }
    }
}