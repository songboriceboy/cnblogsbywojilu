using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.cms.xw.Controller.Admin;
using wojilu.cms.xw.Domain;
using wojilu.Web;
using wojilu.cms.xw.Domain.backup;
using System.Data;

namespace wojilu.cms.xw.Controller
{

    public class MainController : ControllerBase
    {

        private int count = 0;
        private int Tagcount = 0;

        //public void Index()
        //{
        //    // 获取[文章]循环块,根据文章找标签
        //    DataPage<Article> articles = db.findPage<Article>("");
        //    IBlock block = getBlock("list");
        //    foreach (Article a in articles.Results)
        //    {
        //        block.Set("article.Title", a.Title);
        //        block.Set("article.ReadLink", to(new ArticleController().Read, a.Id));
        //        block.Set("article.Content", a.Content);
        //        block.Set("article.Created", a.Created);
        //        block.Set("article.ReadCount", a.ReadCount);


        //        block.Set("article.Tag.Name", a.Tag.Name);//删
        //        block.Set("article.ListLink", to(new ArticleController().List, a.Tag.Id));//删


        //        block.Next();
        //    }

        //    set("page", articles.PageBar);

        //}


        public void Index()
        {
            
            DataPage<Article> articles = db.findPage<Article>("");
            List<Tag> tags = Tag.findAll();
            List<ArticleAndTag> articleAndtags = ArticleAndTag.findAll();
            
            bindArticleAndTag(articles.Results, tags, articleAndtags);

            set("page", articles.PageBar);

            set("IndexLink", to(new MainController().Index));

        }

        private void bindArticleAndTag(List<Article> articles, List<Tag> tags, List<ArticleAndTag> articleAndtags)
        {
            // 获取[文章]循环块
            IBlock block = getBlock("list");
            foreach (Article a in articles)
            {
                //设置文章
                block.Set("article.Title", a.Title);
                block.Set("article.ReadLink", to(new ArticleController().Read, a.Id));

                //将文章内容解析成纯文字并限制字数在450内
                block.Set("article.Content", a.Content);


                block.Set("article.Created", a.Created);
                block.Set("article.ReadCount", a.ReadCount);
                //设置评论次数
                List<Comment> tempListComment = db.find<Comment>("ArticleId = " + a.Id).list();
                block.Set("article.CommentCount", tempListComment.Count);

                //根据文章找标签
                // 获取[标签]循环块,进一步绑定
                IBlock tagBlock = block.GetBlock("listTags");
                List<Tag> tagsByAritcle = filterTags(tags, a, articleAndtags);


                //block.Set("article.Tag.Name", a.Tag.Name);//删
                //block.Set("article.ListLink", to(new ArticleController().List, a.Tag.Id));//删

                foreach (Tag tag in tagsByAritcle)
                {
                    tagBlock.Set("article.Tag.Name", tag.Name);
                    tagBlock.Set("article.ListLink", to(new ArticleController().List, tag.Id));

                    tagBlock.Next();
                }
                block.Next();
            }

        }

        private List<Tag> filterTags(List<Tag> tags, Article article, List<ArticleAndTag> articleAndtags)
        {
            List<Tag> results = new List<Tag>();
            foreach (ArticleAndTag aAndt in articleAndtags)
            {
                if (aAndt.article.Id == article.Id)
                {
                    Tag t = db.findById<Tag>(aAndt.tag.Id);
                    if (t != null)
                    {
                        results.Add(t);
                    }
                }
            }
            return results;
        }

        private string ConvertToWords(string HtmlContent)
        {
            return "";
        }

        #region 服务Layout方法,与Article中的Layout相同,有必要进一步改进

        private void bindTags(List<Tag> tags)
        {
            IBlock block = getBlock("tag");
            foreach (Tag t in tags)
            {
                block.Set("t.Name", t.Name);
                block.Set("t.ShowLink", to(new ArticleController().List, t.Id));
                block.Set("t.CountTag", ComputeTagCount(t));
                Tagcount = 0;
                block.Next();
            }
        }

        private void bindCategory(List<Category> categories)
        {
            IBlock block = getBlock("categories");
            foreach (Category c in categories)
            {
                block.Set("c.Name", c.Name);
                block.Set("c.ShowLink", to(new ArticleController().Index, c.Id));
                block.Set("c.Count", ComputeCount(c));
                //set("Count", ComputeCount(c));
                count = 0;
                block.Next();
            }
        }

        private void bindTopArticles(List<Article> articles)
        {
            IBlock block = getBlock("top");
            foreach (Article a in articles)
            {
                block.Set("a.Title", a.Title);
                block.Set("a.ReadLink", to(new ArticleController().Read, a.Id));

                block.Next();
            }
        }

        private string ComputeCount(Category category)
        {
            List<Article> articles = Article.findAll();
            foreach (Article article in articles)
            {
                if (article.Category == null)
                    return "0";
                if (article.Category.Id == category.Id)
                    count++;
            }
            return count.ToString();
        }

        //#region OldAritcleTag方法
        //private string ComputeTagCount(Tag tag)
        //{
        //    List<Article> articles = Article.findAll();
        //    foreach (Article article in articles)
        //    {
        //        if (article.Tag == null)
        //            return "0";
        //        if (article.Tag.Id == tag.Id)
        //            Tagcount++;
        //    }
        //    return Tagcount.ToString();
        //}
        
        //#endregion

        private string ComputeTagCount(Tag tag)
        {
            List<ArticleAndTag> articleAndtag = ArticleAndTag.find("tagId = "+tag.Id).list();
            return articleAndtag.Count.ToString() ;
        }


        public override void Layout()
        {
            bindRightList();
        }

        public void bindRightList()
        {
            //文章分类列表
            List<Category> categories = Category.findAll();
            bindCategory(categories);
            //文章总数
            set("ListLinkAll", to(new ArticleController().Listone));
            set("totalCount", Article.findAll().Count);

            //归档列表
            //1.应用SQL语句用时间进行归档
            DataTable Timetable = db.RunTable<Article>(@"SELECT t AS fileTime,COUNT(t) AS fileCount
FROM(
SELECT SUBSTRING(CONVERT(VARCHAR(100),Created,112),1,6) AS t
FROM dbo.Article) temp
GROUP BY t");

            bindfileTime(Timetable);

//            /*********************************************/
//            string sqlstr = @"select convert(varchar(7),Created,120) as Date, COUNT(*) as [Count]
//                        from
//                            dbo.Article 
//                        
//                        group by 
//                            convert(varchar(7),Created,120)
//                        order by convert(varchar(7),Created,120) desc";

//            DataTable Timetable = db.RunTable<Article>(sqlstr);
//            IBlock fileblock = getBlock("file");
//            for (int i = 0; i < Timetable.Rows.Count; i++)
//            {
//                fileblock.Set("f.TimeLink", to(new ArticleController().ReadByCreated) + "?time=" + Timetable.Rows[i]["Date"]);
//                fileblock.Set("f.Time", Timetable.Rows[i]["Date"].ToString());
//                fileblock.Set("f.Count", Timetable.Rows[i]["Count"].ToString());
//                fileblock.Next();
//            }

//            /*********************************************/


            //热门文章
            List<Article> topArticle = db.find<Article>("order by ReadCount desc").list(10);
            bindTopArticles(topArticle);

            //分类标签
            List<Tag> tags = Tag.findAll();
            bindTags(tags);
        }


        private void bindfileTime(DataTable Timetable)
        {
            if (Timetable.Rows.Count > 0)
            {
                IBlock block = getBlock("file");
                for (int i = 0; i < Timetable.Rows.Count; i++)
                {
                    DataRow dr = Timetable.Rows[i];
                    string dateString = dr["fileTime"].ToString() + "01";
                    block.Set("f.TimeLink", to(new ArticleController().ReadByCreated) + "?time=" + dateString);

                    DateTime dt = DateTime.ParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    block.Set("f.Time", string.Format("{0:Y}", dt));
                    block.Set("f.Count", dr["fileCount"].ToString());
                    block.Next();
                }
            }
        }

        #endregion


    }

}
