using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace WikipediaSpeedRunLib
{
    public class WikipediaPage
    {
        public WikipediaPage()
        {
        }

        public Dictionary<string, LinkInfos> Links { get; set; }

        public void LoadLinksInfosFromHtml(string html)
        {
            Links = new Dictionary<string, LinkInfos>();
            html = html.SubstringBetweenTwoTags("<div id=\"mw-content-text\"", "</main>");
            if (html == "") throw new ArgumentException("Could not find the main div of article", "html");
            List<string> htmlLinks = LoadHtmlLinkElementsFromHtml(html);
            htmlLinks.ForEach(a =>
            {
                var linkInfo = new LinkInfos(a);
                if (!linkInfo.ShouldIgnore && !Links.ContainsKey(linkInfo.Url)) Links.Add(linkInfo.Url, linkInfo);
            });
        }

        public List<string> LoadHtmlLinkElementsFromHtml(string html)
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