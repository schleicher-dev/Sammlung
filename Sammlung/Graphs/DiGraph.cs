using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sammlung.Compatibility;

namespace Sammlung.Graphs
{

    /// <summary>
    /// The <see cref="DiGraph{T}"/> represents an directed graph.
    /// </summary>
    [PublicAPI]
    public class DiGraph<T> where T : IEquatable<T>
    {
        private readonly IDictionary<T, Node<T>> _nodeMapping;

        public DiGraph()
        {
            _nodeMapping = new Dictionary<T, Node<T>>();
        }

        public Compatibility.IReadOnlyCollection<Node<T>> Nodes =>
            new ReadOnlyCollectionAdapter<Node<T>>(_nodeMapping.Values);

        public Compatibility.IReadOnlyCollection<Edge<T>> Edges =>
            new ReadOnlyCollectionAdapter<Edge<T>>(_nodeMapping.Values.SelectMany(n => n.OutgoingEdges).ToList());

        public Node<T> AddNode(T value) => 
            _nodeMapping.TryGetValue(value, out var node) ? node : _nodeMapping[value] = new Node<T>(value);

        public IEnumerable<Node<T>> AddNodes(IEnumerable<T> values) => values.Select(AddNode).ToList();
        
        public bool TryGetNode(T value, out Node<T> node) => _nodeMapping.TryGetValue(value, out node);

        public Edge<T> AddEdge(T sourceValue, T targetValue) => AddEdge((sourceValue, targetValue));
        
        public Edge<T> AddEdge((T SourceValue, T TargetValue) edge)
        {
            var (sourceValue, targetValue) = edge;
            var sourceNode = AddNode(sourceValue);
            var targetNode = AddNode(targetValue); 
            sourceNode.AddTargetNode(targetNode);
            return new Edge<T>(sourceNode, targetNode);
        }

        public IEnumerable<Edge<T>> AddEdges(IEnumerable<(T SourceValue, T TargetValue)> edges) =>
            edges.Select(AddEdge).ToList();
    }
}