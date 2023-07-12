namespace PathFinderAdapter.Interfaces
{
    public interface IPathFinderService
    {
        public Dictionary<string, INode> BreadthFirstSearch(INode firstNode, INode goalNode);
        public Dictionary<string, INode> DijkstrasAlgorithm(INode firstNode, INode goalNode);
    }
}
