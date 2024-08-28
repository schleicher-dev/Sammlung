using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Sammlung.Graphs;
using Sammlung.Graphs.Algorithms;

namespace Fixtures.Sammlung.Graphs.Algorithms
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    public class StronglyConnectedComponentsAlgorithmTests
    {
        [Test]
        public void TheExampleIsStronglyConnected_ShouldReturnOneComponent()
        {
            var graph = new DiGraph<int, int>(1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 1);

            var components = graph.GetStronglyConnectedComponents().ToList();
            Assert.That(components.Count, Is.EqualTo(1));
            Assert.That(components.All(g => g.IsStronglyConnected()), Is.True);
            Assert.That(graph.IsAcyclic(), Is.False);
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
            
            var components = graph.GetStronglyConnectedComponents().ToList();
            Assert.That(components.Count, Is.EqualTo(2));
            Assert.That(components.All(g => g.IsStronglyConnected()), Is.True);
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
            
            var components = graph.GetStronglyConnectedComponents().ToList();
            Assert.That(components.Count, Is.EqualTo(7));
            Assert.That(components.All(g => g.IsStronglyConnected()), Is.True);
            Assert.That(graph.IsAcyclic(), Is.True);
        }
    }
}