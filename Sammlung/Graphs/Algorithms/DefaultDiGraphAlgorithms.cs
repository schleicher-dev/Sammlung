using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sammlung.Graphs.Algorithms.SCC;

namespace Sammlung.Graphs.Algorithms
{
    /// <summary>
    /// The <see cref="DefaultDiGraphAlgorithms"/> class exposes methods to calculate default properties of a graph.
    /// </summary>
    [PublicAPI]
    public class DefaultDiGraphAlgorithms : IDiGraphAlgorithms
    {
        /// <inheritdoc />
        public IEnumerable<IDiGraph<T, TWeight>> GetStronglyConnectedComponents<T, TWeight>(IDiGraph<T, TWeight> graph) 
            where TWeight : IComparable<TWeight> =>
            DefaultAlgorithmFactory.CreateStrongConnectednessAlgorithm<T, TWeight>().CalculateComponents(graph);

        /// <inheritdoc />
        public bool IsStronglyConnected<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight> => 
            GetStronglyConnectedComponents(graph).Count() == 1;

        private static bool IsCyclicSubGraph<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight>
            => 1 < graph.Vertices.Count() || graph.Vertices.Count() == 1 && graph.Edges.Any();

        /// <inheritdoc />
        public IEnumerable<IDiGraph<T, TWeight>> GetCyclicalComponents<T, TWeight>(IDiGraph<T, TWeight> graph) 
            where TWeight : IComparable<TWeight> =>
            GetStronglyConnectedComponents(graph).Where(IsCyclicSubGraph);

        /// <inheritdoc />
        public bool IsAcyclic<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight> => 
            !GetCyclicalComponents(graph).Any();
    }
}