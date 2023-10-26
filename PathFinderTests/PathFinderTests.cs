using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using PathFinder.SPI.DefaultImplementation;

namespace PathFinderTests
{
    public class PathFinderTests
    {
        [Test]
        public void Should_pathfind_on_BasicGraph()
        {
            var graph = new BasicGraph();

            var node0 = new BasicNode("0");
            var node1 = new BasicNode("1");
            var node2 = new BasicNode("2");
            var node3 = new BasicNode("3");

            graph.AddNode(node0);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);

            graph.AddBidirectionalEdge(node0, node1, 3);
            graph.AddBidirectionalEdge(node0, node2, 1);
            graph.AddBidirectionalEdge(node1, node2, 1);
            graph.AddBidirectionalEdge(node1, node3, 1);

            var pathFinder_sut = new PathFinderService(graph);

            var keyCameFromValue = pathFinder_sut.BreadthFirstSearch(node0, node3);
            var path = pathFinder_sut.GetPath(keyCameFromValue, node0, node3);
            Assert.That(string.Join(',', path.Select(n => n.GetNodeIdentifier()).ToList()), Is.EqualTo("0,1,3"));

            keyCameFromValue = pathFinder_sut.HeuristicSearch(node0, node3);
            path = pathFinder_sut.GetPath(keyCameFromValue, node0, node3);
            Assert.That(string.Join(',', path.Select(n => n.GetNodeIdentifier()).ToList()), Is.EqualTo("0,1,3"));

            keyCameFromValue = pathFinder_sut.DijkstrasAlgorithm(node0, node3);
            path = pathFinder_sut.GetPath(keyCameFromValue, node0, node3);
            Assert.That(string.Join(',', path.Select(n => n.GetNodeIdentifier()).ToList()), Is.EqualTo("0,2,1,3"));

            keyCameFromValue = pathFinder_sut.AstarAlgorithm(node0, node3);
            path = pathFinder_sut.GetPath(keyCameFromValue, node0, node3);
            Assert.That(string.Join(',', path.Select(n => n.GetNodeIdentifier()).ToList()), Is.EqualTo("0,2,1,3"));
        }
    }
}