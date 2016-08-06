using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace NReadability
{
    public static class HtmlNodeExtensions
    {
        public static List<HtmlNode> GetElementsByTagName(this HtmlNode node, string tagName)
        {
            var container = new List<HtmlNode>();
            GetElementsByTagName(node, tagName, container);
            return container;
        }

        private static void GetElementsByTagName(HtmlNode node, string tagName, List<HtmlNode> container)
        {
            foreach (var child in node.ChildNodes)
            {
                if (tagName == "*" || String.Equals(child.Name, tagName, StringComparison.OrdinalIgnoreCase))
                {
                    container.Add(child);
                }

                GetElementsByTagName(child, tagName, container);
            }
        }
    }
}
