using PathFinder.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.API.Services
{
    public class Node : INode
    {
        private string identifier;

        public Node(string identifier)
        {
            this.identifier = identifier;
        }

        public string GetUniqueIdentifier()
        {
            return identifier;
        }
    }
}
