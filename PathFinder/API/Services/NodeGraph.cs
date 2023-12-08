using PathFinder.API.Exceptions;
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

        public bool AddNode(INode node)
        {
            if (nodes.ContainsKey(node.GetUniqueIdentifier()))
                return false;

            nodes.Add(node.GetUniqueIdentifier(), node);
            weightedAdjacencyList.Add(node.GetUniqueIdentifier(), new Dictionary<string, int>());
            return true;
        }

        public bool AddBidirectionalEdge(INode startNode, INode destinationNode, int weight)
        {
            bool successStartToDest = AddUnidirectionalEdge(startNode, destinationNode, 5);
            if (!successStartToDest) return false;
            bool successDestToStart = AddUnidirectionalEdge(destinationNode, startNode, 5);
            if (!successDestToStart)
                RemoveEdge(startNode, destinationNode);

            return successStartToDest && successDestToStart;
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
            string startNodeIdentifier = startNode.GetUniqueIdentifier();
            string destinationNodeIdentifier = destinationNode.GetUniqueIdentifier();

            if (GetNode(startNodeIdentifier) == null)
                throw new NodeGraphException($"Start node '{startNodeIdentifier}' is not present in graph");

            if (GetNode(destinationNodeIdentifier) == null)
                throw new NodeGraphException($"Destination node '{destinationNodeIdentifier}' is not present in graph");

            if(!weightedAdjacencyList.ContainsKey(startNodeIdentifier) || !weightedAdjacencyList[startNodeIdentifier].ContainsKey(destinationNodeIdentifier))
                throw new NodeGraphException($"Edge from node '{startNodeIdentifier}' to node '{destinationNodeIdentifier}' does not exist in graph");

            return weightedAdjacencyList[startNodeIdentifier][destinationNodeIdentifier];
        }

        public IEnumerable<INode> GetNeighbors(INode node)
        {
            string nodeIdentifier = node.GetUniqueIdentifier();

            if (GetNode(nodeIdentifier) == null)
                return Enumerable.Empty<INode>();

            List<INode> nodes = new();
            foreach (var nodeId in weightedAdjacencyList[node.GetUniqueIdentifier()].Keys)
            {
                nodes.Add(this.nodes[nodeId]);
            };
            return nodes;
        }

        // TODO: Implement public removal methods for eveything
        private void RemoveEdge(INode startNode, INode destinationNode)
        {
            this.weightedAdjacencyList[startNode.GetUniqueIdentifier()].Remove(destinationNode.GetUniqueIdentifier());
        }
    }
}
