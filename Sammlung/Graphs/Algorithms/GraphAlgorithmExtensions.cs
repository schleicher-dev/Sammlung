using System;
using System.Linq;
using JetBrains.Annotations;
using Sammlung.Graphs.Algorithms.SCC;

namespace Sammlung.Graphs.Algorithms
{
    [PublicAPI]
    public static class GraphAlgorithmExtensions
    {
        public static bool IsStronglyConnected<T>(this DiGraph<T> graph) 
            where T : IEquatable<T>
        {
            var detector = new TarjanStronglyConnectedDetector<T>(graph);
            return detector.IsStronglyConnected();
        }

        public static bool IsStronglyConnected<T>(this IStronglyConnectedDetector<T> detector)
            where T : IEquatable<T>
        {
            detector.EvaluateIfNotAlready();
            return detector.Result.SingleOrDefault() != null;
        }

        public static bool HasCycles<T>(this DiGraph<T> graph) where T : IEquatable<T>
        {
            var detector = new TarjanStronglyConnectedDetector<T>(graph);
            return detector.HasCycles(graph.Nodes.Count);
        }

        public static bool HasCycles<T>(this IStronglyConnectedDetector<T> detector, int numNodesWhenNoCycle) where T : IEquatable<T>
        {
            detector.EvaluateIfNotAlready();
            return detector.Result.Count != numNodesWhenNoCycle;
        }
    }
}