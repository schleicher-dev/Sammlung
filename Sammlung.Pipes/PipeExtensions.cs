using Sammlung.Pipes.SpecialPipes;

namespace Sammlung.Pipes
{
    /// <summary>
    /// The <see cref="PipeExtensions"/> defines a set of extension methods to use with
    /// <see cref="IUnDiPipe{TSource,TTarget}"/> and <see cref="IBiDiPipe{TSource,TTarget}"/> types.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class PipeExtensions
    {
        /// <summary>
        /// Takes the <see cref="IBiDiPipe{TSource,TTarget}"/> and inverses the processing operations.
        /// </summary>
        /// <param name="pipe">the pipe</param>
        /// <typeparam name="TSource">the source type</typeparam>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <returns></returns>
        public static IBiDiPipe<TTarget, TSource> Invert<TSource, TTarget>(this IBiDiPipe<TSource, TTarget> pipe) =>
            new InvertedBiDiPipe<TSource, TTarget>(pipe);

        /// <summary>
        /// Takes the forward pipe from a <see cref="IBiDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="pipe">the pipe</param>
        /// <typeparam name="TSource">the source type</typeparam>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <returns>the forward pipe</returns>
        public static IUnDiPipe<TSource, TTarget>
            ForwardPipe<TSource, TTarget>(this IBiDiPipe<TSource, TTarget> pipe) =>
            new ProjectedForwardUnDiPipe<TSource, TTarget>(pipe);
        
        /// <summary>
        /// Takes the reverse pipe from a <see cref="IBiDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="pipe">the pipe</param>
        /// <typeparam name="TSource">the source type</typeparam>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <returns>the reverse pipe</returns>
        public static IUnDiPipe<TTarget, TSource>
            ReversePipe<TSource, TTarget>(this IBiDiPipe<TSource, TTarget> pipe) =>
            new ProjectedReverseUnDiPipe<TSource, TTarget>(pipe);

        /// <summary>
        /// Concatenates two <see cref="IBiDiPipe{TSource,TTarget}"/> together yielding a corresponding
        /// <see cref="IBiDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        /// <typeparam name="TSource">the source type</typeparam>
        /// <typeparam name="TIntermediate">the intermediate type</typeparam>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <returns>the concatenated pipe</returns>
        public static IBiDiPipe<TSource, TTarget> Concat<TSource, TIntermediate, TTarget>
            (this IBiDiPipe<TSource, TIntermediate> lhsPipe, IBiDiPipe<TIntermediate, TTarget> rhsPipe) =>
            new ConcatenatedBiDiPipe<TSource, TIntermediate, TTarget>(lhsPipe, rhsPipe);
        
        /// <summary>
        /// Concatenates a <see cref="IBiDiPipe{TSource,TTarget}"/> and a <see cref="IUnDiPipe{TSource,TTarget}"/>
        /// together yielding a corresponding <see cref="IUnDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        /// <typeparam name="TSource">the source type</typeparam>
        /// <typeparam name="TIntermediate">the intermediate type</typeparam>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <returns>the concatenated pipe</returns>
        public static IUnDiPipe<TSource, TTarget> Concat<TSource, TIntermediate, TTarget>
            (this IBiDiPipe<TSource, TIntermediate> lhsPipe, IUnDiPipe<TIntermediate, TTarget> rhsPipe) =>
            new ConcatenatedUnDiPipe<TSource, TIntermediate, TTarget>(lhsPipe.ForwardPipe(), rhsPipe);

        /// <summary>
        /// Concatenates two <see cref="IUnDiPipe{TSource,TTarget}"/> together yielding a corresponding
        /// <see cref="IUnDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        /// <typeparam name="TSource">the source type</typeparam>
        /// <typeparam name="TIntermediate">the intermediate type</typeparam>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <returns>the concatenated pipe</returns>
        public static IUnDiPipe<TSource, TTarget> Concat<TSource, TIntermediate, TTarget>
            (this IUnDiPipe<TSource, TIntermediate> lhsPipe, IUnDiPipe<TIntermediate, TTarget> rhsPipe) =>
            new ConcatenatedUnDiPipe<TSource, TIntermediate, TTarget>(lhsPipe, rhsPipe);
        
        /// <summary>
        /// Concatenates a <see cref="IUnDiPipe{TSource,TTarget}"/> and a <see cref="IBiDiPipe{TSource,TTarget}"/>
        /// together yielding a corresponding <see cref="IUnDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        /// <typeparam name="TSource">the source type</typeparam>
        /// <typeparam name="TIntermediate">the intermediate type</typeparam>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <returns>the concatenated pipe</returns>
        public static IUnDiPipe<TSource, TTarget> Concat<TSource, TIntermediate, TTarget>
            (this IUnDiPipe<TSource, TIntermediate> lhsPipe, IBiDiPipe<TIntermediate, TTarget> rhsPipe) =>
            new ConcatenatedUnDiPipe<TSource, TIntermediate, TTarget>(lhsPipe, rhsPipe.ForwardPipe());
        
        /// <summary>
        /// Combines two <see cref="IUnDiPipe{TSource,TTarget}"/> to form a <see cref="IBiDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="fwdPipe">the pipe for the forward direction</param>
        /// <param name="revPipe">the pipe for the reverse direction</param>
        /// <typeparam name="TSource">the source type</typeparam>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <returns>the combined pipe</returns>
        public static IBiDiPipe<TSource, TTarget> Combine<TSource, TTarget>
            (this IUnDiPipe<TSource, TTarget> fwdPipe, IUnDiPipe<TTarget, TSource> revPipe) =>
            new CompositeBiDiPipe<TSource, TTarget>(fwdPipe, revPipe);
    }
}