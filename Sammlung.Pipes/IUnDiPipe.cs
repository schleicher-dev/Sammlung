namespace Sammlung.Pipes
{
    /// <summary>
    /// The <see cref="IUnDiPipe{TSource,TTarget}"/> type defines a interface which describes a unidirectional
    /// processing chain. The idea is that one may chain multiple pipes to one another to build a pipeline.
    /// These <see cref="IUnDiPipe{TSource,TTarget}"/> items may be chained together using concatenation.
    /// </summary>
    /// <remarks>
    /// { IUnDiPipe[int, float] } -Combine-> { IUnDiPipe[float, string] } -Yields-> { IUnDiPipe[int, string] }
    /// </remarks>
    /// <typeparam name="TSource">the source type</typeparam>
    /// <typeparam name="TTarget">the target type</typeparam>
    public interface IUnDiPipe<in TSource, out TTarget>
    {
        /// <summary>
        /// Process the pipeline in the forward direction.
        /// Which is the only way in an <see cref="IUnDiPipe{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="input" />
        /// <returns>true if the input could be processed else false</returns>
        TTarget Process(TSource input);
    }
}