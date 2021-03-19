using System;
using System.Collections.Generic;

namespace Sammlung.Graphs.Algorithms
{
    public interface IStronglyConnectedDetector<T> where T : IEquatable<T>
    {
        bool IsEvaluated { get; }
        IList<IList<Node<T>>> Result { get; }
        void Evaluate();
        
    }

    public static class StronglyConnectedDetectorExtensions
    {
        public static void EvaluateIfNotAlready<T>(this IStronglyConnectedDetector<T> detector) where T : IEquatable<T>
        {
            if (!detector.IsEvaluated) detector.Evaluate();
        }
    }
}