using System;
using System.Diagnostics;

namespace Sammlung.Pipes
{
    /// <summary>
    /// The <see cref="IBiDiPipe{TSource,TTarget}"/> type defines a interface which describes a bidirectional
    /// processing chain. The idea is that one may chain multiple pipes to one another to build a pipeline.
    /// These <see cref="IBiDiPipe{TSource,TTarget}"/> items may be chained together using concatenation.
    /// </summary>
    /// <remarks>
    /// { IBiDiPipe[int, float] } -Combine-> { IBiDiPipe[float, string] } -Yields-> { IBiDiPipe[int, string] }
    /// </remarks>
    /// <typeparam name="TSource">the source type</typeparam>
    /// <typeparam name="TTarget">the target type</typeparam>
    public interface IBiDiPipe<TSource, TTarget>
    {
        /// <summary>
        /// Process the pipeline in the forward direction.
        /// </summary>
        /// <param name="input" />
        /// <returns>true if the input could be processed else false</returns>
        TTarget ProcessForward(TSource input);

        /// <summary>
        /// Process the pipeline in the reverse direction.
        /// </summary>
        /// <param name="input" />
        /// <returns>true if the input could be processed else false</returns>
        TSource ProcessReverse(TTarget input);
    }
}