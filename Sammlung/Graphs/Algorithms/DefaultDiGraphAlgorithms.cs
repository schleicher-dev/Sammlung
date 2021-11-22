using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sammlung.Utilities;

namespace Sammlung.Graphs.Algorithms
{
    /// <summary>
    /// The <see cref="DefaultDiGraphAlgorithms"/> class exposes methods to calculate default properties of a graph.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class DefaultDiGraphAlgorithms : IDiGraphAlgorithms
    {
        /// <inheritdoc />
        public IEnumerable<IDiGraph<T, TWeight>> GetStronglyConnectedComponents<T, TWeight>(IDiGraph<T, TWeight> graph) 
            where TWeight : IComparable<TWeight> =>
            DefaultAlgorithmFactory.CreateStrongConnectednessAlgorithm<T, TWeight>().CalculateComponents(graph.RequireNotNull(nameof(graph)));

        /// <inheritdoc />
        public bool IsStronglyConnected<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight> => 
            GetStronglyConnectedComponents(graph.RequireNotNull(nameof(graph))).Count() == 1;

        private static bool IsCyclicSubGraph<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight>
        {
            graph = graph.RequireNotNull(nameof(graph));
            return 1 < graph.Vertices.Count() || graph.Vertices.Count() == 1 && graph.Edges.Any();
        }

        /// <inheritdoc />
        public IEnumerable<IDiGraph<T, TWeight>> GetCyclicalComponents<T, TWeight>(IDiGraph<T, TWeight> graph) 
            where TWeight : IComparable<TWeight> =>
            GetStronglyConnectedComponents(graph.RequireNotNull(nameof(graph))).Where(IsCyclicSubGraph);

        /// <inheritdoc />
        public bool IsAcyclic<T, TWeight>(IDiGraph<T, TWeight> graph) where TWeight : IComparable<TWeight> => 
            !GetCyclicalComponents(graph.RequireNotNull(nameof(graph))).Any();
    }
}