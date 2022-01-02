namespace Sammlung.Pipes.SpecialPipes
{
    /// <summary>
    /// The <see cref="IdentityPipe{T}"/> does absolutely nothing, but may be used as seed point for actions
    /// which need an initial pipeline of a particular type.
    /// </summary>
    /// <typeparam name="T">the type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class IdentityPipe<T> : IBiDiPipe<T, T>
    {
        /// <inheritdoc />
        public T ProcessForward(T input) => input;

        /// <inheritdoc />
        public T ProcessReverse(T input) => input;
    }
}