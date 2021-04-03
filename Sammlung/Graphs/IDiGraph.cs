using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sammlung.Graphs
{
    [PublicAPI]
    public interface IDiGraph<TVertex, TWeight> 
        where TWeight : IComparable<TWeight>
    {
        public TWeight DefaultEdgeWeight { get; }
        public IEnumerable<TVertex> Vertices { get; }
        public IEnumerable<IEdge<TVertex, TWeight>> Edges { get; }
        public void AddVertex(TVertex vertex);
        public bool HasVertex(TVertex vertex);
        public IEnumerable<IEdge<TVertex, TWeight>> GetIncomingEdges(TVertex vertex);
        public IEnumerable<IEdge<TVertex, TWeight>> GetOutgoingEdges(TVertex vertex);
        public IEdge<TVertex, TWeight> AddEdge(TVertex source, TVertex target);
        public IEdge<TVertex, TWeight> AddEdge(TVertex source, TVertex target, TWeight weight);
    }
}