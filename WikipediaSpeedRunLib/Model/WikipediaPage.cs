﻿using PathFinder.API.Interfaces;
using WikipediaSpeedRunLib.Extensions;
using WikipediaSpeedRunLib.SPI.Interfaces;

namespace WikipediaSpeedRunLib.Model
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
            isPageLoaded = false;
        }

        public static async Task<WikipediaPage> Build(string url, IHttpClientAdapter httpClient)
        {
            WikipediaPage page = new WikipediaPage(url.GetLongUrl(), httpClient);
            await page.LoadPageInfos();
            return page;
        }

        public Dictionary<string, LinkInfos> ValuableLinks { get; set; }

        public string GetUniqueIdentifier()
        {
            return url;
        }

        private List<WikipediaPage> CreateNeighbors()
        {
            List<WikipediaPage> neighbors = new();
            foreach (var link in ValuableLinks)
            {
                WikipediaPage neighboor = new WikipediaPage(link.Value.Url, httpClient);
                neighboor.LoadPageInfos().Wait();
                neighbors.Add(neighboor);
            }
            return neighbors;
        }

        private List<WikipediaPage> CreateNeighborsThreaded(int nbThreads)
        {
            List<WikipediaPage> neighbors = new();

            foreach (var link in ValuableLinks)
            {
                WikipediaPage neighboor = new WikipediaPage(link.Value.Url, httpClient);
                neighbors.Add(neighboor);
            }

            List<Thread> threads = new();
            Dictionary<int, List<WikipediaPage>> splittedPageList = new();
            for (int i = 0; i < nbThreads; i++) splittedPageList.Add(i, new List<WikipediaPage>());

            int j = 0;
            foreach (var page in neighbors)
            {
                splittedPageList[j].Add(page);
                j++;
                if (j >= nbThreads) j = 0;
            }

            foreach (var onePageGroup in splittedPageList)
            {
                threads.Add(new Thread(() =>
                {
                    foreach (var page in onePageGroup.Value)
                    {
                        page.LoadPageInfos().Wait();
                    }
                }));
            }

            foreach (Thread thread in threads) thread.Start();
            foreach (Thread thread in threads) thread.Join();

            return neighbors;
        }

        public async Task LoadPageInfos()
        {
            string html = await httpClient.GetStringAsync(url);
            ValuableLinks = new Dictionary<string, LinkInfos>();
            html = html.SubstringBetweenTwoTags("<div id=\"mw-content-text\"", "</main>");
            if (html == "") throw new ArgumentException("Could not find the main div of article", "html");
            List<string> htmlLinks = LoadHtmlLinkElementsFromHtml(html);
            htmlLinks.ForEach(a =>
            {
                var linkInfo = new LinkInfos(a);
                if (!linkInfo.ShouldIgnore && !ValuableLinks.ContainsKey(linkInfo.Url)) ValuableLinks.Add(linkInfo.Url, linkInfo);
            });

            isPageLoaded = true;
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