using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.API.Exceptions
{
    public class NodeGraphException : Exception
    {
        public NodeGraphException(string? message) : base(message)
        {
        }
    }
}
