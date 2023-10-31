using PathFinder.API.Interfaces;
using PathFinder.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikipediaSpeedRunLib.API.Interfaces;
using WikipediaSpeedRunLib.Model;

namespace WikipediaSpeedRunLib.API.Services
{
    public class WikiGraphPersistenceFileService : IWikiGraphPersistenceFileService
    {
        public WikiNodeGraph GetWikiGraphFromFile(string filename)
        {
            WikiNodeGraph graph = new WikiNodeGraph();

            using (StreamReader reader = new StreamReader(filename))
            {
                string? line = reader.ReadLine();
                while(line != null)
                {
                    string[] splittedLine = line.Split(';');

                    var mainNode = new WikiNode(splittedLine[0]);
                    mainNode.IsNodeFullyLoaded = bool.Parse(splittedLine[1]);
                    if(!graph.AddNode(mainNode))
                    {
                        var alreadyPresentNode = (WikiNode) graph.GetNode(mainNode.GetUniqueIdentifier());
                        alreadyPresentNode.IsNodeFullyLoaded = mainNode.IsNodeFullyLoaded;
                    }

                    for (int i = 2; i < splittedLine.Length; i++)
                    {
                        INode childNode = new WikiNode(splittedLine[i]);
                        graph.AddNode(childNode);
                        graph.AddUnidirectionalEdge(mainNode, childNode, 1);
                    }

                    line = reader.ReadLine();
                }
            }

            return graph;
        }

        public void SaveWikiGraphToFile(WikiNodeGraph wikiGraph, string filename)
        {
            var adjacencyList = wikiGraph.GetAdjacencyList();
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var nodeAdjacency in adjacencyList)
                {
                    if (nodeAdjacency.Key.Contains(';'))
                        continue;

                    sw.Write(nodeAdjacency.Key);
                    sw.Write(";");
                    sw.Write(((WikiNode)wikiGraph.GetNode(nodeAdjacency.Key)).IsNodeFullyLoaded);

                    if (nodeAdjacency.Value.Count == 0)
                    {
                        sw.WriteLine();
                        continue;
                    }

                    sw.Write(";");
                    sw.Write(string.Join(";", nodeAdjacency.Value.Keys.Where(n => !n.Contains(';'))));
                    sw.WriteLine();
                }
            }
        }
    }
}
