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
            keyCameFromValue.Add(firstNode.GetNodeIdentifier(), null);

            while (fronteir.Count > 0)
            {
                Console.Write($"FRONTEIR: {fronteir.Count}\t");
                INode currentNode = fronteir.Dequeue();
                Console.WriteLine(currentNode.GetNodeIdentifier());
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }

                foreach (INode neighboor in currentNode.GetNeighbors())
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
            keyCameFromValue.Add(firstNode.GetNodeIdentifier(), null);

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

                    if (costSoFar.ContainsKey(neigboorId))
                    {
                        keyCameFromValue[neigboorId] = currentNode;
                        costSoFar[neigboorId] = newCost;
                    }
                    else
                    {
                        keyCameFromValue.Add(neigboorId, currentNode);
                        costSoFar.Add(neigboorId, newCost);
                    }

                    var priority = newCost;
                    fronteir.Enqueue(neighboor, priority);
                }
            }

            return keyCameFromValue;
        }

        public Dictionary<string, INode> HeuristicSearch(INode firstNode, INode goalNode)
        {
            PriorityQueue<INode, int> fronteir = new PriorityQueue<INode, int>();
            fronteir.Enqueue(firstNode, 0);

            Dictionary<string, INode> keyCameFromValue = new Dictionary<string, INode>();
            keyCameFromValue.Add(firstNode.GetNodeIdentifier(), null);

            while (fronteir.Count > 0)
            {
                INode currentNode = fronteir.Dequeue();
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }

                foreach (INode neighboor in currentNode.GetNeighbors())
                {
                    string neigboorId = neighboor.GetNodeIdentifier();
                    if (keyCameFromValue.ContainsKey(neigboorId))
                        continue;

                    keyCameFromValue.Add(neigboorId, currentNode);

                    var priority = neighboor.GetHeuristicDistanceToGoal(goalNode);
                    fronteir.Enqueue(neighboor, priority);
                }
            }

            return keyCameFromValue;
        }

        public Dictionary<string, INode> AstarAlgorithm(INode firstNode, INode goalNode)
        {
            PriorityQueue<INode, int> fronteir = new PriorityQueue<INode, int>();
            fronteir.Enqueue(firstNode, 0);

            Dictionary<string, INode> keyCameFromValue = new Dictionary<string, INode>();
            keyCameFromValue.Add(firstNode.GetNodeIdentifier(), null);

            Dictionary<string, int> costSoFar = new Dictionary<string, int>();
            costSoFar.Add(firstNode.GetNodeIdentifier(), 0);

            while (fronteir.Count > 0)
            {
                INode currentNode = fronteir.Dequeue();
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }

                foreach (INode neighboor in currentNode.GetNeighbors())
                {
                    string neigboorId = neighboor.GetNodeIdentifier();
                    int newCost = costSoFar[currentNode.GetNodeIdentifier()] + neighboor.GetCostOfCrossingThisNode();
                    if (keyCameFromValue.ContainsKey(neigboorId)
                        && newCost >= costSoFar[neigboorId])
                        continue;

                    if (costSoFar.ContainsKey(neigboorId))
                    {
                        keyCameFromValue[neigboorId] = currentNode;
                        costSoFar[neigboorId] = newCost;
                    }
                    else
                    {
                        keyCameFromValue.Add(neigboorId, currentNode);
                        costSoFar.Add(neigboorId, newCost);
                    }

                    var priority = newCost + currentNode.GetHeuristicDistanceToGoal(goalNode);
                    fronteir.Enqueue(neighboor, priority);
                }
            }

            return keyCameFromValue;
        }

        public List<INode> GetPath(Dictionary<string, INode> keyCameFromValue, INode startNode, INode goalNode)
        {
            var path = new List<INode>();
            INode current = goalNode;
            while(current.GetNodeIdentifier() != startNode.GetNodeIdentifier())
            {
                path.Add(current);
                current = keyCameFromValue[current.GetNodeIdentifier()];
            }
            
            path.Add(startNode);
            path.Reverse();

            return path;
        }
    }
}
