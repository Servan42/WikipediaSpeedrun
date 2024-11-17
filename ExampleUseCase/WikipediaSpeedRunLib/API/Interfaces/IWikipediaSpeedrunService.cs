using WikipediaSpeedRunLib.Model;

namespace WikipediaSpeedRunLib.API.Interfaces
{
    public interface IWikipediaSpeedrunService
    {
        public Task<WikiNodeGraph> BuildNodeGraph(string startPageUrl, string? goalpageUrl = null, int maximumDepth = int.MaxValue, int maxLinksPerPage = int.MaxValue);
        public Task<WikiNodeGraph> AppendToNodeGraph(WikiNodeGraph wikiGraph, string startPageUrl, string? goalpageUrl = null, int maximumDepth = int.MaxValue, int maxLinksPerPage = int.MaxValue);
    }
}
