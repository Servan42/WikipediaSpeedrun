// See https://aka.ms/new-console-template for more information

using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using PathFinder.SPI.DefaultImplementation;
using System.Linq;
using WikipediaSpeedRunLib;
using static System.Net.Mime.MediaTypeNames;

HttpClientAdapter httpClient = new HttpClientAdapter();

string goal = "https://en.wikipedia.org/wiki/Manor_house";
//string goal = "https://en.wikipedia.org/wiki/BBC";
//string start = "https://en.wikipedia.org/wiki/Dunloy_railway_station";
string start = "https://en.wikipedia.org/wiki/Zerator";

BasicGraph basicGraph = new BasicGraph();
WikipediaPage startPage = await WikipediaPage.Build(start, httpClient);
//WikipediaPage goalPage = await WikipediaPage.Build(goal, httpClient, basicGraph.AddNode, basicGraph.AddBidirectionalEdge);


var startNode = new BasicNode(start);
var goalNode = new BasicNode(goal);

basicGraph.AddNode(startNode);
basicGraph.AddNode(goalNode);

List<string> currentDepthNodes = new List<string> { startNode.GetNodeIdentifier() };
int i = 0;
while (!currentDepthNodes.Contains(goalNode.GetNodeIdentifier()))
{
    List<string> currentDepthNodesTemp = new List<string>();

    foreach (string node in currentDepthNodes)
    {
        WikipediaPage page = await WikipediaPage.Build(node, httpClient);
        foreach (var newUrl in page.ValuableLinks.Select(v => v.Value.Url))
        {
            currentDepthNodesTemp.Add(newUrl);
            var newNode = new BasicNode(newUrl);
            if (basicGraph.AddNode(newNode))
            {
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                Console.Write($"Adding {newUrl.Replace("\n", "").Replace("\r", "")}");
                basicGraph.AddUnidirectionalEdge(basicGraph.Nodes[node], newNode, 1);
            }
        }
    }

    i++;
    currentDepthNodes = currentDepthNodesTemp;
    
    Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
    Console.WriteLine($"Depth {i}. Added {currentDepthNodes.Count} new nodes");
}

IPathFinderService pathFinderService = new PathFinderService(basicGraph);

var path = pathFinderService.GetPath(pathFinderService.AstarAlgorithm(startNode, goalNode), startNode, goalNode);

Console.WriteLine(string.Join('\n', path));

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

