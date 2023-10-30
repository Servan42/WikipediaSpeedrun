using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikipediaSpeedRunLib.Model;

namespace WikipediaSpeedRunLib.Extensions
{
    public static class StringExtensions
    {
        public static string SubstringBetweenTwoTags(this string source, string tag1, string tag2)
        {
            if (source.IndexOf(tag1) == -1) return "";
            string result = source.Substring(source.IndexOf(tag1) + tag1.Length);
            if (result.IndexOf(tag2) == -1) return "";
            return result.Substring(0, result.IndexOf(tag2));
        }

        public static string GetShortenedUrl(this string longUrl)
        {
            return longUrl.StartsWith(LinkInfos.LINK_PREFIX) ? longUrl.Remove(0, LinkInfos.LINK_PREFIX.Length) : longUrl;
        }

        public static string GetLongUrl(this string shortUrl)
        {
            return shortUrl.Contains(LinkInfos.LINK_PREFIX) ? shortUrl : LinkInfos.LINK_PREFIX + shortUrl;
        }
    }
}
