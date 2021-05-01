using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sammlung.Graphs
{
    /// <summary>
    /// The <see cref="IDiGraph{TVertex,TWeight}"/> interface is an abstract contract for an directed graph.
    /// </summary>
    /// <typeparam name="T">the vertex type</typeparam>
    /// <typeparam name="TWeight">the edge weight type</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "PublicAPI")]
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
        public void AddVertex(T vertex);
        
        /// <summary>
        /// Checks if the vertex is part of the graph.
        /// </summary>
        /// <param name="vertex">the vertex</param>
        /// <returns>true if vertex is part of the graph else false</returns>
        public bool HasVertex(T vertex);
        
        /// <summary>
        /// Gets the incoming edges to the passed vertex.
        /// </summary>
        /// <param name="vertex">the vertex</param>
        /// <returns>the incoming edges</returns>
        public IEnumerable<IEdge<T, TWeight>> GetIncomingEdges(T vertex);
        
        /// <summary>
        /// Gets the outgoing edges from the passed vertex.
        /// </summary>
        /// <param name="vertex">the vertex</param>
        /// <returns>the outgoing edges</returns>
        public IEnumerable<IEdge<T, TWeight>> GetOutgoingEdges(T vertex);
        
        /// <summary>
        /// Adds an edge with the default weight to the <see cref="IDiGraph{T,TWeight}"/>.
        /// </summary>
        /// <param name="source">the source vertex</param>
        /// <param name="target">the target vertex</param>
        /// <returns>the created edge</returns>
        public IEdge<T, TWeight> AddEdge(T source, T target);

        /// <summary>
        /// Adds an edge with the passed weight to the <see cref="IDiGraph{T,TWeight}"/>.
        /// </summary>
        /// <param name="source">the source vertex</param>
        /// <param name="target">the target vertex</param>
        /// <param name="weight">the weight</param>
        /// <returns>the created edge</returns>
        public IEdge<T, TWeight> AddEdge(T source, T target, TWeight weight);

        /// <summary>
        /// Returns if the edge with the given source and target vertex - ignoring the weight - is known to the graph.
        /// </summary>
        /// <param name="source">the source vertex</param>
        /// <param name="target">the target vertex</param>
        /// <returns>true if edge was found else false</returns>
        public bool HasEdge(T source, T target);
    }
}