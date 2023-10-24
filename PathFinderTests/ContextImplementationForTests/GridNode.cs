using PathFinder.SPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTests.ContextImplementationForTests
{
    internal class GridNode : INode
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int DifficultyToCross { get; set; }

        public string GetNodeIdentifier()
        {
            return $"{PosX};{PosY}";
        }

        public int GetCostOfCrossingThisNode()
        {
            return DifficultyToCross;
        }

        public int GetHeuristicDistanceToGoal(INode goalNode)
        {
            GridNode goalGridNode = (GridNode) goalNode;
            return Math.Abs(goalGridNode.PosX - this.PosX) + Math.Abs(goalGridNode.PosY - this.PosY);
        }

        public IEnumerable<INode> GetNeighbors()
        {
            throw new NotImplementedException();
        }
    }
}
