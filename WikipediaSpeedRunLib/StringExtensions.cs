using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaSpeedRunLib
{
    public static class StringExtensions
    {
        public static string SubstringBetweenTwoTags(this string source, string tag1, string tag2)
        {
            if (source.IndexOf(tag1) == -1) return "";
            string result = source.Substring(source.IndexOf(tag1) + tag1.Length);
            if(result.IndexOf(tag2) == -1) return "";
            return result.Substring(0, result.IndexOf(tag2));
        }
    }
}
