using System;
using JetBrains.Annotations;
using Sammlung.Graphs.Algorithms.SCC;

namespace Sammlung.Graphs.Algorithms
{
    /// <summary>
    /// The <see cref="DefaultAlgorithmFactory"/> class exposes factory methods for the graph algorithms.
    /// </summary>
    [PublicAPI]
    public static class DefaultAlgorithmFactory
    {
        /// <summary>
        /// Creates the default <see cref="IStrongConnectednessAlgorithm{TVertex,TWeight}"/>.
        /// </summary>
        /// <typeparam name="T">the vertex type</typeparam>
        /// <typeparam name="TWeight">the edge weight</typeparam>
        /// <returns>the algorithm implementation instance</returns>
        public static IStrongConnectednessAlgorithm<T, TWeight> CreateStrongConnectednessAlgorithm<T, TWeight>()
            where TWeight : IComparable<TWeight>
            => new TarjanStrongConnectednessAlgorithm<T, TWeight>();
    }
}