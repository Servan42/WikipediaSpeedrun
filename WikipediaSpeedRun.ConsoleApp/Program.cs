// See https://aka.ms/new-console-template for more information

using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using System.Diagnostics;
using System.Linq;
using WikipediaSpeedRunLib;

internal class Program
{
    private static async Task Main(string[] args)
    {
        HttpClientAdapter httpClient = new HttpClientAdapter();

        string goal = "https://en.wikipedia.org/wiki/Country";
        //string goal = "https://en.wikipedia.org/wiki/Livestreaming";
        string start = "https://en.wikipedia.org/wiki/Zerator";
        //string goal = "https://en.wikipedia.org/wiki/BBC";
        //string start = "https://en.wikipedia.org/wiki/Dunloy_railway_station";

        INodeGraphWithPathFinding basicGraph = new BasicGraph();

        var startNode = new Node(start);
        var goalNode = new Node(goal);

        basicGraph.AddNode(startNode);
        basicGraph.AddNode(goalNode);

        List<string> currentDepthNodes = new List<string> { startNode.GetUniqueIdentifier() };
        int i = 0;
        Stopwatch stopwatch = new();
        stopwatch.Start();
        while (currentDepthNodes.Count > 0)
        {
            List<string> currentDepthNodesTemp = new List<string>();

            foreach (string node in currentDepthNodes)
            {
                WikipediaPage page = await WikipediaPage.Build(node, httpClient);

                foreach (var newUrl in page.ValuableLinks.Select(v => v.Value.Url))
                {
                    var newNode = new Node(newUrl);
                    if (basicGraph.AddNode(newNode))
                    {
                        currentDepthNodesTemp.Add(newUrl);
                        Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                        Console.Write($"Adding {newUrl.Replace("https://en.wikipedia.org/wiki/", "")}".PadRight(110).Substring(0, 110));
                    }

                    if (newUrl == node) // prevent cycles
                        continue;

                    basicGraph.AddUnidirectionalEdge(basicGraph.GetNode(node), newNode, 1);

                    if (newUrl == goal)
                        goto End;
                }
            }

            currentDepthNodes = currentDepthNodesTemp;

            i++;
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
            Console.WriteLine($"Depth {i}. Added {currentDepthNodes.Count} new nodes. Nodes:{basicGraph.GetNodesCount()} TotalExpected: {stopwatch.Elapsed / basicGraph.GetNodesCount() * 59237748.0}".PadRight(110).Substring(0, 110));
        }

        End:

        var path = basicGraph.AstarAlgorithm(startNode, goalNode);

        Console.WriteLine("PATH:");
        Console.WriteLine(string.Join('\n', path.Select(n => n.GetUniqueIdentifier())));

        Console.ReadLine();
    }
}

internal class BasicGraph : NodeGraphWithPathFinding
{
    public override int GetHeuristicDistanceToGoal(INode startNode, INode destinationNode)
    {
        return 1;
    }
}