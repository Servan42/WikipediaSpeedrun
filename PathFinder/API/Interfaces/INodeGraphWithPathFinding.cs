using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.API.Interfaces
{
    public interface INodeGraphWithPathFinding : INodeGraph
    {
        public List<INode> BreadthFirstSearch(INode firstNode, INode goalNode);
        public List<INode> DijkstrasAlgorithm(INode firstNode, INode goalNode);
        public List<INode> HeuristicSearch(INode firstNode, INode goalNode);
        public List<INode> AstarAlgorithm(INode firstNode, INode goalNode);
    }
}
