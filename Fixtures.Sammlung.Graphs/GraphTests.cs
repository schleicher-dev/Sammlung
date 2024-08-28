using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Sammlung.Graphs;

namespace Fixtures.Sammlung.Graphs
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
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
                new Edge<int, int>(6, 7, 1)
            };

            Assert.That(graph.Edges, Is.EquivalentTo(expectedEdges));

            var copyGraph = new DiGraph<int, int>(graph);
            Assert.That(copyGraph.Edges, Is.EquivalentTo(expectedEdges));
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

            Assert.Multiple(() =>
            {
                Assert.That(graph.GetIncomingEdges(1), Is.EquivalentTo(new List<IEdge<int, int>>
                {
                    new Edge<int, int>(1, 1, 1),
                    new Edge<int, int>(2, 1, 1),
                    new Edge<int, int>(3, 1, 1)
                }));

                Assert.That(graph.GetOutgoingEdges(1), Is.EquivalentTo(new List<IEdge<int, int>>
                {
                    new Edge<int, int>(1, 1, 1),
                    new Edge<int, int>(1, 2, 1),
                    new Edge<int, int>(1, 3, 1)
                }));

                Assert.That(graph.GetIncomingEdges(2), Is.EquivalentTo(new List<IEdge<int, int>>
                {
                    new Edge<int, int>(1, 2, 1),
                    new Edge<int, int>(2, 2, 1),
                    new Edge<int, int>(3, 2, 1)
                }));

                Assert.That(graph.GetOutgoingEdges(2), Is.EquivalentTo(new List<IEdge<int, int>>
                {
                    new Edge<int, int>(2, 1, 1),
                    new Edge<int, int>(2, 2, 1),
                    new Edge<int, int>(2, 3, 1)
                }));

                Assert.That(graph.GetIncomingEdges(3), Is.EquivalentTo(new List<IEdge<int, int>>
                {
                    new Edge<int, int>(1, 3, 1),
                    new Edge<int, int>(2, 3, 1),
                    new Edge<int, int>(3, 3, 1)
                }));

                Assert.That(graph.GetOutgoingEdges(3), Is.EquivalentTo(new List<IEdge<int, int>>
                {
                    new Edge<int, int>(3, 1, 1),
                    new Edge<int, int>(3, 2, 1),
                    new Edge<int, int>(3, 3, 1)
                }));

                Assert.That(graph.HasEdge(3, 1), Is.True);
                Assert.That(graph.HasEdge(4, 2), Is.False);
            });
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

            Assert.Multiple(() =>
            {
                Assert.That(graph.HasVertex(1), Is.True);
                Assert.That(graph.HasVertex(2), Is.True);
                Assert.That(graph.HasVertex(3), Is.True);
                Assert.That(graph.HasVertex(4), Is.True);
                Assert.That(graph.HasVertex(5), Is.True);
                Assert.That(graph.HasVertex(6), Is.True);
                Assert.That(graph.HasVertex(7), Is.True);
                Assert.That(graph.HasVertex(99), Is.True);
                Assert.That(graph.HasVertex(8), Is.False);
            });
        }

        [Test]
        public void Edge_Equals()
        {
            var edgeA = new Edge<int, int>(1, 2, 3);
            var edgeB = new Edge<int, int>(1, 2, 3);
            var edgeC = new Edge<int, int>(1, 2, 4);

            Assert.Multiple(() =>
            {
                Assert.That(edgeA.Equals(edgeA), Is.True);
                Assert.That(edgeA.Equals(edgeB), Is.True);
                Assert.That(edgeA.Equals(edgeC), Is.False);
                Assert.That(edgeA.Equals(null), Is.False);

                Assert.That(edgeA.Equals((object)edgeA), Is.True);
                Assert.That(edgeA.Equals((object)edgeB), Is.True);
                Assert.That(edgeA.Equals((object)edgeC), Is.False);
                Assert.That(edgeA.Equals(default(object)), Is.False);
                // ReSharper disable once SuspiciousTypeConversion.Global
                Assert.That(edgeA.Equals("A"), Is.False);
            });
        }

        [Test]
        public void ToStringComparison()
        {
            var edgeA = new Edge<int, int>(1, 2, 3);
            Assert.That(edgeA.ToString(), Is.EqualTo("Sammlung.Graphs.Edge`2[Source=1, Target=2, Weight=3]"));
        }
    }
}