// See https://aka.ms/new-console-template for more information

using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using PathFinder.SPI.DefaultImplementation;
using System.Diagnostics;
using System.Linq;
using WikipediaSpeedRunLib;
using static System.Net.Mime.MediaTypeNames;

HttpClientAdapter httpClient = new HttpClientAdapter();

string goal = "https://en.wikipedia.org/wiki/Country";
//string goal = "https://en.wikipedia.org/wiki/Livestreaming";
string start = "https://en.wikipedia.org/wiki/Zerator";
//string goal = "https://en.wikipedia.org/wiki/BBC";
//string start = "https://en.wikipedia.org/wiki/Dunloy_railway_station";

BasicGraph basicGraph = new BasicGraph();
WikipediaPage startPage = await WikipediaPage.Build(start, httpClient);
//WikipediaPage goalPage = await WikipediaPage.Build(goal, httpClient, basicGraph.AddNode, basicGraph.AddBidirectionalEdge);


var startNode = new BasicNode(start);
var goalNode = new BasicNode(goal);

basicGraph.AddNode(startNode);
basicGraph.AddNode(goalNode);

List<string> currentDepthNodes = new List<string> { startNode.GetNodeIdentifier() };
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
            var newNode = new BasicNode(newUrl);
            if (basicGraph.AddNode(newNode))
            {
                currentDepthNodesTemp.Add(newUrl);
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                Console.Write($"Adding {newUrl.Replace("https://en.wikipedia.org/wiki/", "")}".PadRight(110).Substring(0, 110));
            }

            if (newUrl == node) // prevent cycles
                continue;

            basicGraph.AddUnidirectionalEdge(basicGraph.Nodes[node], newNode, 1);

            if (newUrl == goal)
                goto End;
        }
    }

    currentDepthNodes = currentDepthNodesTemp;

    i++;
    Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
    Console.WriteLine($"Depth {i}. Added {currentDepthNodes.Count} new nodes. Nodes:{basicGraph.Nodes.Count} TotalExpected: {(stopwatch.Elapsed / basicGraph.Nodes.Count) * 59237748.0}".PadRight(110).Substring(0, 110));
}

End:

IPathFinderService pathFinderService = new PathFinderService(basicGraph);

var path = pathFinderService.GetPath(pathFinderService.AstarAlgorithm(startNode, goalNode), startNode, goalNode);

Console.WriteLine("PATH:");
Console.WriteLine(string.Join('\n', path.Select(n => n.GetNodeIdentifier())));

//DateTime startTime = DateTime.Now;
//var cameFrom2 = pathFinderService.DijkstrasAlgorithm(startPage, goalPage);
//Console.WriteLine("\nFOUND");
//Console.WriteLine(DateTime.Now - startTime);

////DateTime startTime = DateTime.Now;
////var cameFrom = pathFinderService.BreadthFirstSearch(startPage, new WikipediaPage(goal, httpClient));
////Console.WriteLine("\nFOUND");
////Console.WriteLine(DateTime.Now - startTime);


//string currentUrl = goal;
//while (currentUrl != start)
//{
//    Console.WriteLine(currentUrl);
//    currentUrl = cameFrom2[currentUrl].GetNodeIdentifier();
//}

//Console.WriteLine("---");

////string currentUrl = goal;
////while (currentUrl != start)
////{
////    Console.WriteLine(currentUrl);
////    currentUrl = cameFrom[currentUrl].GetNodeIdentifier();
////}

Console.ReadLine();

