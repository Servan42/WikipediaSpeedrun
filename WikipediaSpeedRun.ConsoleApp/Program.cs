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
        IWikipediaSpeedrunService wikipediaSpeedrunService = new WikipediaSpeedrunService(httpClient);

        //string goal = "https://en.wikipedia.org/wiki/Country";
        string goal = "https://en.wikipedia.org/wiki/Livestreaming";
        string start = "https://en.wikipedia.org/wiki/Zerator";
        //string goal = "https://en.wikipedia.org/wiki/BBC";
        //string start = "https://en.wikipedia.org/wiki/Dunloy_railway_station";

        var wikiNodeGraph = await wikipediaSpeedrunService.BuildNodeGraph(start, goal);
        var path = wikiNodeGraph.BreadthFirstSearch(new Node(start.GetShortenedUrl()), new Node(goal.GetShortenedUrl()));
        Console.WriteLine(string.Join("\n", path.Select(n => n.GetUniqueIdentifier())));
    }
}