using System;
using Sammlung.Graphs.Algorithms.SCC;

namespace Sammlung.Graphs.Algorithms
{
    internal class AlgorithmStrategy
    {
        public static AlgorithmStrategy Instance = new AlgorithmStrategy();
        
        public IStronglyConnectedComponentsAlgorithm<TVertex, TWeight>
            GetStronglyConnectedComponentsAlgorithm<TVertex, TWeight>(IDiGraph<TVertex, TWeight> graph)
            where TWeight : IComparable<TWeight> =>
            new TarjanStronglyConnectedComponentsAlgorithm<TVertex, TWeight>(graph);
    }
}