using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.API.Interfaces
{
    public interface INodeGraph
    {
        public bool AddNode(INode node);
        public bool AddNodes(List<INode> nodes);
        public bool AddBidirectionalEdge(INode startNode, INode destinationNode, int weight);
        public bool AddUnidirectionalEdge(INode startNode, INode destinationNode, int weight);
        public INode? GetNode(string uniqueIdentifier);
        public int GetNodesCount();
        public int GetEdgeWeight(INode startNode, INode destinationNode);
        public IEnumerable<INode> GetNeighbors(INode node);
    }
}
