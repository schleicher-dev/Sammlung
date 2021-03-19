using System;
using JetBrains.Annotations;

namespace Sammlung.Graphs
{
    [PublicAPI]
    public class Edge<T> : IEquatable<Edge<T>> where T : IEquatable<T>
    {
        public Edge([System.Diagnostics.CodeAnalysis.NotNull] Node<T> sourceNode, [System.Diagnostics.CodeAnalysis.NotNull] Node<T> targetNode)
        {
            SourceNode = sourceNode;
            TargetNode = targetNode;
        }

        public Node<T> SourceNode { get; }
        public Node<T> TargetNode { get; }

        /// <inheritdoc />
        public bool Equals(Edge<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(SourceNode, other.SourceNode) && Equals(TargetNode, other.TargetNode);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge<T>) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(SourceNode, TargetNode);

        public static bool operator ==(Edge<T> left, Edge<T> right) => Equals(left, right);

        public static bool operator !=(Edge<T> left, Edge<T> right) => !Equals(left, right);
    }
}