namespace Sammlung.CommandLine.Models.Traits
{
    /// <summary>
    /// The <see cref="IBindableTrait{T}"/> type implemented on an entity means, that the particular type can be
    /// bound to the object.
    /// </summary>
    /// <typeparam name="T">the type to bind</typeparam>
    public interface IBindableTrait<in T>
    {
        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="data" />
        void Bind(T data);
    }
}