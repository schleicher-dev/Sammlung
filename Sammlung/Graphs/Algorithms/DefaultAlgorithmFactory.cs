using System;
using System.Diagnostics.CodeAnalysis;
using Sammlung.Graphs.Algorithms.SCC;

namespace Sammlung.Graphs.Algorithms
{
    /// <summary>
    /// The <see cref="DefaultAlgorithmFactory"/> class exposes factory methods for the graph algorithms.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "PublicAPI")]
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