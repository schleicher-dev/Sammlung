using System;
using System.Collections.Generic;

namespace Sammlung.Graphs.Algorithms
{
    public interface IStronglyConnectedComponentsAlgorithm<TVertex, TWeight>
        where TWeight : IComparable<TWeight>
    {
        bool IsEvaluated { get; }
        IEnumerable<IDiGraph<TVertex, TWeight>> Result { get; }
        void Evaluate();
    }
}