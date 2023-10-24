using PathFinder.API.Interfaces;
using PathFinder.API.Services;

namespace PathFinderTests
{
    public class PathFinderTests
    {
        IPathFinderService pathfinderService_sut;

        [SetUp]
        public void Setup()
        {
            this.pathfinderService_sut = new PathFinderService();
        }

        [Test]
        public void Test1()
        {
            this.pathfinderService_sut.BreadthFirstSearch()
        }
    }
}