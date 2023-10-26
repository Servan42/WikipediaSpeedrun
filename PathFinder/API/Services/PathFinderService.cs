using PathFinder.API.Interfaces;
using PathFinder.SPI.Interfaces;

namespace PathFinder.API.Services
{
    public class PathFinderService : IPathFinderService
    {
        private readonly IGraphForPathFinding graph;

        public PathFinderService(IGraphForPathFinding graph)
        {
            this.graph = graph;
        }

        public Dictionary<string, INodeForPathFinding> BreadthFirstSearch(INodeForPathFinding firstNode, INodeForPathFinding goalNode)
        {
            Queue<INodeForPathFinding> fronteir = new Queue<INodeForPathFinding>();
            fronteir.Enqueue(firstNode);
            Dictionary<string, INodeForPathFinding> keyCameFromValue = new Dictionary<string, INodeForPathFinding>();
            keyCameFromValue.Add(firstNode.GetNodeIdentifier(), null);

            while (fronteir.Count > 0)
            {
                INodeForPathFinding currentNode = fronteir.Dequeue();
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }

                foreach (INodeForPathFinding neighboor in this.graph.GetNeighbors(currentNode))
                {
                    if (keyCameFromValue.ContainsKey(neighboor.GetNodeIdentifier()))
                        continue;

                    fronteir.Enqueue(neighboor);
                    keyCameFromValue.Add(neighboor.GetNodeIdentifier(), currentNode);
                }
            }

            return keyCameFromValue;
        }

        public Dictionary<string, INodeForPathFinding> DijkstrasAlgorithm(INodeForPathFinding firstNode, INodeForPathFinding goalNode)
        {
            PriorityQueue<INodeForPathFinding, int> fronteir = new PriorityQueue<INodeForPathFinding, int>();
            fronteir.Enqueue(firstNode, 0);

            Dictionary<string, INodeForPathFinding> keyCameFromValue = new Dictionary<string, INodeForPathFinding>();
            keyCameFromValue.Add(firstNode.GetNodeIdentifier(), null);

            Dictionary<string, int> costSoFar = new Dictionary<string, int>();
            costSoFar.Add(firstNode.GetNodeIdentifier(), 0);

            while (fronteir.Count > 0)
            {
                INodeForPathFinding currentNode = fronteir.Dequeue();
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }

                foreach (INodeForPathFinding neighboor in this.graph.GetNeighbors(currentNode))
                {
                    string neigboorId = neighboor.GetNodeIdentifier();
                    int newCost = costSoFar[currentNode.GetNodeIdentifier()] + this.graph.GetEdgeWeight(currentNode, neighboor);
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

        public Dictionary<string, INodeForPathFinding> HeuristicSearch(INodeForPathFinding firstNode, INodeForPathFinding goalNode)
        {
            PriorityQueue<INodeForPathFinding, int> fronteir = new PriorityQueue<INodeForPathFinding, int>();
            fronteir.Enqueue(firstNode, 0);

            Dictionary<string, INodeForPathFinding> keyCameFromValue = new Dictionary<string, INodeForPathFinding>();
            keyCameFromValue.Add(firstNode.GetNodeIdentifier(), null);

            while (fronteir.Count > 0)
            {
                INodeForPathFinding currentNode = fronteir.Dequeue();
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }

                foreach (INodeForPathFinding neighboor in this.graph.GetNeighbors(currentNode))
                {
                    string neigboorId = neighboor.GetNodeIdentifier();
                    if (keyCameFromValue.ContainsKey(neigboorId))
                        continue;

                    keyCameFromValue.Add(neigboorId, currentNode);

                    var priority = this.graph.GetHeuristicDistanceToGoal(neighboor, goalNode);
                    fronteir.Enqueue(neighboor, priority);
                }
            }

            return keyCameFromValue;
        }

        public Dictionary<string, INodeForPathFinding> AstarAlgorithm(INodeForPathFinding firstNode, INodeForPathFinding goalNode)
        {
            PriorityQueue<INodeForPathFinding, int> fronteir = new PriorityQueue<INodeForPathFinding, int>();
            fronteir.Enqueue(firstNode, 0);

            Dictionary<string, INodeForPathFinding> keyCameFromValue = new Dictionary<string, INodeForPathFinding>();
            keyCameFromValue.Add(firstNode.GetNodeIdentifier(), null);

            Dictionary<string, int> costSoFar = new Dictionary<string, int>();
            costSoFar.Add(firstNode.GetNodeIdentifier(), 0);

            while (fronteir.Count > 0)
            {
                INodeForPathFinding currentNode = fronteir.Dequeue();
                if (currentNode.GetNodeIdentifier() == goalNode.GetNodeIdentifier()) { break; }

                foreach (INodeForPathFinding neighboor in this.graph.GetNeighbors(currentNode))
                {
                    string neigboorId = neighboor.GetNodeIdentifier();
                    int newCost = costSoFar[currentNode.GetNodeIdentifier()] + this.graph.GetEdgeWeight(currentNode, neighboor);
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

                    var priority = newCost + this.graph.GetHeuristicDistanceToGoal(neighboor,goalNode);
                    fronteir.Enqueue(neighboor, priority);
                }
            }

            return keyCameFromValue;
        }

        public List<INodeForPathFinding> GetPath(Dictionary<string, INodeForPathFinding> keyCameFromValue, INodeForPathFinding startNode, INodeForPathFinding goalNode)
        {
            var path = new List<INodeForPathFinding>();
            INodeForPathFinding current = goalNode;
            while (current.GetNodeIdentifier() != startNode.GetNodeIdentifier())
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
