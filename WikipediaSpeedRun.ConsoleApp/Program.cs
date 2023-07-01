// See https://aka.ms/new-console-template for more information

using PathFinderAdapter.Interfaces;
using PathFinderAdapter.Services;
using WikipediaSpeedRunLib;
using static System.Net.Mime.MediaTypeNames;

HttpClientAdapter httpClient = new HttpClientAdapter();

string goal = "https://en.wikipedia.org/wiki/BBC";
string start = "https://en.wikipedia.org/wiki/Dunloy_railway_station";
WikipediaPage startPage = await WikipediaPage.Build(start, httpClient);

IPathFinderService pathFinderService = new PathFinderService();

var cameFrom = pathFinderService.BreadthFirstSearch(startPage, new WikipediaPage(goal, httpClient));

//Queue<LinkInfos> fronteir = new Queue<LinkInfos>();
//fronteir.Enqueue(current);
//Dictionary<string, string> cameFrom = new();

//while (fronteir.Count > 0)
//{
//    current = fronteir.Dequeue();
//    WikipediaPage currentPage = new WikipediaPage(start, httpClient);
//    if (current.Url == goal) break;
//    Console.WriteLine($"F:{fronteir.Count}\t\t Scanning {current.Url}...");
//    await currentPage.LoadPageInfos();
//    foreach (var next in currentPage.ValuableLinks)
//    {
//        if (!cameFrom.ContainsKey(next.Key))
//        {
//            fronteir.Enqueue(next.Value);
//            cameFrom.Add(next.Key, current.Url);
//        }
//    }
//}

Console.WriteLine("FOUND");

//string currentUrl = goal;
//while (currentUrl != start)
//{
//    Console.WriteLine(currentUrl);
//    currentUrl = cameFrom[currentUrl];
//}



Console.ReadLine();

//foreach (var link in wikipediaPage.Links)
//{
//    Console.WriteLine(link.Value.HtmlLinkElement + "\n" + link.Value.Url + "\n" + link.Value.PageTitle + "\n");
//}
//Console.WriteLine(wikipediaPage.Links.Count);
