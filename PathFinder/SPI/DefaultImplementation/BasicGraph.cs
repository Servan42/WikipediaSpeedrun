using PathFinder.SPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.SPI.DefaultImplementation
{
    public class BasicGraph : IGraphForPathFinding
    {
        public Dictionary<string, INodeForPathFinding> Nodes { get; set; }
        public Dictionary<string, Dictionary<string, int>> WeightedAdjacencyList { get; set; }

        public BasicGraph()
        {
            Nodes = new();
            WeightedAdjacencyList = new();
        }

        public void AddNode(INodeForPathFinding node)
        {
            Nodes.Add(node.GetNodeIdentifier(), node);
            WeightedAdjacencyList.Add(node.GetNodeIdentifier(), new Dictionary<string, int>());
        }

        public void AddBidirectionalEdge(INodeForPathFinding startNode, INodeForPathFinding destinationNode, int weight)
        {
            WeightedAdjacencyList[startNode.GetNodeIdentifier()].Add(destinationNode.GetNodeIdentifier(), weight);
            WeightedAdjacencyList[destinationNode.GetNodeIdentifier()].Add(startNode.GetNodeIdentifier(), weight);
        }

        public int GetEdgeWeight(INodeForPathFinding startNode, INodeForPathFinding destinationNode)
        {
            return WeightedAdjacencyList[startNode.GetNodeIdentifier()][destinationNode.GetNodeIdentifier()];
        }

        public int GetHeuristicDistanceToGoal(INodeForPathFinding startNode, INodeForPathFinding destinationNode)
        {
            return 1;
        }

        public IEnumerable<INodeForPathFinding> GetNeighbors(INodeForPathFinding node)
        {
            List<INodeForPathFinding> nodes = new();
            foreach (var nodeId in WeightedAdjacencyList[node.GetNodeIdentifier()].Keys)
            {
                nodes.Add(this.Nodes[nodeId]);
            };
            return nodes;
        }
    }
}
