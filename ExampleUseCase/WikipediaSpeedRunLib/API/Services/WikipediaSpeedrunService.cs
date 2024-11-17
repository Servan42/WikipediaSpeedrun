using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WikipediaSpeedRunLib.API.Interfaces;
using WikipediaSpeedRunLib.Extensions;
using WikipediaSpeedRunLib.Model;
using WikipediaSpeedRunLib.SPI.Interfaces;
using WikipediaSpeedRunLib.SPI.SelfServices;

namespace WikipediaSpeedRunLib.API.Services
{
    public class WikipediaSpeedrunService : IWikipediaSpeedrunService
    {
        private readonly IHttpClientAdapter httpClient;
        private readonly CancellationToken cancellationToken;

        public WikipediaSpeedrunService(IHttpClientAdapter httpClientAdapter, CancellationToken cancellationToken)
        {
            this.httpClient = httpClientAdapter;
            this.cancellationToken = cancellationToken;
        }

        public async Task<WikiNodeGraph> AppendToNodeGraph(WikiNodeGraph wikiNodeGraph, string startPageUrl, string? goalpageUrl = null, int maximumDepth = int.MaxValue, int maxLinksPerPage = int.MaxValue)
        {
            string startPageShortUrl = startPageUrl.GetShortenedUrl();
            var startNode = new WikiNode(startPageShortUrl);
            wikiNodeGraph.AddNode(startNode);

            string? goalPageShortUrl = null;
            if (!string.IsNullOrEmpty(goalpageUrl))
            {
                goalPageShortUrl = goalpageUrl.GetShortenedUrl();
                var goalNode = new WikiNode(goalPageShortUrl);
                wikiNodeGraph.AddNode(goalNode);
            }

            List<string> currentDepthNodes = new List<string> { startNode.GetUniqueIdentifier() };
            int depthCount = 0;
            while (currentDepthNodes.Count > 0 && depthCount < maximumDepth)
            {
                currentDepthNodes = await LoadNextDepthNodes(currentDepthNodes, wikiNodeGraph, goalPageShortUrl, maxLinksPerPage);
                depthCount++;
            }

            return wikiNodeGraph;
        }

        public async Task<WikiNodeGraph> BuildNodeGraph(string startPageUrl, string? goalpageUrl = null, int maximumDepth = int.MaxValue, int maxLinksPerPage = int.MaxValue)
        {
            var wikiNodeGraph = new WikiNodeGraph();
            return await AppendToNodeGraph(wikiNodeGraph, startPageUrl, goalpageUrl, maximumDepth, maxLinksPerPage);
        }

        private async Task<List<string>> LoadNextDepthNodes(List<string> currentDepthNodes, INodeGraphWithPathFinding wikiNodeGraph, string? goalPageShortUrl, int maxLinksPerPage)
        {
            List<string> nextDepthNodes = new();

            foreach (string nodeShortUrl in currentDepthNodes)
            {
                var nodeToLoad = (WikiNode)wikiNodeGraph.GetNode(nodeShortUrl);
                IEnumerable<string> newNodesUrls;

                if (nodeToLoad.IsNodeFullyLoaded)
                {
                    newNodesUrls = wikiNodeGraph.GetNeighbors(nodeToLoad).Select(n => n.GetUniqueIdentifier());
                    if (maxLinksPerPage < int.MaxValue && newNodesUrls.Count() > maxLinksPerPage)
                    {
                        newNodesUrls = newNodesUrls.Take(maxLinksPerPage);
                    }
                }
                else
                {
                    WikipediaPage page = await WikipediaPage.Build(nodeShortUrl, httpClient);

                    newNodesUrls = page.ValuableLinks.Select(v => v.Value.ShortUrl);
                    if (maxLinksPerPage < int.MaxValue && page.ValuableLinks.Count > maxLinksPerPage)
                    {
                        nodeToLoad.IsNodeFullyLoaded = false;
                        newNodesUrls = newNodesUrls.Take(maxLinksPerPage);
                    }
                    else
                    {
                        nodeToLoad.IsNodeFullyLoaded = true;
                    }
                }

                foreach (var newShortUrl in newNodesUrls)
                {
                    var newNode = new WikiNode(newShortUrl);
                    wikiNodeGraph.AddNode(newNode);
                    nextDepthNodes.Add(newShortUrl);

                    if (newShortUrl == nodeShortUrl) // prevents cycles
                        continue;

                    wikiNodeGraph.AddUnidirectionalEdge(wikiNodeGraph.GetNode(nodeShortUrl), newNode, 1);

                    if (newShortUrl == goalPageShortUrl)
                        return new();
                }

                if (this.cancellationToken.IsCancellationRequested)
                    return new();
            }

            return nextDepthNodes;
        }
    }
}
