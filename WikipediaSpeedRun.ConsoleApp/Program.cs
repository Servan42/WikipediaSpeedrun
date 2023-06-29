// See https://aka.ms/new-console-template for more information

using WikipediaSpeedRunLib;
using static System.Net.Mime.MediaTypeNames;

HttpClient httpClient = new HttpClient();

string goal = "https://en.wikipedia.org/wiki/BBC";
string start = "https://en.wikipedia.org/wiki/Dunloy_railway_station";
LinkInfos current = new LinkInfos(start, "Dunloy railway station");

Queue<LinkInfos> fronteir = new Queue<LinkInfos>();
fronteir.Enqueue(current);
Dictionary<string, string> cameFrom = new();

while (fronteir.Count > 0)
{
    current = fronteir.Dequeue();
    WikipediaPage currentPage = new WikipediaPage();
    if (current.Url == goal) break;
    Console.WriteLine($"F:{fronteir.Count}\t\t Scanning {current.Url}...");
    currentPage.LoadLinksInfosFromHtml(await httpClient.GetStringAsync(current.Url));
    foreach (var next in currentPage.Links)
    {
        if (!cameFrom.ContainsKey(next.Key))
        {
            fronteir.Enqueue(next.Value);
            cameFrom.Add(next.Key, current.Url);
        }
    }
}

Console.WriteLine("FOUND");

string currentUrl = goal;
while (currentUrl != start)
{
    Console.WriteLine(currentUrl);
    currentUrl = cameFrom[currentUrl];
}



Console.ReadLine();

//foreach (var link in wikipediaPage.Links)
//{
//    Console.WriteLine(link.Value.HtmlLinkElement + "\n" + link.Value.Url + "\n" + link.Value.PageTitle + "\n");
//}
//Console.WriteLine(wikipediaPage.Links.Count);
