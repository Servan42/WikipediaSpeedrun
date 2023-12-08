using PathFinder.API.Exceptions;
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

        [Test]
        public void Should_the_weight_of_an_edge()
        {
            // GIVEN
            INode startNode = new Node("1");
            INode destinationNode = new Node("2");
            graph_sut.AddNode(startNode);
            graph_sut.AddNode(destinationNode);
            graph_sut.AddUnidirectionalEdge(startNode, destinationNode, 5);

            // WHEN
            int weight = graph_sut.GetEdgeWeight(startNode, destinationNode);

            // THEN
            Assert.That(weight, Is.EqualTo(5));
        }

        [Test]
        public void Should_return_an_error_when_trying_to_get_the_weight_of_an_edge_of_non_existant_startNode()
        {
            // GIVEN
            INode startNode = new Node("1");
            INode destinationNode = new Node("2");
            graph_sut.AddNode(destinationNode);

            // WHEN
            var ex = Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(startNode, destinationNode));

            // THEN
            Assert.That(ex.Message, Is.EqualTo("Start node '1' is not present in graph"));
        }

        [Test]
        public void Should_return_an_error_when_trying_to_get_the_weight_of_an_edge_of_non_existant_destinationNode()
        {
            // GIVEN
            INode startNode = new Node("1");
            INode destinationNode = new Node("2");
            graph_sut.AddNode(startNode);

            // WHEN
            var ex = Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(startNode, destinationNode));

            // THEN
            Assert.That(ex.Message, Is.EqualTo("Destination node '2' is not present in graph"));
        }

        [Test]
        public void Should_return_an_error_when_trying_to_get_the_weight_of_an_edge_that_do_not_exist()
        { 
            // GIVEN
            INode startNode = new Node("1");
            INode destinationNode = new Node("2");
            graph_sut.AddNode(startNode);
            graph_sut.AddNode(destinationNode);

            // WHEN
            var ex = Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(startNode, destinationNode));

            // THEN
            Assert.That(ex.Message, Is.EqualTo("Edge from node '1' to node '2' does not exist in graph"));
        }

        [Test]
        public void Should_get_neighbors()
        {
            // GIVEN
            INode node1 = new Node("1");
            INode node2 = new Node("2");
            INode node3 = new Node("3");
            graph_sut.AddNode(node1);
            graph_sut.AddNode(node2);
            graph_sut.AddNode(node3);
            graph_sut.AddUnidirectionalEdge(node1, node2, 1);
            graph_sut.AddUnidirectionalEdge(node1, node3, 1);

            // WHEN
            var neighgbors = graph_sut.GetNeighbors(node1).ToList();

            // THEN
            Assert.That(neighgbors.Count, Is.EqualTo(2));
            Assert.That(neighgbors.First().GetUniqueIdentifier(), Is.EqualTo("2"));
            Assert.That(neighgbors.Last().GetUniqueIdentifier(), Is.EqualTo("3"));
        }

        [Test]
        public void Should_return_empty_list_if_node_is_not_in_graph()
        {
            // GIVEN
            INode node1 = new Node("1");

            // WHEN
            var neighgbors = graph_sut.GetNeighbors(node1).ToList();

            // THEN
            Assert.That(neighgbors.Count, Is.EqualTo(0));
        }

        [Test]
        public void Should_add_bidirectional_edge()
        {
            // GIVEN
            INode node1 = new Node("1");
            INode node2 = new Node("2");
            graph_sut.AddNode(node1);
            graph_sut.AddNode(node2);

            // WHEN
            bool result = graph_sut.AddBidirectionalEdge(node1, node2, 5);

            // THEN
            Assert.That(result, Is.True);
            Assert.That(graph_sut.GetEdgeWeight(node1, node2), Is.EqualTo(5));
            Assert.That(graph_sut.GetEdgeWeight(node2, node1), Is.EqualTo(5));
        }

        [Test]
        public void Should_retrun_false_when_cannot_add_bidirectional_edge_because_node1_is_not_in_graph()
        {
            // GIVEN
            INode node1 = new Node("1");
            INode node2 = new Node("2");
            graph_sut.AddNode(node2);

            // WHEN
            bool result = graph_sut.AddBidirectionalEdge(node1, node2, 5);

            // THEN
            Assert.That(result, Is.False);
            Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(node1, node2));
            Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(node2, node1));
        }

        [Test]
        public void Should_retrun_false_when_cannot_add_bidirectional_edge_because_node2_is_not_in_graph()
        {
            // GIVEN
            INode node1 = new Node("1");
            INode node2 = new Node("2");
            graph_sut.AddNode(node1);

            // WHEN
            bool result = graph_sut.AddBidirectionalEdge(node1, node2, 5);

            // THEN
            Assert.That(result, Is.False);
            Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(node1, node2));
            Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(node2, node1));
        }

        [Test]
        public void Should_retrun_false_when_cannot_add_bidirectional_edge_because_edge_from_1_to_2_already_exist()
        {
            // GIVEN
            INode node1 = new Node("1");
            INode node2 = new Node("2");
            graph_sut.AddNode(node1);
            graph_sut.AddNode(node2);
            graph_sut.AddUnidirectionalEdge(node1, node2, 5);

            // WHEN
            bool result = graph_sut.AddBidirectionalEdge(node1, node2, 5);

            // THEN
            Assert.That(result, Is.False);
            Assert.That(graph_sut.GetEdgeWeight(node1, node2), Is.EqualTo(5));
            Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(node2, node1));
        }

        [Test]
        public void Should_retrun_false_when_cannot_add_bidirectional_edge_because_edge_from_2_to_1_already_exist()
        {
            // GIVEN
            INode node1 = new Node("1");
            INode node2 = new Node("2");
            graph_sut.AddNode(node1);
            graph_sut.AddNode(node2);
            graph_sut.AddUnidirectionalEdge(node2, node1, 5);

            // WHEN
            bool result = graph_sut.AddBidirectionalEdge(node1, node2, 5);

            // THEN
            Assert.That(result, Is.False);
            Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(node1, node2));
            Assert.That(graph_sut.GetEdgeWeight(node2, node1), Is.EqualTo(5));
        }

        [Test]
        public void Should_retrun_false_when_cannot_add_bidirectional_edge_because_it_already_exist()
        {
            // GIVEN
            INode node1 = new Node("1");
            INode node2 = new Node("2");
            graph_sut.AddNode(node1);
            graph_sut.AddNode(node2);
            graph_sut.AddBidirectionalEdge(node2, node1, 5);

            // WHEN
            bool result = graph_sut.AddBidirectionalEdge(node1, node2, 5);

            // THEN
            Assert.That(result, Is.False);
            Assert.That(graph_sut.GetEdgeWeight(node2, node1), Is.EqualTo(5));
            Assert.That(graph_sut.GetEdgeWeight(node1, node2), Is.EqualTo(5));
        }

        [Test]
        public void Should_retrun_false_when_trying_to_add_bidirectional_edge_to_a_single_node()
        {
            // GIVEN
            INode node1 = new Node("1");
            graph_sut.AddNode(node1);

            // WHEN
            bool result = graph_sut.AddBidirectionalEdge(node1, node1, 5);

            // THEN
            Assert.That(result, Is.False);
            Assert.Throws<NodeGraphException>(() => graph_sut.GetEdgeWeight(node1, node1));
        }
    }
}
