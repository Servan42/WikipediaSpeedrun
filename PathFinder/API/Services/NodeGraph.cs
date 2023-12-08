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
        protected readonly Dictionary<string, INode> nodes;
        protected readonly Dictionary<string, Dictionary<string, int>> weightedAdjacencyList;

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

        public bool AddUnidirectionalEdge(INode startNode, INode destinationNode, int weight)
        {
            string startNodeIdentifier = startNode.GetUniqueIdentifier();
            string destinationNodeIdentifier = destinationNode.GetUniqueIdentifier();

            if (GetNode(startNodeIdentifier) == null || !this.weightedAdjacencyList.ContainsKey(startNodeIdentifier))
                return false;

            if (GetNode(destinationNodeIdentifier) == null)
                return false;

            if (this.weightedAdjacencyList[startNodeIdentifier].ContainsKey(destinationNodeIdentifier))
                return false;

            this.weightedAdjacencyList[startNodeIdentifier].Add(destinationNodeIdentifier, weight);
            return true;
        }

        public INode? GetNode(string uniqueIdentifier)
        {
            if (!this.nodes.ContainsKey(uniqueIdentifier))
                return null;

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
