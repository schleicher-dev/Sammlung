using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.Utilities;

namespace Sammlung.Graphs.Algorithms.SCC
{
    /// <summary>
    /// The <see cref="TarjanSccAlgorithm{T,TWeight}"/> is an implementation of an algorithm which
    /// determines the strongly connected components of a graph.
    /// </summary>
    /// <typeparam name="T">the vertex type</typeparam>
    /// <typeparam name="TWeight">the weight type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    internal class TarjanSccAlgorithm<T, TWeight> 
        : ISccAlgorithm<T, TWeight>
        where TWeight : IComparable<TWeight>
    {
        /// <inheritdoc />
        public IEnumerable<IDiGraph<T, TWeight>> CalculateComponents(IDiGraph<T, TWeight> graph)
        {
            var algorithm = new Algorithm(graph.RequireNotNull(nameof(graph)));
            return algorithm.CalculateComponents();
        }
        
        private class Algorithm
        {
            private readonly IDiGraph<T, TWeight> _graph;
            private int _index;
            private readonly Stack<T> _stack;
            private readonly Dictionary<T, Metadata> _metadataLookup;

            public Algorithm(IDiGraph<T, TWeight> graph)
            {
                _graph = graph;
                _metadataLookup = graph.Vertices.ToDictionary(n => n, n => new Metadata(-1, -1, false));

                _index = 0;
                _stack = new Stack<T>();
            }
            
            public IEnumerable<IDiGraph<T, TWeight>> CalculateComponents()
                => _graph.Vertices.Where(n => _metadataLookup[n].Index == -1).SelectMany(StrongConnect);

            private IEnumerable<IDiGraph<T, TWeight>> StrongConnect(T vertex)
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

                IDiGraph<T, TWeight> componentGraph = new DiGraph<T, TWeight>(_graph.DefaultEdgeWeight);

                T wVertex;
                do
                {
                    wVertex = _stack.Pop();
                    var stackNodeMetadata = _metadataLookup[wVertex];
                    stackNodeMetadata.OnStack = false;
                    componentGraph.AddVertex(wVertex);

                } while (!EqualityComparer<T>.Default.Equals(vertex, wVertex));

                var filteredEdges = componentGraph.Vertices
                    .SelectMany(v => _graph.GetOutgoingEdges(v))
                    .Where(e => componentGraph.Vertices.Contains(e.SourceVertex))
                    .Where(e => componentGraph.Vertices.Contains(e.TargetVertex));
                foreach (var edge in filteredEdges)
                    componentGraph.AddEdge(edge.SourceVertex, edge.TargetVertex, edge.Weight);
            
                yield return componentGraph;
            }
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