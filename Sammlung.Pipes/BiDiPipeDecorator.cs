namespace Sammlung.Pipes
{
    /// <summary>
    /// The <see cref="BiDiPipeDecorator{TSource,TTarget}"/> is the base class for all decorators of <see cref="IBiDiPipe{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public abstract class BiDiPipeDecorator<TSource, TTarget> : IBiDiPipe<TSource, TTarget>
    {
        /// <inheritdoc />
        public abstract TTarget ProcessForward(TSource input);
        
        /// <inheritdoc />
        public abstract TSource ProcessReverse(TTarget input);
    }
}