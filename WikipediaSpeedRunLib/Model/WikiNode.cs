using PathFinder.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaSpeedRunLib.Model
{
    public class WikiNode : INode
    {
        private string identifier;
        public bool IsNodeFullyLoaded { get; set; } = false;

        public WikiNode(string identifier)
        {
            this.identifier = identifier;
        }

        public string GetUniqueIdentifier()
        {
            return identifier;
        }
    }
}
