using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using HtmlAgilityPack;



namespace BlogGather
{
	public static class HtmlToText
	{
		#region Class Methods

        //public static string ExtractText(this HtmlDocument htmlDocument)
        //{
        //    using (StringWriter sw = new StringWriter(CultureInfo.InvariantCulture))
        //    {
        //        ConvertTo(htmlDocument.DocumentNode, sw);
        //        sw.Flush();
        //        return sw.ToString();
        //    }
        //}

		public static DocumentWithLinks GetLinks(this HtmlDocument htmlDocument)
		{
			return new DocumentWithLinks(htmlDocument);
		}
        public static DocumentWithLinks GetSrcLinks(this HtmlDocument htmlDocument)
        {
            return new DocumentWithLinks(htmlDocument, true);
        }
        //private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        //{
        //    foreach (HtmlNode subnode in node.ChildNodes)
        //    {
        //        ConvertTo(subnode, outText);
        //    }
        //}

        //private static void ConvertTo(HtmlNode node, TextWriter outText)
        //{
        //    string html;
        //    switch (node.NodeType)
        //    {
        //        case HtmlNodeType.Comment:
        //            // don't output comments
        //            break;

        //        case HtmlNodeType.Document:
        //            ConvertContentTo(node, outText);
        //            break;

        //        case HtmlNodeType.Text:
        //            // script and style must not be output
        //            string parentName = node.ParentNode.Name;
        //            if ((parentName == "script") || (parentName == "style"))
        //                break;

        //            // get text
        //            html = ((HtmlTextNode) node).Text;

        //            // is it in fact a special closing node output as text?
        //            if (HtmlNode.IsOverlappedClosingElement(html))
        //                break;

        //            // check the text is meaningful and not a bunch of whitespaces
        //            if (html.Trim().Length > 0)
        //            {
        //                outText.Write(HtmlEntity.DeEntitize(html));
        //                outText.Write(" ");
        //            }
        //            break;

        //        case HtmlNodeType.Element:
        //            switch (node.Name)
        //            {
        //                case "p":
        //                    // treat paragraphs as crlf
        //                    outText.Write("\r\n");
        //                    break;
        //            }

        //            if (node.HasChildNodes)
        //            {
        //                ConvertContentTo(node, outText);
        //            }
        //            break;
        //    }
        //}

		#endregion
	}

	public class DocumentWithLinks
	{
		#region Readonly & Static Fields

		private readonly HtmlDocument m_Doc;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of a DocumentWithLinkedFiles.
		/// </summary>
		/// <param name="doc">The input HTML document. May not be null.</param>
		public DocumentWithLinks(HtmlDocument doc)
		{


			m_Doc = doc;
			GetLinks();
			GetReferences();
            GetReferencesText();
		}
        public DocumentWithLinks(HtmlDocument doc, bool bSrc)
        {


            m_Doc = doc;
            GetSrcLinks();
            //GetReferences();
            //GetReferencesText();
        }
		#endregion

		#region Instance Properties

		/// <summary>
		/// Gets a list of links as they are declared in the HTML document.
		/// </summary>
		public IEnumerable<string> Links { get; private set; }

		/// <summary>
		/// Gets a list of reference links to other HTML documents, as they are declared in the HTML document.
		/// </summary>
		public IEnumerable<string> References { get; private set; }
        public Dictionary<string, string> m_dicLink2Text = new Dictionary<string, string>();
        //public List<string> HrefInnerTexts = new List<string>();
		#endregion

		#region Instance Methods

		private void GetLinks()
		{
			HtmlNodeCollection atts = m_Doc.DocumentNode.SelectNodes("//*[@background or @lowsrc or @src or @href or @action]");
            if (Equals(atts, null))
			{
				Links = new string[0];
				return;
			}

			Links = atts.
				SelectMany(n => new[]
					{
						ParseLink(n, "background"),
						ParseLink(n, "href"),
						ParseLink(n, "src"),
						ParseLink(n, "lowsrc"),
                        
						ParseLink(n, "action"),
					}).
				Distinct().
				ToArray();
		}
        private void GetSrcLinks()
        {
            HtmlNodeCollection atts = m_Doc.DocumentNode.SelectNodes("//*[@src]");
            if (Equals(atts, null))
            {
                atts = m_Doc.DocumentNode.SelectNodes("//*[@data-src]");
                if (Equals(atts, null))
                {
                    Links = new string[0];
                    return;
                }
            }

            Links = atts.
                SelectMany(n => new[]
					{
						ParseLink(n, "src"),
                        ParseLink(n, "orgSrc"),
                        ParseLink(n, "data-src"),
                        
				
					}).
                Distinct().
                ToArray();
        }
		private void GetReferences()
		{
			HtmlNodeCollection hrefs = m_Doc.DocumentNode.SelectNodes("//a[@href]");

            if (Equals(hrefs, null))
			{
				References = new string[0];
				return;
			}

			References = hrefs.
				Select(href => href.Attributes["href"].Value).
				Distinct().
				ToArray();
		}
        private void GetReferencesText()
        {
            try
            {
                m_dicLink2Text.Clear();
                HtmlNodeCollection hrefs = m_Doc.DocumentNode.SelectNodes("//a[@href]");

                if (Equals(hrefs, null))
                {
                    //References = new string[0];
                    return;
                }

                foreach (HtmlNode node in hrefs)
                {
                    string strHerf = HttpUtility.UrlDecode(node.Attributes["href"].Value.ToString().ToLower());
                    string strHtml = HttpUtility.HtmlDecode(node.InnerHtml);
                    if (!m_dicLink2Text.Keys.Contains(HttpUtility.UrlDecode(node.Attributes["href"].Value.ToString().ToLower())))
                        if(!HttpUtility.HtmlDecode(node.InnerHtml).Contains("img src")
                            && !HttpUtility.HtmlDecode(node.InnerHtml).Contains("img ")
                            && !HttpUtility.HtmlDecode(node.InnerHtml).Contains(" src"))
                            m_dicLink2Text.Add(HttpUtility.UrlDecode(node.Attributes["href"].Value.ToString().ToLower()), HttpUtility.HtmlDecode(node.InnerHtml));
                    // HrefInnerTexts.Add(node.InnerHtml);
                }
                int a = 0;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }


            //References = hrefs.
            //    Select(href => href.Attributes["href"].Value).
            //    Distinct().
            //    ToArray();
        }
		#endregion

		#region Class Methods

		private static string ParseLink(HtmlNode node, string name)
		{
			HtmlAttribute att = node.Attributes[name];
            if (Equals(att, null))
			{
				return null;
			}

			// if name = href, we are only interested by <link> tags
            //if ((name == "href") && (node.Name != "link" && node.Name != "base"))
            if ((name == "href") && (node.Name != "link" && node.Name != "a"))
			{
				return null;
			}

			return att.Value;
		}

		#endregion
	}
}