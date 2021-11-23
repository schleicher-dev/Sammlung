using System;
using System.Collections.Generic;

namespace Sammlung.Collections.Graphs
{
    /// <summary>
    /// The <see cref="Edge{TVertex,TWeight}"/> type implements the <seealso cref="IEdge{TVertex,TWeight}"/> interface
    /// which represents a directed edge.
    /// </summary>
    /// <typeparam name="T">the vertex type</typeparam>
    /// <typeparam name="TWeight">the edge weight</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class Edge<T, TWeight> : IEdge<T, TWeight>, IEquatable<Edge<T, TWeight>>
    {
        /// <summary>
        /// Creates a new <see cref="Edge{TVertex,TWeight}"/> using two vertices and a weight.
        /// </summary>
        /// <param name="sourceVertex">the source vertex</param>
        /// <param name="targetVertex">the target vertex</param>
        /// <param name="weight"></param>
        public Edge(T sourceVertex, T targetVertex, TWeight weight)
        {
            SourceVertex = sourceVertex;
            TargetVertex = targetVertex;
            Weight = weight;
        }
        
        /// <inheritdoc />
        public T SourceVertex { get; }
        
        /// <inheritdoc />
        public T TargetVertex { get; }
        
        /// <inheritdoc />
        public TWeight Weight { get; }

        /// <inheritdoc />
        public override string ToString() => 
            $"{GetType().Namespace}.{GetType().Name}[Source={SourceVertex?.ToString()}, Target={TargetVertex?.ToString()}, Weight={Weight?.ToString()}]";

        /// <inheritdoc />
        public bool Equals(Edge<T, TWeight> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(SourceVertex, other.SourceVertex) && 
                   EqualityComparer<T>.Default.Equals(TargetVertex, other.TargetVertex) && 
                   EqualityComparer<TWeight>.Default.Equals(Weight, other.Weight);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Edge<T, TWeight>) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                hashCode ^= SourceVertex.GetHashCode() * 31;
                hashCode ^= TargetVertex.GetHashCode() * 31;
                hashCode ^= Weight.GetHashCode() * 31;
            }
            return hashCode;
        }
    }
}