using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.SPI.Interfaces
{
    public interface IGraphForPathFinding
    {
        public int GetEdgeWeight(INodeForPathFinding startNode, INodeForPathFinding destinationNode);
        public IEnumerable<INodeForPathFinding> GetNeighbors(INodeForPathFinding node);
        public int GetHeuristicDistanceToGoal(INodeForPathFinding startNode, INodeForPathFinding destinationNode);
    }
}
