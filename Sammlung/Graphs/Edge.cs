using System;
using System.Collections.Generic;

namespace Sammlung.Graphs
{
    public class Edge<TVertex, TWeight> : IEdge<TVertex, TWeight>, IEquatable<Edge<TVertex, TWeight>>
    {
        public Edge(TVertex sourceVertex, TVertex targetVertex, TWeight weight)
        {
            SourceVertex = sourceVertex;
            TargetVertex = targetVertex;
            Weight = weight;
        }
        
        public TVertex SourceVertex { get; }
        public TVertex TargetVertex { get; }
        public TWeight Weight { get; }

        public override string ToString()
        {
            return string.Format("{0}.{1}[Source={2}, Target={3}, Weight={4}]",
                GetType().Namespace, GetType().Name, SourceVertex?.ToString(), TargetVertex?.ToString(), Weight?.ToString());
        }

        public bool Equals(Edge<TVertex, TWeight> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TVertex>.Default.Equals(SourceVertex, other.SourceVertex) && 
                   EqualityComparer<TVertex>.Default.Equals(TargetVertex, other.TargetVertex) && 
                   EqualityComparer<TWeight>.Default.Equals(Weight, other.Weight);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge<TVertex, TWeight>) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceVertex, TargetVertex, Weight);
        }
    }
}