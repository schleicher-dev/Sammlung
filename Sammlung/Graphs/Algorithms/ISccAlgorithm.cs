using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sammlung.Graphs.Algorithms
{
    /// <summary>
    /// The <see cref="ISccAlgorithm{T,TWeight}"/> exposes an interface for the calculation
    /// of strongly connected components of a graph.
    /// </summary>
    /// <typeparam name="T">the vertex type</typeparam>
    /// <typeparam name="TWeight">the edge weight type</typeparam>
    /// <remarks>SCC stands for strongly connected components</remarks>
    [JetBrains.Annotations.PublicAPI]
    public interface ISccAlgorithm<T, TWeight>
        where TWeight : IComparable<TWeight>
    {
        /// <summary>
        /// Calculates the strongly connected components of a graph.
        /// </summary>
        /// <returns>the strongly connected components</returns>
        IEnumerable<IDiGraph<T, TWeight>> CalculateComponents(IDiGraph<T, TWeight> graph);
    }
}