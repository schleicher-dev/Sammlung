using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlung.Graphs;
using Sammlung.Graphs.Algorithms;
using Sammlung.Graphs.Algorithms.SCC;

namespace _Fixtures.Sammlung
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class StronglyConnectedDetectorTests
    {
        [Test]
        public void TheExampleIsStronglyConnected_ShouldReturnOneComponent()
        {
            var graph = new DiGraph<int>();
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 1);
            
            var verifier = new TarjanStronglyConnectedDetector<int>(graph);
            verifier.EvaluateIfNotAlready();
            var components = verifier.Result;
            Assert.AreEqual(1, components.Count);
        }
        
        [Test]
        public void TheExampleHasTwoComponents_ShouldReturnSevenComponents()
        {
            var graph = new DiGraph<int>();
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 1);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 4);
            
            var verifier = new TarjanStronglyConnectedDetector<int>(graph);
            verifier.EvaluateIfNotAlready();
            var components = verifier.Result;
            Assert.AreEqual(2, components.Count);
        }

        [Test]
        public void TheExampleHasNoCycle_ShouldReturnAllComponents()
        {
            
            var graph = new DiGraph<int>();
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 6);
            graph.AddEdge(6, 7);
            
            var verifier = new TarjanStronglyConnectedDetector<int>(graph);
            verifier.EvaluateIfNotAlready();
            var components = verifier.Result;
            Assert.AreEqual(7, components.Count);
        }
    }
}