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
        /// <typeparam name="T1">the source type</typeparam>
        /// <typeparam name="T2">the target type</typeparam>
        /// <returns></returns>
        public static IBiDiPipe<T2, T1> Invert<T1, T2>(this IBiDiPipe<T1, T2> pipe) =>
            new InvertedBiDiPipe<T1, T2>(pipe);

        /// <summary>
        /// Takes the forward pipe from a <see cref="IBiDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="pipe">the pipe</param>
        /// <typeparam name="T1">the source type</typeparam>
        /// <typeparam name="T2">the target type</typeparam>
        /// <returns>the forward pipe</returns>
        public static IUnDiPipe<T1, T2> ForwardPipe<T1, T2>(this IBiDiPipe<T1, T2> pipe) =>
            new ProjectedForwardUnDiPipe<T1, T2>(pipe);

        /// <summary>
        /// Takes the reverse pipe from a <see cref="IBiDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="pipe">the pipe</param>
        /// <typeparam name="T1">the source type</typeparam>
        /// <typeparam name="T2">the target type</typeparam>
        /// <returns>the reverse pipe</returns>
        public static IUnDiPipe<T2, T1> ReversePipe<T1, T2>(this IBiDiPipe<T1, T2> pipe) =>
            new ProjectedReverseUnDiPipe<T1, T2>(pipe);

        /// <summary>
        /// Concatenates two <see cref="IBiDiPipe{TSource,TTarget}"/> together yielding a corresponding
        /// <see cref="IBiDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        /// <typeparam name="T1">the source type</typeparam>
        /// <typeparam name="T2">the intermediate type</typeparam>
        /// <typeparam name="T3">the target type</typeparam>
        /// <returns>the concatenated pipe</returns>
        public static IBiDiPipe<T1, T3> Concat<T1, T2, T3>(this IBiDiPipe<T1, T2> lhsPipe, IBiDiPipe<T2, T3> rhsPipe) =>
            new ConcatenatedBiDiPipe<T1, T2, T3>(lhsPipe, rhsPipe);

        /// <summary>
        /// Concatenates a <see cref="IBiDiPipe{TSource,TTarget}"/> and a <see cref="IUnDiPipe{TSource,TTarget}"/>
        /// together yielding a corresponding <see cref="IUnDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        /// <typeparam name="T1">the source type</typeparam>
        /// <typeparam name="T2">the intermediate type</typeparam>
        /// <typeparam name="T3">the target type</typeparam>
        /// <returns>the concatenated pipe</returns>
        public static IUnDiPipe<T1, T3> Concat<T1, T2, T3>(this IBiDiPipe<T1, T2> lhsPipe, IUnDiPipe<T2, T3> rhsPipe) =>
            new ConcatenatedUnDiPipe<T1, T2, T3>(lhsPipe.ForwardPipe(), rhsPipe);

        /// <summary>
        /// Concatenates two <see cref="IUnDiPipe{TSource,TTarget}"/> together yielding a corresponding
        /// <see cref="IUnDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        /// <typeparam name="T1">the source type</typeparam>
        /// <typeparam name="T2">the intermediate type</typeparam>
        /// <typeparam name="T3">the target type</typeparam>
        /// <returns>the concatenated pipe</returns>
        public static IUnDiPipe<T1, T3> Concat<T1, T2, T3>(this IUnDiPipe<T1, T2> lhsPipe, IUnDiPipe<T2, T3> rhsPipe) =>
            new ConcatenatedUnDiPipe<T1, T2, T3>(lhsPipe, rhsPipe);

        /// <summary>
        /// Concatenates a <see cref="IUnDiPipe{TSource,TTarget}"/> and a <see cref="IBiDiPipe{TSource,TTarget}"/>
        /// together yielding a corresponding <see cref="IUnDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        /// <typeparam name="T1">the source type</typeparam>
        /// <typeparam name="T2">the intermediate type</typeparam>
        /// <typeparam name="T3">the target type</typeparam>
        /// <returns>the concatenated pipe</returns>
        public static IUnDiPipe<T1, T3> Concat<T1, T2, T3>(this IUnDiPipe<T1, T2> lhsPipe, IBiDiPipe<T2, T3> rhsPipe) =>
            new ConcatenatedUnDiPipe<T1, T2, T3>(lhsPipe, rhsPipe.ForwardPipe());

        /// <summary>
        /// Combines two <see cref="IUnDiPipe{TSource,TTarget}"/> to form a <see cref="IBiDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="fwdPipe">the pipe for the forward direction</param>
        /// <param name="revPipe">the pipe for the reverse direction</param>
        /// <typeparam name="T1">the source type</typeparam>
        /// <typeparam name="T2">the target type</typeparam>
        /// <returns>the combined pipe</returns>
        public static IBiDiPipe<T1, T2> CreateBiDiPipe<T1, T2>
            (this IUnDiPipe<T1, T2> fwdPipe, IUnDiPipe<T2, T1> revPipe) =>
            new CompositeBiDiPipe<T1, T2>(fwdPipe, revPipe);
    }
}