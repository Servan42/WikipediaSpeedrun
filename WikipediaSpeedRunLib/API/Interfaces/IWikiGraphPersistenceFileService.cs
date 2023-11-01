using PathFinder.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikipediaSpeedRunLib.Model;

namespace WikipediaSpeedRunLib.API.Interfaces
{
    public interface IWikiGraphPersistenceFileService
    {
        public void SaveWikiGraphToFile(WikiNodeGraph wikiGraph, string filename);
        public void SaveLoadedNodeListAlphabetical(WikiNodeGraph nodeGraph, string filename);
        public void SaveLoadedNodeListByMaxNodes(WikiNodeGraph wikiGraph, string filename);
        public WikiNodeGraph GetWikiGraphFromFile(string filename);
    }
}
