using PathFinder.API.Interfaces;

namespace WikipediaSpeedRunLib.API.Interfaces
{
    public interface IWikipediaSpeedrunService
    {
        public Task<INodeGraphWithPathFinding> BuildNodeGraph(string startPageUrl, string? goalpageUrl = null, int maximumDepth = int.MaxValue, int maxLinksPerPage = int.MaxValue);
    }
}
