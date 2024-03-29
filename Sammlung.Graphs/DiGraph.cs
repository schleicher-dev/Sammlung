using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Werkzeug;

namespace Sammlung.Graphs
{
    /// <summary>
    /// The <see cref="DiGraph{TVertex,TWeight}"/> class is an implementation of a directed weighted graph.
    /// </summary>
    /// <typeparam name="T">the vertex type</typeparam>
    /// <typeparam name="TWeight">the edge weight type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class DiGraph<T, TWeight> : IDiGraph<T, TWeight>
        where TWeight : IComparable<TWeight>
    {
        /// <summary>
        /// Creates a new <see cref="DiGraph{T,TWeight}"/> using the default edge weight.
        /// </summary>
        /// <param name="defaultEdgeWeight">the default edge weight</param>
        public DiGraph(TWeight defaultEdgeWeight)
        {
            DefaultEdgeWeight = defaultEdgeWeight;
            _vertices = new HashSet<T>();
            _incomingEdges = new Dictionary<T, HashSet<IEdge<T, TWeight>>>();
            _outgoingEdges = new Dictionary<T, HashSet<IEdge<T, TWeight>>>();
        }

        /// <summary>
        /// Creates a new <see cref="DiGraph{T,TWeight}"/> using an existing graph.
        /// </summary>
        /// <param name="graph">the existing graph</param>
        public DiGraph(IDiGraph<T, TWeight> graph) : this(graph.DefaultEdgeWeight)
        {
            graph = graph.RequireNotNull(nameof(graph));
            _vertices = new HashSet<T>(graph.Vertices);
            foreach (var edge in graph.Edges) AddEdge(edge.SourceVertex, edge.TargetVertex, edge.Weight);
        }

        private readonly HashSet<T> _vertices;
        private readonly IDictionary<T, HashSet<IEdge<T, TWeight>>> _incomingEdges;
        private readonly IDictionary<T, HashSet<IEdge<T, TWeight>>> _outgoingEdges;

        /// <inheritdoc />
        public TWeight DefaultEdgeWeight { get; }

        /// <inheritdoc />
        public IEnumerable<T> Vertices => _vertices;

        /// <inheritdoc />
        public IEnumerable<IEdge<T, TWeight>> Edges => _outgoingEdges.SelectMany(e => e.Value);

        /// <inheritdoc />
        public void AddVertex(T vertex) => _vertices.Add(vertex);

        /// <inheritdoc />
        public bool HasVertex(T vertex) => _vertices.Contains(vertex);

        /// <inheritdoc />
        public IEnumerable<IEdge<T, TWeight>> GetIncomingEdges(T vertex) =>
            _incomingEdges.TryGetValue(vertex, out var inEdges) ? inEdges : Enumerable.Empty<IEdge<T, TWeight>>();

        /// <inheritdoc />
        public IEnumerable<IEdge<T, TWeight>> GetOutgoingEdges(T vertex) =>
            _outgoingEdges.TryGetValue(vertex, out var outEdges)
                ? outEdges
                : Enumerable.Empty<IEdge<T, TWeight>>();

        private static void InternalAddEdge(HashSet<T> vertices, IDictionary<T, HashSet<IEdge<T, TWeight>>> edgeLookup, T vertex, IEdge<T, TWeight> edge)
        {
            vertices.Add(vertex);
            if (!edgeLookup.TryGetValue(vertex, out var edges))
                edgeLookup[vertex] = edges = new HashSet<IEdge<T, TWeight>>();
            edges.Add(edge);
        }

        /// <inheritdoc />
        public IEdge<T, TWeight> AddEdge(T source, T target) =>
            AddEdge(source, target, DefaultEdgeWeight);

        /// <inheritdoc />
        public IEdge<T, TWeight> AddEdge(T source, T target, TWeight weight)
        {
            IEdge<T, TWeight> edge = new Edge<T, TWeight>(source, target, weight);

            InternalAddEdge(_vertices, _outgoingEdges, source, edge);
            InternalAddEdge(_vertices, _incomingEdges, target, edge);

            return edge;
        }

        /// <inheritdoc />
        public bool HasEdge(T source, T target) => 
            GetOutgoingEdges(source).Any(e => EqualityComparer<T>.Default.Equals(target, e.TargetVertex));
    }
}