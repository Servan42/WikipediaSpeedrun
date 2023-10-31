// See https://aka.ms/new-console-template for more information

using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using System.Diagnostics;
using System.Linq;
using WikipediaSpeedRunLib.API.Interfaces;
using WikipediaSpeedRunLib.API.Services;
using WikipediaSpeedRunLib.Extensions;
using WikipediaSpeedRunLib.Model;
using WikipediaSpeedRunLib.SPI.Interfaces;
using WikipediaSpeedRunLib.SPI.SelfServices;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IHttpClientAdapter httpClient = new HttpClientAdapter();
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IWikipediaSpeedrunService wikipediaSpeedrunService = new WikipediaSpeedrunService(httpClient, cancellationTokenSource.Token);
        IWikiGraphPersistenceFileService wikiGraphPersistenceFileService = new WikiGraphPersistenceFileService();

        string start = "https://en.wikipedia.org/wiki/Zerator";

        Stopwatch stopwatch = new();
        stopwatch.Start();
        var graphfromfile = wikiGraphPersistenceFileService.GetWikiGraphFromFile("F:\\Servan\\Graph.txt");
        stopwatch.Stop();
        Console.WriteLine($"Read from file: {stopwatch.Elapsed}");

        int initalNumberOfNodes = graphfromfile.GetNodesCount();
        int numberOfLoadedNodes = graphfromfile.GetNumberofLoadedNodes();
        Console.WriteLine($"Current number of nodes: {initalNumberOfNodes} ({(initalNumberOfNodes / 6736719.0) * 100.0:#.##}% of Wikipedia). Loaded: {((double)numberOfLoadedNodes / (double)initalNumberOfNodes)*100.0:0.##}% ({(numberOfLoadedNodes / 6736719.0) * 100.0:0.##}%)");

        stopwatch.Restart();
        var wikiNodeGraphTask = Task.Run(() => wikipediaSpeedrunService.AppendToNodeGraph(graphfromfile, start, null));
        Console.WriteLine("Press a key to stop downloading wikipedia...");
        Console.ReadKey();
        cancellationTokenSource.Cancel();
        var wikiNodeGraph = await wikiNodeGraphTask;
        stopwatch.Stop();
        Console.WriteLine($"Search from the internet: {stopwatch.Elapsed}");

        stopwatch.Restart();
        wikiGraphPersistenceFileService.SaveWikiGraphToFile(wikiNodeGraph, "F:\\Servan\\Graph.txt");
        stopwatch.Stop();
        Console.WriteLine($"Save graph to file: {stopwatch.Elapsed}");
        Console.WriteLine($"New nodes: {wikiNodeGraph.GetNodesCount() - initalNumberOfNodes}");

        string goal;

        while (true)
        {
            Console.Write("Enter start: ");
            start = Console.ReadLine();
            Console.Write("Enter goal: ");
            goal = Console.ReadLine();
            Pathfinding(goal, start, stopwatch, wikiNodeGraph);
        }
    }

    private static void Pathfinding(string goal, string start, Stopwatch stopwatch, WikiNodeGraph wikiNodeGraph)
    {
        if (wikiNodeGraph.GetNode(goal.GetShortenedUrl()) == null)
        {
            Console.WriteLine($"Node {goal} does not exist in current graph");
            return;
        }

        if (wikiNodeGraph.GetNode(start.GetShortenedUrl()) == null)
        {
            Console.WriteLine($"Node {start} does not exist in current graph");
            return;
        }

        try
        {
            stopwatch.Restart();
            var path = wikiNodeGraph.BreadthFirstSearch(new WikiNode(start.GetShortenedUrl()), new WikiNode(goal.GetShortenedUrl()));
            stopwatch.Stop();
            Console.WriteLine($"BreadthFirstSearch: {stopwatch.Elapsed}");

            Console.WriteLine("PATH:");
            Console.WriteLine(string.Join("\n", path.Select(n => n.GetUniqueIdentifier())));
            Console.WriteLine();
        }
        catch (Exception)
        {
            Console.WriteLine("No path could be found.\n");
        }
    }
}