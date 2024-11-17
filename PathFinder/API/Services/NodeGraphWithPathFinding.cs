using PathFinder.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.API.Services
{
    public abstract class NodeGraphWithPathFinding : NodeGraph, INodeGraphWithPathFinding
    {
        public abstract int GetHeuristicDistanceToGoal(INode startNode, INode destinationNode);

        public List<INode> BreadthFirstSearch(INode firstNode, INode goalNode)
        {
            Queue<INode> fronteir = new Queue<INode>();
            fronteir.Enqueue(firstNode);
            Dictionary<string, INode> keyCameFromValue = new Dictionary<string, INode>();
            keyCameFromValue.Add(firstNode.GetUniqueIdentifier(), null);

            while (fronteir.Count > 0)
            {
                INode currentNode = fronteir.Dequeue();
                if (currentNode.GetUniqueIdentifier() == goalNode.GetUniqueIdentifier()) { break; }

                foreach (INode neighboor in GetNeighbors(currentNode))
                {
                    if (keyCameFromValue.ContainsKey(neighboor.GetUniqueIdentifier()))
                        continue;

                    fronteir.Enqueue(neighboor);
                    keyCameFromValue.Add(neighboor.GetUniqueIdentifier(), currentNode);
                }
            }

            return GetPath(keyCameFromValue, firstNode, goalNode);
        }

        public List<INode> DijkstrasAlgorithm(INode firstNode, INode goalNode)
        {
            PriorityQueue<INode, int> fronteir = new PriorityQueue<INode, int>();
            fronteir.Enqueue(firstNode, 0);

            Dictionary<string, INode> keyCameFromValue = new Dictionary<string, INode>();
            keyCameFromValue.Add(firstNode.GetUniqueIdentifier(), null);

            Dictionary<string, int> costSoFar = new Dictionary<string, int>();
            costSoFar.Add(firstNode.GetUniqueIdentifier(), 0);

            while (fronteir.Count > 0)
            {
                INode currentNode = fronteir.Dequeue();
                if (currentNode.GetUniqueIdentifier() == goalNode.GetUniqueIdentifier()) { break; }

                foreach (INode neighboor in GetNeighbors(currentNode))
                {
                    string neigboorId = neighboor.GetUniqueIdentifier();
                    int newCost = costSoFar[currentNode.GetUniqueIdentifier()] + GetEdgeWeight(currentNode, neighboor);
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

            return GetPath(keyCameFromValue, firstNode, goalNode);
        }

        public List<INode> HeuristicSearch(INode firstNode, INode goalNode)
        {
            PriorityQueue<INode, int> fronteir = new PriorityQueue<INode, int>();
            fronteir.Enqueue(firstNode, 0);

            Dictionary<string, INode> keyCameFromValue = new Dictionary<string, INode>();
            keyCameFromValue.Add(firstNode.GetUniqueIdentifier(), null);

            while (fronteir.Count > 0)
            {
                INode currentNode = fronteir.Dequeue();
                if (currentNode.GetUniqueIdentifier() == goalNode.GetUniqueIdentifier()) { break; }

                foreach (INode neighboor in GetNeighbors(currentNode))
                {
                    string neigboorId = neighboor.GetUniqueIdentifier();
                    if (keyCameFromValue.ContainsKey(neigboorId))
                        continue;

                    keyCameFromValue.Add(neigboorId, currentNode);

                    var priority = GetHeuristicDistanceToGoal(neighboor, goalNode);
                    fronteir.Enqueue(neighboor, priority);
                }
            }

            return GetPath(keyCameFromValue, firstNode, goalNode);
        }

        public List<INode> AstarAlgorithm(INode firstNode, INode goalNode)
        {
            PriorityQueue<INode, int> fronteir = new PriorityQueue<INode, int>();
            fronteir.Enqueue(firstNode, 0);

            Dictionary<string, INode> keyCameFromValue = new Dictionary<string, INode>();
            keyCameFromValue.Add(firstNode.GetUniqueIdentifier(), null);

            Dictionary<string, int> costSoFar = new Dictionary<string, int>();
            costSoFar.Add(firstNode.GetUniqueIdentifier(), 0);

            while (fronteir.Count > 0)
            {
                INode currentNode = fronteir.Dequeue();
                if (currentNode.GetUniqueIdentifier() == goalNode.GetUniqueIdentifier()) { break; }

                foreach (INode neighboor in GetNeighbors(currentNode))
                {
                    string neigboorId = neighboor.GetUniqueIdentifier();
                    int newCost = costSoFar[currentNode.GetUniqueIdentifier()] + GetEdgeWeight(currentNode, neighboor);
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

                    var priority = newCost + GetHeuristicDistanceToGoal(neighboor, goalNode);
                    fronteir.Enqueue(neighboor, priority);
                }
            }

            return GetPath(keyCameFromValue, firstNode, goalNode);
        } 

        private List<INode> GetPath(Dictionary<string, INode> keyCameFromValue, INode startNode, INode goalNode)
        {
            var path = new List<INode>();
            INode current = goalNode;
            while (current.GetUniqueIdentifier() != startNode.GetUniqueIdentifier())
            {
                path.Add(current);
                current = keyCameFromValue[current.GetUniqueIdentifier()];
            }

            path.Add(startNode);
            path.Reverse();

            return path;
        }
    }
}
