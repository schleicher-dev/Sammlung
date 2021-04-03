using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Sammlung.Graphs;
using Sammlung.Graphs.Algorithms;
using Sammlung.Graphs.Algorithms.SCC;

namespace _Fixtures.Sammlung
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class StronglyConnectedComponentsAlgorithmTests
    {
        [Test]
        public void TheExampleIsStronglyConnected_ShouldReturnOneComponent()
        {
            var graph = new DiGraph<int, int>(1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 1);

            var graphAlgorithms = new DefaultDiGraphAlgorithms();
            var components = graphAlgorithms.GetStronglyConnectedComponents(graph).ToList();
            Assert.AreEqual(1, components.Count);
            Assert.IsTrue(components.All(g => graphAlgorithms.IsStronglyConnected(g)));
            Assert.IsFalse(graphAlgorithms.IsAcyclic(graph));
        }
        
        [Test]
        public void TheExampleHasTwoComponents_ShouldReturnTwoComponents()
        {
            var graph = new DiGraph<int, int>(1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 1);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 4);
            
            var graphAlgorithms = new DefaultDiGraphAlgorithms();
            var components = graphAlgorithms.GetStronglyConnectedComponents(graph).ToList();
            Assert.AreEqual(2, components.Count);
            Assert.IsTrue(components.All(g => graphAlgorithms.IsStronglyConnected(g)));
        }

        [Test]
        public void TheExampleHasNoCycle_ShouldReturnAllComponents()
        {
            var graph = new DiGraph<int, int>(1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 6);
            graph.AddEdge(6, 7);
            
            var graphAlgorithms = new DefaultDiGraphAlgorithms();
            var components = graphAlgorithms.GetStronglyConnectedComponents(graph).ToList();
            Assert.AreEqual(7, components.Count);
            Assert.IsTrue(components.All(g => graphAlgorithms.IsStronglyConnected(g)));
            Assert.IsTrue(graphAlgorithms.IsAcyclic(graph));
        }
    }
}