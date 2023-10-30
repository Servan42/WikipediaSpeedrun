using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public WikipediaSpeedrunService(IHttpClientAdapter httpClientAdapter)
        {
            this.httpClient = httpClientAdapter;
        }

        public async Task<INodeGraphWithPathFinding> BuildNodeGraph(string startPageUrl, string? goalpageUrl = null, int maximumDepth = int.MaxValue, int maxLinksPerPage = int.MaxValue)
        {

            INodeGraphWithPathFinding wikiNodeGraph = new BasicGraph();

            string startPageShortUrl = startPageUrl.GetShortenedUrl();
            var startNode = new Node(startPageShortUrl);
            wikiNodeGraph.AddNode(startNode);

            string? goalPageShortUrl = null;
            if (!string.IsNullOrEmpty(goalpageUrl))
            {
                goalPageShortUrl = goalpageUrl.GetShortenedUrl();
                var goalNode = new Node(goalPageShortUrl);
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

        private async Task<List<string>> LoadNextDepthNodes(List<string> currentDepthNodes, INodeGraphWithPathFinding wikiNodeGraph, string? goalPageShortUrl, int maxLinksPerPage)
        {
            List<string> nextDepthNodes = new();

            foreach (string nodeShortUrl in currentDepthNodes)
            {
                WikipediaPage page = await WikipediaPage.Build(nodeShortUrl, httpClient);
                
                var newNodesUrls = page.ValuableLinks.Select(v => v.Value.ShortUrl);
                if (maxLinksPerPage < int.MaxValue && page.ValuableLinks.Count > maxLinksPerPage)
                {
                    newNodesUrls = newNodesUrls.Take(maxLinksPerPage);
                }

                foreach (var newShortUrl in newNodesUrls)
                {
                    var newNode = new Node(newShortUrl);
                    if (wikiNodeGraph.AddNode(newNode))
                    {
                        nextDepthNodes.Add(newShortUrl);
                    }

                    if (newShortUrl == nodeShortUrl) // prevents cycles
                        continue;

                    wikiNodeGraph.AddUnidirectionalEdge(wikiNodeGraph.GetNode(nodeShortUrl), newNode, 1);

                    if (newShortUrl == goalPageShortUrl)
                        return new();
                }
            }

            return nextDepthNodes;
        }
    }

    internal class BasicGraph : NodeGraphWithPathFinding
    {
        public override int GetHeuristicDistanceToGoal(INode startNode, INode destinationNode)
        {
            return 0;
        }
    }

}
