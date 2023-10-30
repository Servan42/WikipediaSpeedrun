
using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using System.Security.Cryptography.X509Certificates;

namespace PathFinderTests
{
    public class PathFinderTests
    {
        [Test]
        public void Should_pathfind_on_BasicGraph()
        {
            INodeGraphWithPathFinding graph = new BasicGraph();

            var node0 = new Node("0");
            var node1 = new Node("1");
            var node2 = new Node("2");
            var node3 = new Node("3");

            graph.AddNode(node0);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);

            graph.AddBidirectionalEdge(node0, node1, 3);
            graph.AddBidirectionalEdge(node0, node2, 1);
            graph.AddBidirectionalEdge(node1, node2, 1);
            graph.AddBidirectionalEdge(node1, node3, 1);

            var path = graph.BreadthFirstSearch(node0, node3);
            Assert.That(string.Join(',', path.Select(n => n.GetUniqueIdentifier()).ToList()), Is.EqualTo("0,1,3"));

            path = graph.HeuristicSearch(node0, node3);
            Assert.That(string.Join(',', path.Select(n => n.GetUniqueIdentifier()).ToList()), Is.EqualTo("0,1,3"));

            path = graph.DijkstrasAlgorithm(node0, node3);
            Assert.That(string.Join(',', path.Select(n => n.GetUniqueIdentifier()).ToList()), Is.EqualTo("0,2,1,3"));

            path = graph.AstarAlgorithm(node0, node3);
            Assert.That(string.Join(',', path.Select(n => n.GetUniqueIdentifier()).ToList()), Is.EqualTo("0,2,1,3"));
        }
    }

    internal class BasicGraph : NodeGraphWithPathFinding
    {
        public override int GetHeuristicDistanceToGoal(INode startNode, INode destinationNode)
        {
            return 1;
        }
    }
}