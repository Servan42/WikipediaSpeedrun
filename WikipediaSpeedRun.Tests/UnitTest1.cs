using Moq;
using System.Text;
using WikipediaSpeedRunLib.Model;
using WikipediaSpeedRunLib.SPI.Interfaces;

namespace WikipediaSpeedRun.Tests
{
    public class Tests
    {
        private const string validLinkElement1 = "<a href=\"/wiki/Potentiometer\" title=\"Potentiometer\">potentiometers</a>";
        private const string validLinkElement2 = "<a href=\"/wiki/Image_recognition\" class=\"mw-redirect\" title=\"Image recognition\"><b>image recognition</b></a>";
        Mock<IHttpClientAdapter> mockHttpClient;
        WikipediaPage sut_page;

        [SetUp]
        public void Setup()
        {
            mockHttpClient = new();
            sut_page = new WikipediaPage("url", mockHttpClient.Object);
        }

        [Test]
        public async Task Should_ignore_links_that_arent_in_article()
        {
            // GIVEN
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
            mockHttpClient.Setup(x => x.GetStringAsync("url")).ReturnsAsync(html.ToString());

            // WHEN
            await sut_page.LoadPageInfos();

            // THEN
            Assert.That(sut_page.ValuableLinks.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Should_ignore_invalid_links()
        {
            // GIVEN
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
            html.Append(invalidLink6);
            html.Append("</div></main></body></html>");
            mockHttpClient.Setup(x => x.GetStringAsync("url")).ReturnsAsync(html.ToString());

            // WHEN
            await sut_page.LoadPageInfos();

            // THEN
            Assert.That(sut_page.ValuableLinks.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Should_ignore_duplicate_links()
        {
            // GIVEN
            StringBuilder html = new();

            html.Append("<!DOCTYPE html><html><head></head><body><main><div id=\"mw-content-text\">");
            html.Append(validLinkElement1);
            html.Append(validLinkElement1);
            html.Append("</div></main></body></html>");
            mockHttpClient.Setup(x => x.GetStringAsync("url")).ReturnsAsync(html.ToString());

            // WHEN
            await sut_page.LoadPageInfos();

            // THEN
            Assert.That(sut_page.ValuableLinks.Count, Is.EqualTo(1));
        }

        private static readonly object[] linkElements =
        {
            new object[] {new List<string> { validLinkElement1 } },
            new object[] {new List<string> { validLinkElement2 } },
            new object[] {new List<string> {validLinkElement1, validLinkElement2 }}
        };

        [TestCaseSource(nameof(linkElements))]
        public async Task Should_find_link_html_elements_in_page(List<string> linkelements)
        {
            // GIVEN
            StringBuilder html = new();
            html.Append("<!DOCTYPE html><html><head></head><body><main><div id=\"mw-content-text\">");
            linkelements.ForEach(e => html.Append(e));
            html.Append("</div></main></body></html>");
            mockHttpClient.Setup(x => x.GetStringAsync("url")).ReturnsAsync(html.ToString());

            // WHEN
            await sut_page.LoadPageInfos();

            // THEN
            Assert.That(sut_page.ValuableLinks.Count, Is.EqualTo(linkelements.Count));
            linkelements.ForEach(e => Assert.That(sut_page.ValuableLinks.Values.Count(l => l.HtmlLinkElement == e) == 1));
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