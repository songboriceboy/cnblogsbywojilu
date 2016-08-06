using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace NReadability
{
    public class Readability
    {
        public static Readability Create(string documentHtml)
        {
            return new Readability(documentHtml);
        }

        private Readability(string documentHtml)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(documentHtml);

            var htmlNode = doc.DocumentNode.Element("html");

            TagNameToLowerCase(htmlNode);

            RemoveScripts(htmlNode);

            this.Title = GetArticleTitle(htmlNode);
            this.Content = GetArticleContent(doc);
        }

        public string Title { get; private set; }

        public string Content { get; private set; }

        private static void TagNameToLowerCase(HtmlNode node)
        {
            node.Name = node.Name.ToLower();

            foreach (var child in node.ChildNodes)
            {
                TagNameToLowerCase(child);
            }
        }

        private static void RemoveScripts(HtmlNode node)
        {
            foreach (var script in node.GetElementsByTagName("script"))
            {
                script.Remove();
            }
        }

        private static string GetInnerText(HtmlNode node)
        {
            return node.InnerText;
        }

        private static Regex s_unlikelyCandidates = new Regex("combx|comment|community|disqus|extra|foot|header|menu|remark|rss|shoutbox|sidebar|sponsor|ad-break|agegate|pagination|pager|popup|tweet|twitter", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex s_okMaybeItsACandidate = new Regex("and|article|body|column|main|shadow", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex s_divToPElements = new Regex("<(a|blockquote|dl|div|img|ol|p|pre|table|ul)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        private static double GetLinkDensity(HtmlNode node)
        {
            var links = node.GetElementsByTagName("a");
            
            var textLength = GetInnerText(node).Length;
            var linkLength = links.Sum(l => GetInnerText(l).Length);

            return linkLength * 1.0 / textLength;
        }

        private static int CalculateNodeScore(HtmlNode node)
        {
            var score = 0;
            switch (node.Name)
            {
                case "div":
                    score += 5;
                    break;

                case "pre":
                case "td":
                case "blockquote":
                    score += 3;
                    break;

                case "address":
                case "ol":
                case "ul":
                case "dl":
                case "dd":
                case "dt":
                case "li":
                case "form":
                    score -= 3;
                    break;

                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                case "th":
                    score -= 5;
                    break;
            }

            return score + GetClassWeight(node);
        }

        private static int GetClassWeight(HtmlNode node)
        {
            return 0;
        }

        private static string GetArticleTitle(HtmlNode htmlNode)
        {
            var titleNode = htmlNode.GetElementsByTagName("title").FirstOrDefault();
            if (titleNode == null) return null;

            string currTitle, origTitle;
            currTitle = origTitle = GetInnerText(titleNode);

            if (Regex.IsMatch(currTitle, @" [\|\-] "))
            {
                currTitle = Regex.Replace(origTitle,  @"(.*)[\|\-] .*", "$1");

                if (currTitle.Split(' ').Length < 3)
                {
                    currTitle = origTitle.Replace(@"[^\|\-]*[\|\-](.*)", "$1");
                }
            }
            else if (currTitle.IndexOf(": ") != -1)
            {
                currTitle = Regex.Replace(origTitle, @".*:(.*)", "$1");

                if(currTitle.Split(' ').Length < 3)
                {
                    currTitle = Regex.Replace(origTitle, @"[^:]*[:](.*)", "$1");
                }
            }
            else if (currTitle.Length > 150 || currTitle.Length < 15)
            {
                var hOnes = htmlNode.GetElementsByTagName("h1");
                if (hOnes.Count == 1)
                {
                    currTitle = GetInnerText(hOnes[0]);
                }
            }

            if (currTitle.Split(' ').Length <= 4)
            {
                currTitle = origTitle;
            }
        
            return currTitle.Trim();
        }

        private static string GetArticleContent(HtmlDocument doc)
        {
            var body = doc.DocumentNode.Element("html").Element("body");

            var allElements = body.GetElementsByTagName("*");

            var nodesToScore = new List<HtmlNode>();

            for (var nodeIndex = 0; nodeIndex < allElements.Count; nodeIndex++)
            {
                var node = allElements[nodeIndex];
                var unlikelyMatchString = node.GetAttributeValue("class", "") + node.Id;
                if (s_unlikelyCandidates.IsMatch(unlikelyMatchString) &&
                    !s_okMaybeItsACandidate.IsMatch(unlikelyMatchString) &&
                    node.Name != "body")
                {
                    Console.WriteLine("Removing unlikely candidate - " + unlikelyMatchString);
                    node.Remove();
                    continue;
                }

                if (node.Name == "p" || node.Name == "td" || node.Name == "pre")
                {
                    nodesToScore.Add(node);
                }

                if (node.Name == "div")
                {
                    if (!s_divToPElements.IsMatch(node.InnerHtml))
                    {
                        var newNode = node.OwnerDocument.CreateElement("p");
                        newNode.InnerHtml = node.InnerHtml;
                        node.ParentNode.ReplaceChild(newNode, node);

                        nodesToScore.Add(node);
                    }
                    else
                    {
                        foreach (var childNode in node.ChildNodes.ToList())
                        {
                            if (childNode.NodeType == HtmlNodeType.Text)
                            {
                                var p = node.OwnerDocument.CreateElement("p");
                                p.InnerHtml = childNode.InnerText;
                                childNode.ParentNode.ReplaceChild(p, childNode);
                            }
                        }
                    }
                }
            }

            var scores = new Dictionary<HtmlNode, int>();

            var candidates = new List<HtmlNode>();
            for (var pt = 0; pt < nodesToScore.Count; pt++)
            {
                var parentNode = nodesToScore[pt].ParentNode;
                var grandParentNode = parentNode != null ? parentNode.ParentNode : null;
                var innerText = GetInnerText(nodesToScore[pt]);

                if (parentNode == null) continue;

                if (innerText.Length < 25) continue;

                if (!scores.ContainsKey(parentNode))
                {
                    scores.Add(parentNode, CalculateNodeScore(parentNode));
                    candidates.Add(parentNode);
                }

                if (grandParentNode != null && !scores.ContainsKey(grandParentNode))
                {
                    scores.Add(grandParentNode, CalculateNodeScore(grandParentNode));
                    candidates.Add(grandParentNode);
                }

                var contentScore = 0;

                contentScore++;

                contentScore += innerText.Split(',', '\uFF0C').Length;

                contentScore += Math.Min(innerText.Length / 100, 3);

                scores[parentNode] += contentScore;

                if (grandParentNode != null)
                {
                    scores[grandParentNode] += contentScore / 2;
                }
            }

            HtmlNode topCandidate = null;
            foreach (var cand in candidates)
            {
                scores[cand] = (int)(scores[cand] * (1 - GetLinkDensity(cand)));

                if (topCandidate == null || scores[cand] > scores[topCandidate])
                {
                    topCandidate = cand;
                }

                if (topCandidate == null || topCandidate.Name == "body")
                {
                    topCandidate = doc.CreateElement("div");
                    topCandidate.InnerHtml = body.InnerHtml;
                    body.InnerHtml = "";
                    body.AppendChild(topCandidate);
                    scores[topCandidate] = CalculateNodeScore(topCandidate);
                }
            }

            return topCandidate == null ? null : topCandidate.InnerHtml;
        }
    }
}
