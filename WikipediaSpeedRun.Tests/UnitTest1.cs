using System.Text;
using WikipediaSpeedRunLib;

namespace WikipediaSpeedRun.Tests
{
    public class Tests
    {
        private const string validLinkElement1 = "<a href=\"/wiki/Potentiometer\" title=\"Potentiometer\">potentiometers</a>";
        private const string validLinkElement2 = "<a href=\"/wiki/Image_recognition\" class=\"mw-redirect\" title=\"Image recognition\"><b>image recognition</b></a>";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Should_ignore_links_that_arent_in_article()
        {
            // GIVEN
            WikipediaPage sut_page = new WikipediaPage();
            StringBuilder html = new();
            html.Append("<!DOCTYPE html><html><head></head><body>");
            html.Append("<main>");
            html.Append(validLinkElement1);
            html.Append("<div id=\"mw-content-text\">");
            html.Append(validLinkElement1);
            html.Append("</div>");
            html.Append("</main>");
            html.Append(validLinkElement1);
            html.Append("</body></html>");

            // WHEN
            sut_page.LoadLinksInfosFromHtml(html.ToString());

            // THEN
            Assert.That(sut_page.Links.Count, Is.EqualTo(1));
        }

        [Test]
        public void Should_ignore_invalid_links()
        {
            // GIVEN
            WikipediaPage sut_page = new WikipediaPage();
            StringBuilder html = new();

            string invalidLink1 = "<a title=\"Potentiometer\">potentiometers</a>";
            string invalidLink2 = "<a href=\"/wiki/Potentiometer\">potentiometers</a>";
            string invalidLink3 = "<a href=\"/wiki/Potentiometer\" title=\"Something:Potentiometer\">potentiometers</a>";
            string invalidLink4 = "<a href=\"/wiki/File:Potentiometer\" title=\"Potentiometer\">potentiometers</a>";
            string invalidLink5 = "<a href=\"/wiki/Special:Potentiometer\" title=\"Potentiometer\">potentiometers</a>";
            string invalidLink6 = "<a href=\"https:/externallink/wiki/Potentiometer\" title=\"Potentiometer\">potentiometers</a>";

            html.Append("<!DOCTYPE html><html><head></head><body><main><div id=\"mw-content-text\">");
            html.Append(validLinkElement1);
            html.Append(invalidLink1);
            html.Append(invalidLink2);
            html.Append(invalidLink3);
            html.Append(invalidLink4);
            html.Append(invalidLink5);
            html.Append("</div></main></body></html>");

            // WHEN
            sut_page.LoadLinksInfosFromHtml(html.ToString());

            // THEN
            Assert.That(sut_page.Links.Count, Is.EqualTo(1));
        }

        [Test]
        public void Should_ignore_duplicate_links()
        {
            // GIVEN
            WikipediaPage sut_page = new WikipediaPage();
            StringBuilder html = new();

            html.Append("<!DOCTYPE html><html><head></head><body><main><div id=\"mw-content-text\">");
            html.Append(validLinkElement1);
            html.Append(validLinkElement1);
            html.Append("</div></main></body></html>");

            // WHEN
            sut_page.LoadLinksInfosFromHtml(html.ToString());

            // THEN
            Assert.That(sut_page.Links.Count, Is.EqualTo(1));
        }

        private static readonly object[] linkElements =
        {
            new object[] {new List<string> { validLinkElement1 } },
            new object[] {new List<string> { validLinkElement2 } },
            new object[] {new List<string> {validLinkElement1, validLinkElement2 }}
        };

        [TestCaseSource(nameof(linkElements))]
        public void Should_find_link_html_elements_in_page(List<string> linkelements)
        {
            // GIVEN
            WikipediaPage sut_page = new WikipediaPage();
            StringBuilder html = new();


            html.Append("<!DOCTYPE html><html><head></head><body>");
            linkelements.ForEach(e => html.Append(e));
            html.Append("</body></html>");

            // WHEN
            var result = sut_page.LoadHtmlLinkElementsFromHtml(html.ToString());

            // THEN
            Assert.That(result.Count, Is.EqualTo(linkelements.Count));
            linkelements.ForEach(e => Assert.That(result.Contains(e)));
        }

        [TestCase(validLinkElement1, "https://en.wikipedia.org/wiki/Potentiometer", "Potentiometer")]
        [TestCase(validLinkElement2, "https://en.wikipedia.org/wiki/Image_recognition", "Image recognition")]
        [TestCase("<a class=\"aaa\">yes</a>", "", "")]
        public void Should_extract_link_infos_from_html_element(string linkElement, string expectedUrl, string expectedTitle)
        {
            // GIVEN
            LinkInfos linkInfos;

            // WHEN
            linkInfos = new(linkElement);

            // THEN
            Assert.That(linkInfos.Url, Is.EqualTo(expectedUrl));
            Assert.That(linkInfos.PageTitle, Is.EqualTo(expectedTitle));
        }
    }
}