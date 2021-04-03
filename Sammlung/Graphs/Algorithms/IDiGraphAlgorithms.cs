using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sammlung.Graphs.Algorithms.SCC;

namespace Sammlung.Graphs.Algorithms
{
    /// <summary>
    /// The <see cref="IDiGraphAlgorithms"/> interface exposes several computation algorithms for graphs.
    /// This interfaces follows the strategy design pattern.
    /// </summary>
    [PublicAPI]
    public interface IDiGraphAlgorithms
    {
        /// <summary>
        /// The <see cref="GetStronglyConnectedComponents{T,TWeight}"/> method calculates the strongly connected
        /// components of a graph using one of the well-known strongly connected components algorithms.
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the vertex type</typeparam>
        /// <typeparam name="TWeight">the weight type</typeparam>
        /// <returns>an array of strongly connected sub-graphs</returns>
        IEnumerable<IDiGraph<T, TWeight>> GetStronglyConnectedComponents<T, TWeight>(IDiGraph<T, TWeight> graph)
            where TWeight : IComparable<TWeight>;

        /// <summary>
        /// Indicates if the passed graph is strongly connected.
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the vertex type</typeparam>
        /// <typeparam name="TWeight">the edge weight type</typeparam>
        /// <returns>true if strongly connected else false</returns>
        /// <remarks>
        /// Think before you compute this twice. The graph is strongly connected iff it consists of one strongly
        /// connected component. This is, what is basically returned here, while destroying the result.
        /// </remarks>
        bool IsStronglyConnected<T, TWeight>(IDiGraph<T, TWeight> graph)
            where TWeight : IComparable<TWeight>;

        /// <summary>
        /// Returns all cyclical components of the passed graph.
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the vertex type</typeparam>
        /// <typeparam name="TWeight">the edge weight type</typeparam>
        /// <returns>every strongly connected component with more than one vertex or self-referencing vertices</returns>
        /// <remarks>
        /// A graph is cyclical iff there is a series of edges such that the start and the end vertex are the same.
        /// </remarks>
        IEnumerable<IDiGraph<T, TWeight>> GetCyclicalComponents<T, TWeight>(IDiGraph<T, TWeight> graph)
            where TWeight : IComparable<TWeight>;

        /// <summary>
        /// Checks if the passed graph is acyclic.
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the vertex type</typeparam>
        /// <typeparam name="TWeight">the edge weight type</typeparam>
        /// <returns>true if there is no cycle else false</returns>
        /// <remarks>
        /// A graph is acyclic iff there is no cyclical component in it.
        /// </remarks>
        bool IsAcyclic<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight>;
    }

    [PublicAPI]
    public class DefaultDiGraphAlgorithms : IDiGraphAlgorithms
    {
        /// <inheritdoc />
        public IEnumerable<IDiGraph<T, TWeight>> GetStronglyConnectedComponents<T, TWeight>(IDiGraph<T, TWeight> graph) 
            where TWeight : IComparable<TWeight>
        {
            IStronglyConnectedComponentsAlgorithm<T, TWeight> algorithm = 
                new TarjanStronglyConnectedComponentsAlgorithm<T, TWeight>(graph);
            return algorithm.Result;
        }
        
        /// <inheritdoc />
        public bool IsStronglyConnected<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight> => 
            GetStronglyConnectedComponents(graph).Count() == 1;

        private static bool IsCyclicSubGraph<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight>
            => 1 < graph.Vertices.Count() || graph.Vertices.Count() == 1 && graph.Edges.Any();

        /// <inheritdoc />
        public IEnumerable<IDiGraph<T, TWeight>> GetCyclicalComponents<T, TWeight>(IDiGraph<T, TWeight> graph) 
            where TWeight : IComparable<TWeight> =>
            GetStronglyConnectedComponents(graph).Where(IsCyclicSubGraph);

        /// <inheritdoc />
        public bool IsAcyclic<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight> => 
            !GetCyclicalComponents(graph).Any();
    }
}