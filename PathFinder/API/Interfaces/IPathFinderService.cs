using PathFinder.SPI.Interfaces;

namespace PathFinder.API.Interfaces
{
    public interface IPathFinderService
    {
        public Dictionary<string, INodeForPathFinding> BreadthFirstSearch(INodeForPathFinding firstNode, INodeForPathFinding goalNode);
        public Dictionary<string, INodeForPathFinding> DijkstrasAlgorithm(INodeForPathFinding firstNode, INodeForPathFinding goalNode);
        public Dictionary<string, INodeForPathFinding> HeuristicSearch(INodeForPathFinding firstNode, INodeForPathFinding goalNode);
        public Dictionary<string, INodeForPathFinding> AstarAlgorithm(INodeForPathFinding firstNode, INodeForPathFinding goalNode);
        public List<INodeForPathFinding> GetPath(Dictionary<string, INodeForPathFinding> keyCameFromValue, INodeForPathFinding startNode, INodeForPathFinding goalNode);
    }
}
