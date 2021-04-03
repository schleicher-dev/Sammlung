using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sammlung.Graphs.Algorithms.SCC;

namespace Sammlung.Graphs.Algorithms
{
    [PublicAPI]
    public static class GraphAlgorithmExtensions
    {
        public static IEnumerable<IDiGraph<TVertex, TWeight>> 
            GetStronglyConnectedComponents<TVertex, TWeight>(this IDiGraph<TVertex, TWeight> graph)
            where TWeight : IComparable<TWeight> =>
            AlgorithmStrategy.Instance.GetStronglyConnectedComponentsAlgorithm(graph).Result;

        public static bool IsStronglyConnected<TVertex, TWeight>(this IDiGraph<TVertex, TWeight> graph)
            where TWeight : IComparable<TWeight> =>
            graph.GetStronglyConnectedComponents().Count() == 1;

        public static IEnumerable<IDiGraph<TVertex, TWeight>> GetCyclicalComponents<TVertex, TWeight>(this IDiGraph<TVertex, TWeight> graph) 
            where TWeight : IComparable<TWeight> =>
            graph.GetStronglyConnectedComponents().Where(g => 1 < g.Vertices.Count());

        public static bool IsAcyclic<TVertex, TWeight>(this IDiGraph<TVertex, TWeight> graph)
            where TWeight : IComparable<TWeight> =>
            !graph.GetCyclicalComponents().Any();
    }
}