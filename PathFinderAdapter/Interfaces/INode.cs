namespace PathFinderAdapter.Interfaces
{
    public interface INode
    {
        public string GetNodeIdentifier();
        public IEnumerable<INode> GetNeighbors();
        public int GetCostOfCrossingThisNode();
        public int GetHeuristicDistanceToGoal(INode goalNode);
    }
}
