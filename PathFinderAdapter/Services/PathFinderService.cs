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
                Console.Write($"FRONTEIR: {fronteir.Count}\t");
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

        public Dictionary<string, INode> DijkstrasAlgorithm(INode firstNode, INode goalNode)
        {
            PriorityQueue<INode, int> fronteir = new PriorityQueue<INode, int>();
            fronteir.Enqueue(firstNode, 0);
            Dictionary<string, INode> keyCameFromValue = new Dictionary<string, INode>();
            Dictionary<string, int> costSoFar = new Dictionary<string, int>();
            costSoFar.Add(firstNode.GetNodeIdentifier(), 0);

            while (fronteir.Count > 0)
            {
                Console.Write($"FRONTEIR: {fronteir.Count}\t");
                INode currentNode = fronteir.Dequeue();
                Console.WriteLine(currentNode.GetNodeIdentifier());
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }

                foreach (INode neighboor in currentNode.GetNeighbors())
                {
                    string neigboorId = neighboor.GetNodeIdentifier();
                    int newCost = costSoFar[currentNode.GetNodeIdentifier()] + neighboor.GetCostOfCrossingThisNode();
                    if (keyCameFromValue.ContainsKey(neigboorId)
                        && newCost >= costSoFar[neigboorId])
                        continue;

                    if(costSoFar.ContainsKey(neigboorId))
                    {
                        keyCameFromValue[neigboorId] = currentNode;
                        costSoFar[neigboorId] = newCost;
                    }
                    else
                    {
                        keyCameFromValue.Add(neigboorId, currentNode);
                        costSoFar.Add(neigboorId, newCost);
                    }

                    fronteir.Enqueue(neighboor, newCost);
                }
            }

            return keyCameFromValue;
        }
    }
}
