using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sammlung.Compatibility;

namespace Sammlung.Graphs
{
    [PublicAPI]
    public class Node<T> : IEquatable<Node<T>> where T : IEquatable<T>
    {
        private readonly ISet<Node<T>> _incomingNodes;
        private readonly ISet<Node<T>> _outgoingNodes;

        public Node([System.Diagnostics.CodeAnalysis.NotNull] T value)
        {
            Value = value;
            _incomingNodes = new HashSet<Node<T>>();
            _outgoingNodes = new HashSet<Node<T>>();
        }
        
        public T Value { get; }

        public Compatibility.IReadOnlyCollection<Node<T>> IncomingNodes => new ReadOnlyCollectionAdapter<Node<T>>(_incomingNodes);

        public ISet<Edge<T>> IncomingEdges => 
            IncomingNodes.Select(n => new Edge<T>(n, this)).ToHashSet();

        public Compatibility.IReadOnlyCollection<Node<T>> OutgoingNodes => new ReadOnlyCollectionAdapter<Node<T>>(_incomingNodes);
        
        public ISet<Edge<T>> OutgoingEdges => 
            OutgoingNodes.Select(n => new Edge<T>(this, n)).ToHashSet();

        public void AddTargetNode(Node<T> targetNode)
        {
            _outgoingNodes.Add(targetNode);
            targetNode._incomingNodes.Add(this);
        }

        public void AddSourceNode(Node<T> sourceNode) => sourceNode.AddTargetNode(this);

        /// <inheritdoc />
        public bool Equals(Node<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node<T>) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

        public static bool operator ==(Node<T> left, Node<T> right) => Equals(left, right);

        public static bool operator !=(Node<T> left, Node<T> right) => !Equals(left, right);
    }
}