using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using PathFinderTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTests
{
    internal class NodeGraphTests
    {
        INodeGraph graph_sut;
        
        [SetUp]
        public void SetUp()
        {
            graph_sut = new GraphStub();
        }

        [Test]
        public void Should_add_node_to_graph()
        {
            // GIVEN
            INode node = new Node("1");

            // WHEN
            bool result = graph_sut.AddNode(node);

            // THEN
            Assert.That(result, Is.True);

            var addedNode = graph_sut.GetNode("1");
            Assert.IsNotNull(addedNode);
            Assert.That(addedNode.GetUniqueIdentifier(), Is.EqualTo("1"));
        }

        [Test]
        public void Should_get_node_count()
        {
            // GIVEN
            INode node = new Node("1");
            graph_sut.AddNode(node);

            // WHEN
            var count = graph_sut.GetNodesCount();

            // THEN
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void Should_get_node()
        {
            // GIVEN
            INode node = new Node("1");
            graph_sut.AddNode(node);

            // WHEN
            var nodeInGraph = graph_sut.GetNode("1");

            // THEN
            Assert.IsNotNull(nodeInGraph);
            Assert.That(nodeInGraph.GetUniqueIdentifier(), Is.EqualTo("1"));
        }

        [Test]
        public void Should_not_get_node_if_it_is_not_in_graph()
        {
            // WHEN
            var nodeInGraph = graph_sut.GetNode("1");

            // THEN
            Assert.IsNull(nodeInGraph);
        }

        [Test]
        public void Should_not_add_node_to_graph_if_its_already_present()
        {
            // GIVEN
            INode node = new Node("1");
            graph_sut.AddNode(node);

            // WHEN
            bool result = graph_sut.AddNode(node);

            // THEN
            Assert.That(result, Is.False);
            Assert.That(graph_sut.GetNodesCount(), Is.EqualTo(1));
        }

        [Test]
        public void Should_add_unidirectional_weighted_edge()
        {
            // GIVEN
            INode startNode = new Node("1");
            INode destinationNode = new Node("2");
            graph_sut.AddNode(startNode);
            graph_sut.AddNode(destinationNode);

            // WHEN
            bool result = graph_sut.AddUnidirectionalEdge(startNode, destinationNode, 5);

            // THEN
            Assert.That(result, Is.True);
            Assert.That(graph_sut.GetEdgeWeight(startNode, destinationNode), Is.EqualTo(5));
        }
        
        [Test]
        public void Should_not_add_unidirectional_weighted_edge_if_it_already_exists()
        {
            // GIVEN
            INode startNode = new Node("1");
            INode destinationNode = new Node("2");
            graph_sut.AddNode(startNode);
            graph_sut.AddNode(destinationNode);
            graph_sut.AddUnidirectionalEdge(startNode, destinationNode, 5);

            // WHEN
            bool result = graph_sut.AddUnidirectionalEdge(startNode, destinationNode, 5);

            // THEN
            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_not_add_unidirectional_weighted_edge_if_startNode_is_not_in_graph()
        {
            // GIVEN
            INode startNode = new Node("1");
            INode destinationNode = new Node("2");
            graph_sut.AddNode(destinationNode);

            // WHEN
            bool result = graph_sut.AddUnidirectionalEdge(startNode, destinationNode, 5);

            // THEN
            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_not_add_unidirectional_weighted_edge_if_destinationNode_is_not_in_graph()
        {
            // GIVEN
            INode startNode = new Node("1");
            INode destinationNode = new Node("2");
            graph_sut.AddNode(startNode);

            // WHEN
            bool result = graph_sut.AddUnidirectionalEdge(startNode, destinationNode, 5);

            // THEN
            Assert.That(result, Is.False);
        }

        //[Test]
        //public void Should_return_an_error_when_trying_to_get_the_weight_of_an_edge_
    }
}
