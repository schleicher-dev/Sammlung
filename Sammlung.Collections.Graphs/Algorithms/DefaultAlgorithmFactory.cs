using System;
using Sammlung.Collections.Graphs.Algorithms.SCC;

namespace Sammlung.Collections.Graphs.Algorithms
{
    /// <summary>
    /// The <see cref="DefaultAlgorithmFactory"/> class exposes factory methods for the graph algorithms.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class DefaultAlgorithmFactory
    {
        /// <summary>
        /// Creates the default <see cref="ISccAlgorithm{T,TWeight}"/>.
        /// </summary>
        /// <typeparam name="T">the vertex type</typeparam>
        /// <typeparam name="TWeight">the edge weight</typeparam>
        /// <returns>the algorithm implementation instance</returns>
        public static ISccAlgorithm<T, TWeight> CreateStrongConnectednessAlgorithm<T, TWeight>()
            where TWeight : IComparable<TWeight>
            => new TarjanSccAlgorithm<T, TWeight>();
    }
}