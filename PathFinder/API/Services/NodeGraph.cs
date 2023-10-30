using PathFinder.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.API.Services
{
    public abstract class NodeGraph : INodeGraph
    {
        private readonly Dictionary<string, INode> nodes;
        private readonly Dictionary<string, Dictionary<string, int>> weightedAdjacencyList;

        public NodeGraph()
        {
            nodes = new();
            weightedAdjacencyList = new();
        }

        public abstract int GetHeuristicDistanceToGoal(INode startNode, INode destinationNode);

        public bool AddNode(INode node)
        {
            if (nodes.ContainsKey(node.GetUniqueIdentifier()))
                return false;

            nodes.Add(node.GetUniqueIdentifier(), node);
            weightedAdjacencyList.Add(node.GetUniqueIdentifier(), new Dictionary<string, int>());
            return true;
        }

        public void AddBidirectionalEdge(INode startNode, INode destinationNode, int weight)
        {
            weightedAdjacencyList[startNode.GetUniqueIdentifier()].Add(destinationNode.GetUniqueIdentifier(), weight);
            weightedAdjacencyList[destinationNode.GetUniqueIdentifier()].Add(startNode.GetUniqueIdentifier(), weight);
        }

        public void AddUnidirectionalEdge(INode startNode, INode destinationNode, int weight)
        {
            if (weightedAdjacencyList[startNode.GetUniqueIdentifier()].ContainsKey(destinationNode.GetUniqueIdentifier()))
                return;

            weightedAdjacencyList[startNode.GetUniqueIdentifier()].Add(destinationNode.GetUniqueIdentifier(), weight);
        }

        public INode GetNode(string uniqueIdentifier)
        {
            return this.nodes[uniqueIdentifier];
        }

        public int GetNodesCount()
        {
            return this.nodes.Count;
        }

        public int GetEdgeWeight(INode startNode, INode destinationNode)
        {
            return weightedAdjacencyList[startNode.GetUniqueIdentifier()][destinationNode.GetUniqueIdentifier()];
        }

        public IEnumerable<INode> GetNeighbors(INode node)
        {
            List<INode> nodes = new();
            foreach (var nodeId in weightedAdjacencyList[node.GetUniqueIdentifier()].Keys)
            {
                nodes.Add(this.nodes[nodeId]);
            };
            return nodes;
        }
    }
}
