using PathFinder.SPI.Interfaces;

namespace PathFinder.API.Interfaces
{
    public interface IPathFinderService
    {
        public Dictionary<string, INode> BreadthFirstSearch(INode firstNode, INode goalNode);
        public Dictionary<string, INode> DijkstrasAlgorithm(INode firstNode, INode goalNode);
        public Dictionary<string, INode> HeuristicSearch(INode firstNode, INode goalNode);
        public Dictionary<string, INode> AstarAlgorithm(INode firstNode, INode goalNode);
        public List<INode> GetPath(Dictionary<string, INode> keyCameFromValue, INode startNode, INode goalNode);
    }
}
