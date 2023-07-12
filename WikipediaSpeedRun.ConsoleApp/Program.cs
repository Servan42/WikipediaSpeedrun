// See https://aka.ms/new-console-template for more information

using PathFinderAdapter.Interfaces;
using PathFinderAdapter.Services;
using WikipediaSpeedRunLib;
using static System.Net.Mime.MediaTypeNames;

HttpClientAdapter httpClient = new HttpClientAdapter();

string goal = "https://en.wikipedia.org/wiki/BBC";
//string start = "https://en.wikipedia.org/wiki/Dunloy_railway_station";
string start = "https://en.wikipedia.org/wiki/Lough_Finn";
WikipediaPage startPage = await WikipediaPage.Build(start, httpClient);

IPathFinderService pathFinderService = new PathFinderService();

DateTime startTime = DateTime.Now;
var cameFrom2 = pathFinderService.DijkstrasAlgorithm(startPage, new WikipediaPage(goal, httpClient));
Console.WriteLine("\nFOUND");
Console.WriteLine(DateTime.Now - startTime);

startTime = DateTime.Now;
var cameFrom = pathFinderService.BreadthFirstSearch(startPage, new WikipediaPage(goal, httpClient));
Console.WriteLine("\nFOUND");
Console.WriteLine(DateTime.Now - startTime);


string currentUrl = goal;
while (currentUrl != start)
{
    Console.WriteLine(currentUrl);
    currentUrl = cameFrom2[currentUrl].GetNodeIdentifier();
}

Console.WriteLine("---");

currentUrl = goal;
while (currentUrl != start)
{
    Console.WriteLine(currentUrl);
    currentUrl = cameFrom[currentUrl].GetNodeIdentifier();
}

Console.ReadLine();

