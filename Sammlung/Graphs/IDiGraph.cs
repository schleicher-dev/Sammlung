using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sammlung.Graphs
{
    /// <summary>
    /// The <see cref="IDiGraph{TVertex,TWeight}"/> interface is an abstract contract for an directed graph.
    /// </summary>
    /// <typeparam name="T">the vertex type</typeparam>
    /// <typeparam name="TWeight">the edge weight type</typeparam>
    [PublicAPI]
    public interface IDiGraph<T, TWeight> 
        where TWeight : IComparable<TWeight>
    {
        /// <summary>
        /// Holds the default edge weight assigned to edges without edge weight assignment.
        /// </summary>
        public TWeight DefaultEdgeWeight { get; }
        
        /// <summary>
        /// The vertices of the graph.
        /// </summary>
        public IEnumerable<T> Vertices { get; }
        
        /// <summary>
        /// The edges of the graph.
        /// </summary>
        public IEnumerable<IEdge<T, TWeight>> Edges { get; }
        
        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="vertex">the vertex</param>
        public void AddVertex([NotNull] T vertex);
        
        /// <summary>
        /// Checks if the vertex is part of the graph.
        /// </summary>
        /// <param name="vertex">the vertex</param>
        /// <returns>true if vertex is part of the graph else false</returns>
        public bool HasVertex([NotNull] T vertex);
        
        /// <summary>
        /// Gets the incoming edges to the passed vertex.
        /// </summary>
        /// <param name="vertex">the vertex</param>
        /// <returns>the incoming edges</returns>
        public IEnumerable<IEdge<T, TWeight>> GetIncomingEdges([NotNull] T vertex);
        
        /// <summary>
        /// Gets the outgoing edges from the passed vertex.
        /// </summary>
        /// <param name="vertex">the vertex</param>
        /// <returns>the outgoing edges</returns>
        public IEnumerable<IEdge<T, TWeight>> GetOutgoingEdges([NotNull] T vertex);
        
        /// <summary>
        /// Adds an edge with the default weight to the <see cref="IDiGraph{T,TWeight}"/>.
        /// </summary>
        /// <param name="source">the source vertex</param>
        /// <param name="target">the target vertex</param>
        /// <returns>the created edge</returns>
        public IEdge<T, TWeight> AddEdge([NotNull] T source, [NotNull] T target);

        /// <summary>
        /// Adds an edge with the passed weight to the <see cref="IDiGraph{T,TWeight}"/>.
        /// </summary>
        /// <param name="source">the source vertex</param>
        /// <param name="target">the target vertex</param>
        /// <param name="weight">the weight</param>
        /// <returns>the created edge</returns>
        public IEdge<T, TWeight> AddEdge([NotNull] T source, [NotNull] T target, [NotNull] TWeight weight);

        /// <summary>
        /// Returns if the edge with the given source and target vertex - ignoring the weight - is known to the graph.
        /// </summary>
        /// <param name="source">the source vertex</param>
        /// <param name="target">the target vertex</param>
        /// <returns>true if edge was found else false</returns>
        public bool HasEdge([NotNull] T source, [NotNull] T target);
    }
}