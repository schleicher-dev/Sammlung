using System;
using System.Collections.Generic;

namespace Sammlung.Graphs.Algorithms.StronglyConnectedComponents
{
    /// <summary>
    /// The <see cref="IStronglyConnectedComponentsAlgorithm{T,TWeight}"/> exposes an interface for the calculation
    /// of strongly connected components of a graph.
    /// </summary>
    /// <typeparam name="T">the vertex type</typeparam>
    /// <typeparam name="TWeight">the edge weight type</typeparam>
    /// <remarks>SCC stands for strongly connected components</remarks>
    [JetBrains.Annotations.PublicAPI]
    public interface IStronglyConnectedComponentsAlgorithm<T, TWeight>
        where TWeight : IComparable<TWeight>
    {
        /// <summary>
        /// Calculates the strongly connected components of a graph.
        /// </summary>
        /// <returns>the strongly connected components</returns>
        IEnumerable<IDiGraph<T, TWeight>> CalculateComponents(IDiGraph<T, TWeight> graph);
    }
}