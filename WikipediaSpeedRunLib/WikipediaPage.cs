using PathFinderAdapter.Interfaces;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;

namespace WikipediaSpeedRunLib
{
    public class WikipediaPage : INode
    {
        private readonly string url;
        private readonly IHttpClientAdapter httpClient;
        private bool isPageLoaded;

        public WikipediaPage(string url, IHttpClientAdapter httpClient)
        {
            this.url = url;
            this.httpClient = httpClient;
            this.isPageLoaded = false;
        }

        public static async Task<WikipediaPage> Build(string url, IHttpClientAdapter httpClient)
        {
            WikipediaPage page = new WikipediaPage(url, httpClient);
            await page.LoadPageInfos();
            return page;
        }

        public Dictionary<string, LinkInfos> ValuableLinks { get; set; }

        public string GetNodeIdentifier()
        {
            return url;
        }

        public IEnumerable<INode> GetNeighbors()
        {
            if(!this.isPageLoaded)
            {
                LoadPageInfos().Wait();
            }

            List<WikipediaPage> neighbors = new();
            foreach(var link in this.ValuableLinks)
            {
                WikipediaPage neighboor = new WikipediaPage(link.Value.Url, this.httpClient);
                neighbors.Add(neighboor);
            }

            return neighbors;
        }

        public async Task LoadPageInfos()
        {
            string html = await httpClient.GetStringAsync(this.url);
            ValuableLinks = new Dictionary<string, LinkInfos>();
            html = html.SubstringBetweenTwoTags("<div id=\"mw-content-text\"", "</main>");
            if (html == "") throw new ArgumentException("Could not find the main div of article", "html");
            List<string> htmlLinks = LoadHtmlLinkElementsFromHtml(html);
            htmlLinks.ForEach(a =>
            {
                var linkInfo = new LinkInfos(a);
                if (!linkInfo.ShouldIgnore && !ValuableLinks.ContainsKey(linkInfo.Url)) ValuableLinks.Add(linkInfo.Url, linkInfo);
            });

            this.isPageLoaded = true;
        }

        private List<string> LoadHtmlLinkElementsFromHtml(string html)
        {
            List<string> htmlLinkElements = new List<string>();
            string[] splitLeftTag = html.Split("<a ");
            foreach (string s in splitLeftTag.Skip(1))
            {
                htmlLinkElements.Add("<a " + s.Substring(0, s.IndexOf("</a>")) + "</a>");
            }
            return htmlLinkElements;
        }
    }
}