using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Graphs.Algorithms.StronglyConnectedComponents;
using Sammlung.Werkzeug;

namespace Sammlung.Graphs.Algorithms
{
    /// <summary>
    /// The <see cref="DiGraphExtensions"/> type extends the <see cref="IDiGraph{T,TWeight}"/> with useful method
    /// shorthands and helpful default implementations of algorithms.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class DiGraphExtensions
    {
        /// <summary>
        /// Creates a <see cref="IStronglyConnectedComponentsAlgorithm{T,TWeight}"/> from the passed graph.
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the node type</typeparam>
        /// <typeparam name="TWeight">the weight type</typeparam>
        /// <returns>the algorithm</returns>
        public static IStronglyConnectedComponentsAlgorithm<T, TWeight>
            CreateStronglyConnectedComponentsAlgorithm<T, TWeight>(this IDiGraph<T, TWeight> graph)
            where TWeight : IComparable<TWeight> =>
            new TarjanStronglyConnectedComponentsAlgorithm<T, TWeight>();

        /// <summary>
        /// Calculates all strongly connected components of the graph and returns them as <see cref="IDiGraph{T,TWeight}"/>.
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the node type</typeparam>
        /// <typeparam name="TWeight">the weight type</typeparam>
        /// <returns>the components</returns>
        public static IEnumerable<IDiGraph<T, TWeight>> GetStronglyConnectedComponents<T, TWeight>(this IDiGraph<T, TWeight> graph) 
            where TWeight : IComparable<TWeight> =>
            graph.CreateStronglyConnectedComponentsAlgorithm().CalculateComponents(graph.RequireNotNull(nameof(graph)));

        /// <summary>
        /// Calculates all strongly connected components of the graph and checks if there is only one component,
        /// which therefore indicates, that the graph is strongly connected. 
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the node type</typeparam>
        /// <typeparam name="TWeight">the weight type</typeparam>
        /// <returns>true if strongly connected else false</returns>
        public static bool IsStronglyConnected<T, TWeight>(this IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight> => 
            GetStronglyConnectedComponents(graph.RequireNotNull(nameof(graph))).Count() == 1;

        private static bool IsCyclicSubGraph<T, TWeight>(this IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight>
        {
            graph = graph.RequireNotNull(nameof(graph));
            return 1 < graph.Vertices.Count() || graph.Vertices.Count() == 1 && graph.Edges.Any();
        }

        /// <summary>
        /// Calculates the strongly connected components of the graph and returns only cyclic components.
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the node type</typeparam>
        /// <typeparam name="TWeight">the weight type</typeparam>
        /// <returns>the cyclic components</returns>
        public static IEnumerable<IDiGraph<T, TWeight>> GetCyclicalComponents<T, TWeight>(this IDiGraph<T, TWeight> graph) 
            where TWeight : IComparable<TWeight> =>
            GetStronglyConnectedComponents(graph.RequireNotNull(nameof(graph))).Where(IsCyclicSubGraph);

        /// <summary>
        /// Checks if the graph is acyclic.
        /// </summary>
        /// <param name="graph">the graph</param>
        /// <typeparam name="T">the node type</typeparam>
        /// <typeparam name="TWeight">the weight type</typeparam>
        /// <returns>true if cyclic else false</returns>
        public static bool IsAcyclic<T, TWeight>(this IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight> => 
            !GetCyclicalComponents(graph.RequireNotNull(nameof(graph))).Any();
    }
}