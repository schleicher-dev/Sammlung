using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Exceptions;

namespace Sammlung.Graphs.Algorithms.SCC
{
    internal class TarjanStronglyConnectedComponentsAlgorithm<TVertex, TWeight> 
        : IStronglyConnectedComponentsAlgorithm<TVertex, TWeight>
        where TWeight : IComparable<TWeight>
    {
        private readonly IDiGraph<TVertex, TWeight> _graph;
        private readonly Dictionary<TVertex, Metadata> _metadataLookup;
        private List<IDiGraph<TVertex, TWeight>> _result;
        private int _index;
        private readonly Stack<TVertex> _stack;

        public TarjanStronglyConnectedComponentsAlgorithm(IDiGraph<TVertex, TWeight> graph)
        {
            _graph = graph;
            _metadataLookup = graph.Vertices.ToDictionary(n => n, n => new Metadata(-1, -1, false));
            _result = new List<IDiGraph<TVertex, TWeight>>();

            _index = 0;
            _stack = new Stack<TVertex>();
            IsEvaluated = false;
        }

        /// <inheritdoc />
        public bool IsEvaluated { get; private set; }

        public IEnumerable<IDiGraph<TVertex, TWeight>> Result
        {
            get
            {
                 if (!IsEvaluated) Evaluate();
                return _result ;
            }
        }

        /// <inheritdoc />
        public void Evaluate()
        {
            _result = _graph.Vertices.Where(n => _metadataLookup[n].Index == -1).SelectMany(StrongConnect).ToList();
            IsEvaluated = true;
        }
        
        private IEnumerable<IDiGraph<TVertex, TWeight>> StrongConnect(TVertex vertex)
        {
            var metadata = _metadataLookup[vertex];
            metadata.Index = _index;
            metadata.LowLink = _index;
            _index += 1;
            _stack.Push(vertex);
            metadata.OnStack = true;

            foreach (var outEdge in _graph.GetOutgoingEdges(vertex))
            {
                var targetVertex = outEdge.TargetVertex;

                var outMetadata = _metadataLookup[targetVertex];
                if (outMetadata.Index == -1)
                    foreach (var result in StrongConnect(targetVertex))
                        yield return result;
                if (outMetadata.OnStack)
                    metadata.LowLink = Math.Min(metadata.LowLink, outMetadata.LowLink);
            }

            if (metadata.LowLink != metadata.Index) yield break;

            IDiGraph<TVertex, TWeight> componentGraph = new DiGraph<TVertex, TWeight>(_graph.DefaultEdgeWeight);

            TVertex wVertex;
            do
            {
                wVertex = _stack.Pop();
                var stackNodeMetadata = _metadataLookup[wVertex];
                stackNodeMetadata.OnStack = false;
                componentGraph.AddVertex(wVertex);

            } while (!EqualityComparer<TVertex>.Default.Equals(vertex, wVertex));

            var filteredEdges = componentGraph.Vertices
                .SelectMany(v => _graph.GetOutgoingEdges(v))
                .Where(e => componentGraph.Vertices.Contains(e.SourceVertex))
                .Where(e => componentGraph.Vertices.Contains(e.TargetVertex));
            foreach (var edge in filteredEdges)
                componentGraph.AddEdge(edge.SourceVertex, edge.TargetVertex, edge.Weight);
            
            yield return componentGraph;
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