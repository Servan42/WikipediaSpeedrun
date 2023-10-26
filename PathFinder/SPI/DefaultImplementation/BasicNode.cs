using PathFinder.SPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.SPI.DefaultImplementation
{
    public class BasicNode : INodeForPathFinding
    {
        private string identifier;

        public BasicNode(string identifier)
        {
            this.identifier = identifier;
        }

        public string GetNodeIdentifier()
        {
            return identifier;
        }
    }
}
