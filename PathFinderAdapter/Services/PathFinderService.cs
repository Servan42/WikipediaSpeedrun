using PathFinderAdapter.Interfaces;

namespace PathFinderAdapter.Services
{
    public class PathFinderService : IPathFinderService
    {
        public Dictionary<string, INode> BreadthFirstSearch(INode firstNode, INode goalNode)
        {
            Queue<INode> fronteir = new Queue<INode>();
            fronteir.Enqueue(firstNode);
            Dictionary<string, INode> keyCameFromValue = new Dictionary<string, INode>();

            while(fronteir.Count > 0)
            {
                INode currentNode = fronteir.Dequeue();
                Console.WriteLine(currentNode.GetNodeIdentifier());
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }
                
                foreach(INode neighboor in currentNode.GetNeighbors())
                {
                    if (keyCameFromValue.ContainsKey(neighboor.GetNodeIdentifier()))
                        continue;

                    fronteir.Enqueue(neighboor);
                    keyCameFromValue.Add(neighboor.GetNodeIdentifier(), currentNode);
                }
            }

            return keyCameFromValue;
        }
    }
}
