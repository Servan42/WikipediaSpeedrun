using WikipediaSpeedRunLib.Extensions;

namespace WikipediaSpeedRunLib.Model
{
    public class LinkInfos
    {
        public static readonly string LINK_PREFIX = "https://en.wikipedia.org/wiki/";
        public string HtmlLinkElement { get; private set; }
        public string Url { get; set; }
        public string ShortUrl => Url.GetShortenedUrl();
        public string PageTitle { get; set; }
        public bool IsExternalLink { get; set; }

        public bool ShouldIgnore => string.IsNullOrEmpty(Url)
            || string.IsNullOrEmpty(PageTitle)
            || Url.Contains("File:")
            || Url.Contains("Special:")
            || PageTitle.Contains(":")
            || IsExternalLink;

        public LinkInfos(string url, string pageTitle)
        {
            Url = url;
            PageTitle = pageTitle;
        }

        public LinkInfos(string htmlLinkElement)
        {
            HtmlLinkElement = htmlLinkElement;
            ParseHtmlLinkElement();
        }

        private void ParseHtmlLinkElement()
        {
            Url = HtmlLinkElement.SubstringBetweenTwoTags("href=\"", "\"");
            if (Url.StartsWith("/wiki/"))
            {
                Url = "https://en.wikipedia.org" + Url;
            }
            else
            {
                IsExternalLink = true;
            }
            PageTitle = HtmlLinkElement.SubstringBetweenTwoTags("title=\"", "\"");
        }
    }
}