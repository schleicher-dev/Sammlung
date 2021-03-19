using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Sammlung.Graphs.Algorithms.SCC
{
    internal class TarjanStronglyConnectedDetector<T> : IStronglyConnectedDetector<T> where T : IEquatable<T>
    {
        private readonly Dictionary<Node<T>, Metadata> _metadataLookup;
        private readonly Stack<Node<T>> _stack;
        private readonly DiGraph<T> _graph;
        private int _index;
        private List<IList<Node<T>>> _result;

        public TarjanStronglyConnectedDetector([NotNull] DiGraph<T> graph)
        {
            _graph = graph;
            _metadataLookup = graph.Nodes.ToDictionary(n => n, n => new Metadata(-1, -1, false));

            _index = 0;
            _stack = new Stack<Node<T>>();
            IsEvaluated = false;
        }

        /// <inheritdoc />
        public bool IsEvaluated { get; private set; }

        /// <inheritdoc />
        public IList<IList<Node<T>>> Result => _result;

        /// <inheritdoc />
        public void Evaluate()
        {
            _result = _graph.Nodes.Where(n => _metadataLookup[n].Index == -1).SelectMany(StrongConnect).ToList();
            IsEvaluated = true;
        }
        
        private IEnumerable<IList<Node<T>>> StrongConnect(Node<T> node)
        {
            var metadata = _metadataLookup[node];
            metadata.Index = _index;
            metadata.LowLink = _index;
            _index += 1;
            _stack.Push(node);
            metadata.OnStack = true;

            foreach (var outgoingNode in node.OutgoingNodes)
            {
                var outgoingMetadata = _metadataLookup[outgoingNode];
                if (outgoingMetadata.Index == -1)
                    foreach (var result in StrongConnect(outgoingNode))
                        yield return result;
                if (outgoingMetadata.OnStack)
                    metadata.LowLink = Math.Min(metadata.LowLink, outgoingMetadata.LowLink);
            }
            
            if(metadata.LowLink != metadata.Index) yield break;

            var componentNodes = new List<Node<T>>();
            Node<T> stackNode;
            do
            {
                stackNode = _stack.Pop();
                var stackNodeMetadata = _metadataLookup[stackNode];
                stackNodeMetadata.OnStack = false;
                componentNodes.Add(stackNode);
            } while (node != stackNode);

            yield return componentNodes;
        }

        private class Metadata
        {
            public Metadata(int index, int lowLink, bool onStack)
            {
                Index = index;
                LowLink = lowLink;
                OnStack = onStack;
            }
            
            public int Index { get; set; }
            public int LowLink { get; set; }
            public bool OnStack { get; set; }
        }
    }
}