using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using System.Text.RegularExpressions;
using wojilu.Data;
using wojilu.Net;
using System.Threading;
using wojilu.Common.Spider.Interface;
using HtmlAgilityPack;
//using Fizzler;
//using Fizzler.Systems.HtmlAgilityPack;
//using System.Linq;
using wojilu.Members.Users.Domain;
using System.Collections;
using BlogGather;
using Maticsoft.Model;
using System.Data;
namespace wojilu.Common.Spider.Service {

    public class SpiderTool : ISpiderTool {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SpiderTool ) );
        private static Random rd = new Random();
     
        public void DownloadPage( SpiderTemplate s, StringBuilder log, int[] arrSleep ) {

            logger.Info( "抓取列表页..." + s.SiteName + "_" + s.ListUrl );
            log.AppendLine( "抓取列表页..." + s.SiteName + "_" + s.ListUrl );

            List<DetailLink> list = GetDataList( s, log );

            foreach (DetailLink link in list) {
                savePageDetail( link, log );

                // 暂停几秒，TODO 可配置
                int sleepms = rd.Next( arrSleep[0], arrSleep[1] );
                Thread.Sleep( sleepms );
            }

            log.AppendLine( "抓取完毕。" );

        }

        public static List<DetailLink> GetDataList( SpiderTemplate s, StringBuilder sb ) {

            if (strUtil.HasText( s.ListUrl )) {
                s.SiteUrl = new UrlInfo( s.ListUrl ).SiteUrl;
            }

            // 一、先抓取列表页面内容
            string page = downloadListPage( s, sb );
            if (strUtil.IsNullOrEmpty( page )) {
                logger.Error( "list page is empty, url=" + s.SiteUrl );
            }

            // 二、得到所有文章的title和url
            List<DetailLink> list = getListItem( s, page, sb );
            return list;
        }
        protected static string GetUrlLeftPart(string strPath)
        {
            int n = strPath.LastIndexOf("/");
            if (n > -1)
            {
                return strPath.Substring(0, n + 1);
            }
            else
                return new Uri(strPath).GetLeftPart(UriPartial.Authority);
        }
        //protected static string NormalizeLink(string baseUrl, string link)
        //{
        //    return link.NormalizeUrl(baseUrl);
        //}
        //protected static string GetNormalizedLink(string baseUrl, string decodedLink)
        //{
        //    string normalizedLink = NormalizeLink(baseUrl, decodedLink);

        //    return normalizedLink;
        //}
        protected static HtmlAgilityPack.HtmlDocument GetHtmlDocument(string strPage)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };
            htmlDoc.LoadHtml(strPage);


            return htmlDoc;
        }
        //protected static void SaveUrlToDB(string strReturnPage, SpiderTemplate s, List<DetailLink> list)
        //{
                   
        //    Dictionary<string, string> m_dicLink2Text = new Dictionary<string, string>();
        //    string strUrlFilterRule = s.ListPattern;
        //    //strUrlFilterRule = ParseUrl(strUrlFilterRule);
        //    HtmlAgilityPack.HtmlDocument htmlDoc = GetHtmlDocument(strReturnPage);

        //   // string baseUrl = new Uri(strVisitUrl).GetLeftPart(UriPartial.Authority);
        //    string baseUrl = GetUrlLeftPart(s.ListUrl);
        //    DocumentWithLinks links = htmlDoc.GetLinks();
        //    bool bNoArticle = true;
        //    List<string> lstRevomeSame = new List<string>();

        //  //  int nCountPerPage = 0;
        //  //  bool bExistFind = false;
        //  //  List<string> lstNeedDownLoad = new List<string>();
        //    foreach (string link in links.Links.Union(links.References))
        //    {
                
        //        if (string.IsNullOrEmpty(link))
        //        {
        //            continue;
        //        }

        //        //string decodedLink = ExtendedHtmlUtility.HtmlEntityDecode(link);
        //        string decodedLink = link;
        //        //if (decodedLink != link)
        //        //{
        //        //    int a = 1;
        //        //}
        //        //Console.WriteLine(decodedLink);
        //        string normalizedLink = GetNormalizedLink(baseUrl, decodedLink);
        //        //Console.WriteLine(normalizedLink);

        //        if (string.IsNullOrEmpty(normalizedLink))
        //        {
        //            continue;
        //        }

        //        MatchCollection matchs = Regex.Matches(normalizedLink, strUrlFilterRule, RegexOptions.Singleline);
        //        if (matchs.Count > 0)
        //        {
        //            string strLinkText = "";

        //            foreach (string strTemp in links.m_dicLink2Text.Keys)
        //            {
        //                if (strTemp.Contains(normalizedLink))
        //                {
        //                    strLinkText = links.m_dicLink2Text[strTemp];
        //                    break;
        //                }
        //            }
        //            //if (links.m_dicLink2Text.Keys.Contains(normalizedLink))
        //            //    strLinkText = links.m_dicLink2Text[normalizedLink];

        //            if (strLinkText == "")
        //            {
        //                if (links.m_dicLink2Text.Keys.Contains(link))
        //                    strLinkText = links.m_dicLink2Text[link].TrimEnd().TrimStart();
        //                if (links.m_dicLink2Text.Keys.Contains(link.ToLower()))
        //                    strLinkText = links.m_dicLink2Text[link.ToLower()].TrimEnd().TrimStart();
        //            }
              
        //            if (lstRevomeSame.Contains(normalizedLink))
        //                continue;
        //            else
        //                lstRevomeSame.Add(normalizedLink);
                    
                    
        //            //bool bRet = AddLayerNodeToSaveUrlToDB(m_strWholeDbName, normalizedLink, ref strLinkText);
        //            DetailLink lnk = new DetailLink();
        //            lnk.Template = s;
        //            lnk.Url = normalizedLink;
        //            lnk.Title = strLinkText;
        //            list.Add(lnk);
        //        }
        //        //Console.WriteLine(" uri is " + normalizedLink.ToString());
        //    }

           
           

        //    return;

        //}
        public static List<DetailLink> getListItem(SpiderTemplate s, string page, StringBuilder sb)
        {

            List<DetailLink> list = new List<DetailLink>();
            if (strUtil.IsNullOrEmpty( page )) return list;

            //获取全部url
            //MatchCollection matchs = Regex.Matches( page, SpiderConfig.ListLinkPattern, RegexOptions.Singleline );
            //if (matchs.Count == 0) {
            //    logger.Error( "list link match count=0" );
            //    logInfo( "list link match count=0", s, sb );
            //}
           // SaveUrlToDB(page, s, list);
            //for (int i = matchs.Count - 1; i >= 0; i--) {
            //    DetailLink dlink = 

            //    if (dlink == null) continue;

            //    if (dlink.Url.Length > 100) continue;
            //    list.Add( dlink );
            //}
            logInfo( "共抓取到链接：" + list.Count, s, sb );

            return list;
        }
        
        
        private static void savePageDetail( DetailLink lnk, StringBuilder sb ) {

          

            SpiderTemplate template = lnk.Template;
            string url = lnk.Url;
            string title = lnk.Title;
            string summary = lnk.Abstract;

            if (isPageExist( url, sb )) return;

            String pageBody = new PagedDetailSpider().GetContent( url, template, sb );


            if (pageBody == null) return;

            SpiderArticle pd = new SpiderArticle();
            pd.Title = title;
            pd.Url = strUtil.SubString( url, 250 );
            pd.Abstract = summary;
            pd.Body = pageBody;
            pd.SpiderTemplate = template;

            MatchCollection matchs = Regex.Matches( pageBody, RegPattern.Img, RegexOptions.Singleline );
            if (matchs.Count > 0) {
                pd.IsPic = 1;
                pd.PicUrl = matchs[0].Groups[1].Value;
            }

            pd.insert();

            sb.AppendLine( "保存成功..." + lnk.Title + "_" + lnk.Url );


            pageBody = Regex.Replace(pageBody, "font-size", "", RegexOptions.IgnoreCase);
            string strArcitleLink = "<div class=\"ArcitleLink\"><a href=" + pd.Url + ">原文链接</a></div>";
            pageBody = pageBody + strArcitleLink;

            Maticsoft.BLL.BlogCategory bllBlogCategory = new Maticsoft.BLL.BlogCategory();
            DataSet ds = bllBlogCategory.GetList("AppId = '" + template.IsDelete.ToString() + "'");
            int nCateID = 1;
            if (ds.Tables[0].Rows.Count > 0)
            {
                nCateID = (int)ds.Tables[0].Rows[0]["Id"];
            }





            BlogPost data = new BlogPost();


            data.CategoryId = nCateID;
            data.Title = title;
            data.Abstract = summary;
            data.Content = pageBody;
            data.AccessStatus = 0;
            data.CommentCondition = 0;
            data.SaveStatus = 1;//草稿
            data.Created = System.DateTime.Now.Date;
            data.IsTop = 0;
            data.IsPick = 0;
            data.IsPic = 0;
            data.Ip = "";
            data.OwnerId = template.IsDelete;
            data.OwnerUrl = template.SiteName;
            data.OwnerType = "wojilu.Members.Users.Domain.User";
            data.CreatorUrl = template.SiteName;
            data.AppId = template.IsDelete; ;
            data.CreatorId = template.IsDelete;
            Maticsoft.BLL.BlogPost bll = new Maticsoft.BLL.BlogPost();
            bll.Add(data);
            
        }

        //检查数据库中是否已经存在此数据？
        private static bool isPageExist( string pageUrl, StringBuilder sb ) {
            bool isExist = false;
            List<SpiderArticle> list = SpiderArticle.find( "Url=:url and IsDelete=0" ).set( "url", pageUrl ).list();
            if (list.Count > 0) {
                logger.Info( "pass..." + pageUrl );
                sb.AppendLine( "pass..." + pageUrl );
                isExist = true;
            }
            return isExist;
        }
        //解析用户输入的通配符方式的目标网页模式
        //将/www.cnblogs.com/*/archive/*/*/*/*.html
        //转换为www\.cnblogs\.com/(.*?)/archive/(.*?)/(.*?)/(.*?)/(.*?).html
        private static string ParseUrl( string strUrlSrc ) {
            //string strRet = strUrlSrc.Replace( ".", @"\." );
            //strRet = strRet.Replace( "?", @"\?" );
            //strRet = strRet.Replace( "*", "(.*?)" );
            return strUrlSrc;
        }

        private static DetailLink getDetailLink( Match match, SpiderTemplate s ) {

            string url = match.Groups[1].Value;
            string title = match.Groups[2].Value;
            //判断输入的url是否满足用户定义的通配符方式的模式
            MatchCollection matchs = Regex.Matches( url, ParseUrl( s.ListPattern ), RegexOptions.Singleline );
            if (matchs.Count == 0) {
                return null;
            }
            if (url.IndexOf( "javascript:" ) >= 0) return null;
            if (url.StartsWith( "#" )) return null;

            title = Regex.Replace( title, "<.+?>", "" );
            if (strUtil.IsNullOrEmpty( title )) return null;
            if (title == "更多") return null;
            if (title == "more") return null;
            if (title == "更多&gt;&gt;") return null;

            string summary = "";
            if (match.Groups.Count > 2) summary = match.Groups[3].Value;

            if (url.StartsWith( "http" ) == false) url = strUtil.Join( s.SiteUrl, url );


            DetailLink lnk = new DetailLink();
            lnk.Template = s;
            lnk.Url = url;
            lnk.Title = title;
            lnk.Abstract = summary;

            return lnk;
        }

        private static string downloadListPage( SpiderTemplate s, StringBuilder sb ) {
            string page = null;

            try {
                page = downloadListPageBody( s, sb );
            }
            catch (Exception ex) {
                logInfo( "error=抓取" + s.ListUrl + "发生错误：" + ex.Message, s, sb );
                return page;
            }

            return page;
        }


        private static string downloadListPageBody( SpiderTemplate s, StringBuilder sb ) {

            String target;

            if (strUtil.HasText( s.ListEncoding )) {
                target = PageLoader.Download( s.ListUrl, SpiderConfig.UserAgent, s.ListEncoding );
            }
            else {
                target = PageLoader.Download( s.ListUrl, SpiderConfig.UserAgent, "" );
            }

            if (strUtil.IsNullOrEmpty( target )) {
                logInfo( "error=原始页面没有内容: " + s.ListUrl, s, sb );
                return target;
            }
            else {
                logInfo( "抓取列表内容成功", s, sb );
            }

            if (strUtil.HasText( s.GetListBodyPattern() )) {
                HtmlDocument htmlDoc = new HtmlDocument {
                    OptionAddDebuggingAttributes = false,
                    OptionAutoCloseOnEnd = true,
                    OptionFixNestedTags = true,
                    OptionReadEncoding = true
                };

                htmlDoc.LoadHtml( target );
                try {
                    //IEnumerable<HtmlNode> Nodes = htmlDoc.DocumentNode.QuerySelectorAll( s.GetListBodyPattern() );

                    //if (Nodes.Count() > 0) {
                    //    logInfo( "匹配列表内容成功", s, sb );
                    //    target = Nodes.ToArray()[0].OuterHtml;
                    //    target = target.Trim();
                    //    return target;
                    //}
                    //else {
                    //    logInfo( "error=没有匹配的页面内容:" + s.ListUrl, s, sb );
                    //    return null;
                    //}
                }
                catch (Exception ex) {
                    logInfo( "htmlDoc QuerySelectorAll解析出错=" + ex.Message, s, sb );
                    return null;
                }
            }

            //这里未来也可以改成css选择器的方式，来细化目标url集合的范围
            //Match match = Regex.Match(target, s.GetListBodyPattern(), RegexOptions.Singleline);
            //if (match.Success)
            //{
            //    target = match.Value;
            //}
            //else
            //{
            //    target = "";
            //    logInfo("error=没有匹配的页面内容:" + s.ListUrl, s, sb);
            //}

            return target.Trim();
        }

        private static void logInfo( String msg, SpiderTemplate s, StringBuilder sb ) {
            logger.Info( msg );
            sb.AppendLine( msg );
        }

    }


}
