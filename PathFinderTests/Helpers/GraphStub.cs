using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTests.Helpers
{
    internal class GraphStub : NodeGraphWithPathFinding
    {
        public override int GetHeuristicDistanceToGoal(INode startNode, INode destinationNode)
        {
            return 1;
        }
    }
}
