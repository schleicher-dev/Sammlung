using System.Collections.Generic;
using NUnit.Framework;
using Sammlung.Graphs;
using Sammlung.Graphs.Algorithms;

namespace _Fixtures.Sammlung
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class GraphTests
    {
        [Test]
        public void CreationOfGraphYieldsCorrectEdges()
        {
            var graph = new DiGraph<int, int>(1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 6);
            graph.AddEdge(6, 7);
            
            var expectedEdges = new List<IEdge<int, int>>
            {
                new Edge<int, int>(1, 2, 1),
                new Edge<int, int>(2, 3, 1),
                new Edge<int, int>(3, 4, 1),
                new Edge<int, int>(4, 5, 1),
                new Edge<int, int>(5, 6, 1),
                new Edge<int, int>(6, 7, 1),
            };
            
            CollectionAssert.AreEquivalent(expectedEdges, graph.Edges);

            var copyGraph = new DiGraph<int, int>(graph);
            CollectionAssert.AreEquivalent(expectedEdges, copyGraph.Edges);
        }

        [Test]
        public void GraphKnowsTheIncomingAndOutgoingEdges()
        {
            var graph = new DiGraph<int, int>(1);
            graph.AddEdge(1, 1);
            graph.AddEdge(2, 2);
            graph.AddEdge(3, 3);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 1);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 2);
            graph.AddEdge(3, 1);
            graph.AddEdge(1, 3);

            CollectionAssert.AreEquivalent(new List<IEdge<int, int>>
            {
                new Edge<int, int>(1, 1, 1),
                new Edge<int, int>(2, 1, 1),
                new Edge<int, int>(3, 1, 1),
            }, graph.GetIncomingEdges(1));

            CollectionAssert.AreEquivalent(new List<IEdge<int, int>>
            {
                new Edge<int, int>(1, 1, 1),
                new Edge<int, int>(1, 2, 1),
                new Edge<int, int>(1, 3, 1),
            }, graph.GetOutgoingEdges(1));

            CollectionAssert.AreEquivalent(new List<IEdge<int, int>>
            {
                new Edge<int, int>(1, 2, 1),
                new Edge<int, int>(2, 2, 1),
                new Edge<int, int>(3, 2, 1),
            }, graph.GetIncomingEdges(2));

            CollectionAssert.AreEquivalent(new List<IEdge<int, int>>
            {
                new Edge<int, int>(2, 1, 1),
                new Edge<int, int>(2, 2, 1),
                new Edge<int, int>(2, 3, 1),
            }, graph.GetOutgoingEdges(2));
            
            CollectionAssert.AreEquivalent(new List<IEdge<int, int>>
            {
                new Edge<int, int>(1, 3, 1),
                new Edge<int, int>(2, 3, 1),
                new Edge<int, int>(3, 3, 1),
            }, graph.GetIncomingEdges(3));

            CollectionAssert.AreEquivalent(new List<IEdge<int, int>>
            {
                new Edge<int, int>(3, 1, 1),
                new Edge<int, int>(3, 2, 1),
                new Edge<int, int>(3, 3, 1),
            }, graph.GetOutgoingEdges(3));
            
            Assert.IsTrue(graph.HasEdge(3, 1));
            Assert.IsFalse(graph.HasEdge(4, 2));
        }

        [Test]
        public void HasVertex()
        {
            var graph = new DiGraph<int, int>(1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 6);
            graph.AddEdge(6, 7);
            graph.AddVertex(99);
            
            Assert.IsTrue(graph.HasVertex(1));
            Assert.IsTrue(graph.HasVertex(2));
            Assert.IsTrue(graph.HasVertex(3));
            Assert.IsTrue(graph.HasVertex(4));
            Assert.IsTrue(graph.HasVertex(5));
            Assert.IsTrue(graph.HasVertex(6));
            Assert.IsTrue(graph.HasVertex(7));
            Assert.IsTrue(graph.HasVertex(99));
            Assert.IsFalse(graph.HasVertex(8));
        }

        [Test]
        public void Edge_Equals()
        {
            var edgeA = new Edge<int, int>(1, 2, 3);
            var edgeB = new Edge<int, int>(1, 2, 3);
            var edgeC = new Edge<int, int>(1, 2, 4);
            
            Assert.IsTrue(edgeA.Equals(edgeA));
            Assert.IsTrue(edgeA.Equals(edgeB));
            Assert.IsFalse(edgeA.Equals(edgeC));
            Assert.IsFalse(edgeA.Equals(null));
            
            Assert.IsTrue(edgeA.Equals((object) edgeA));
            Assert.IsTrue(edgeA.Equals((object) edgeB));
            Assert.IsFalse(edgeA.Equals((object) edgeC));
            Assert.IsFalse(edgeA.Equals(default(object)));
            Assert.IsFalse(edgeA.Equals("A"));
        }

        [Test]
        public void ToStringComparison()
        {
            var edgeA = new Edge<int, int>(1, 2, 3);
            Assert.AreEqual("Sammlung.Graphs.Edge`2[Source=1, Target=2, Weight=3]", edgeA.ToString());
        }
    }
}