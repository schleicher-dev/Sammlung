using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Sammlung.Graphs
{
    [PublicAPI]
    public class DiGraph<TVertex, TWeight> : IDiGraph<TVertex, TWeight>
        where TWeight : IComparable<TWeight>
    {
        public DiGraph(TWeight defaultEdgeWeight)
        {
            DefaultEdgeWeight = defaultEdgeWeight;
            _vertices = new HashSet<TVertex>();
            _incomingEdges = new Dictionary<TVertex, HashSet<IEdge<TVertex, TWeight>>>();
            _outgoingEdges = new Dictionary<TVertex, HashSet<IEdge<TVertex, TWeight>>>();
        }

        public DiGraph(IDiGraph<TVertex, TWeight> graph) : this(graph.DefaultEdgeWeight)
        {
            _vertices = new HashSet<TVertex>(graph.Vertices);
            foreach (var edge in graph.Edges) AddEdge(edge.SourceVertex, edge.TargetVertex, edge.Weight);
        }

        private readonly HashSet<TVertex> _vertices;
        private readonly IDictionary<TVertex, HashSet<IEdge<TVertex, TWeight>>> _incomingEdges;
        private readonly IDictionary<TVertex, HashSet<IEdge<TVertex, TWeight>>> _outgoingEdges;

        /// <inheritdoc />
        public TWeight DefaultEdgeWeight { get; }

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => _vertices;

        /// <inheritdoc />
        public IEnumerable<IEdge<TVertex, TWeight>> Edges => _outgoingEdges.SelectMany(e => e.Value);

        /// <inheritdoc />
        public void AddVertex(TVertex vertex) => _vertices.Add(vertex);

        /// <inheritdoc />
        public bool HasVertex(TVertex vertex) => _vertices.Contains(vertex);

        /// <inheritdoc />
        public IEnumerable<IEdge<TVertex, TWeight>> GetIncomingEdges(TVertex vertex) =>
            _incomingEdges.TryGetValue(vertex, out var inEdges) ? inEdges : Enumerable.Empty<IEdge<TVertex, TWeight>>();

        /// <inheritdoc />
        public IEnumerable<IEdge<TVertex, TWeight>> GetOutgoingEdges(TVertex vertex) =>
            _outgoingEdges.TryGetValue(vertex, out var outEdges)
                ? outEdges
                : Enumerable.Empty<IEdge<TVertex, TWeight>>();

        /// <inheritdoc />
        public IEdge<TVertex, TWeight> AddEdge(TVertex source, TVertex target) =>
            AddEdge(source, target, DefaultEdgeWeight);

        /// <inheritdoc />
        public IEdge<TVertex, TWeight> AddEdge(TVertex source, TVertex target, TWeight weight)
        {
            IEdge<TVertex, TWeight> edge = new Edge<TVertex, TWeight>(source, target, weight);

            _vertices.Add(source);
            if (!_outgoingEdges.TryGetValue(source, out var outEdges))
                _outgoingEdges[source] = outEdges = new HashSet<IEdge<TVertex, TWeight>>();
            outEdges.Add(edge);

            _vertices.Add(target);
            if (!_incomingEdges.TryGetValue(target, out var inEdges))
                _incomingEdges[target] = inEdges = new HashSet<IEdge<TVertex, TWeight>>();
            inEdges.Add(edge);

            return edge;
        }
    }
}