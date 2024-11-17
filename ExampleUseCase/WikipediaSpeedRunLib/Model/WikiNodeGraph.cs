using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaSpeedRunLib.Model
{
    public class WikiNodeGraph : NodeGraphWithPathFinding
    {
        public override int GetHeuristicDistanceToGoal(INode startNode, INode destinationNode)
        {
            return 0;
        }

        public Dictionary<string, Dictionary<string, int>> GetAdjacencyList()
        {
            return weightedAdjacencyList;
        }

        public IEnumerable<string> GetLoadedNodes()
        {
            return this.nodes.Values.Where(n => ((WikiNode)n).IsNodeFullyLoaded).Select(n => n.GetUniqueIdentifier());
        }

        public int GetNumberofLoadedNodes()
        {
            return this.nodes.Count(n => ((WikiNode)n.Value).IsNodeFullyLoaded);
        }
    }
}
