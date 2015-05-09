using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.cms.xw.Domain;
using wojilu.Web;
using wojilu.cms.xw.Domain.backup;
using System.Data;
using System.Text.RegularExpressions;

namespace wojilu.cms.xw.Controller
{

    public class ArticleController : ControllerBase
    {

        private int count = 0;
        private int Tagcount = 0;

        public void Index(int id)
        {

            Category c = Category.findById(id);
            ctx.SetItem("category", c);//??将某个对象存储在ctx中,方便不同的Controller和Action之间调用,可以看做ViewState

            DataPage<Article> list = Article.findPage("CategoryId=" + id);
            IBlock block = getBlock("list");
            foreach (Article a in list.Results)
            {
                block.Set("article.Id", a.Id);
                block.Set("article.Title", a.Title);
                block.Set("article.ReadLink", to(new ArticleController().Read, a.Id));//设置当前form的提交网址,即将链接地址url设置到变量上,当点击时,则跳转到相应的方法中
                block.Set("article.Created", a.Created);
                block.Set("article.ReadCount", a.ReadCount);
                block.Next();//非常重要,不写则不能加载其余列表
            }

            set("page", list.PageBar);
        }

        public void Read(int id)
        {

            HideLayout(typeof(wojilu.cms.xw.Controller.ArticleController));

            Article a = Article.findById(id);
            a.ReadCount++;
            db.update(a);
            bind("article", a);

            set("CommentLink", to(new ArticleController().SaveComment, id));//保存文章的评论到数据库中

            //ctx.SetItem( "article", a );
            //ctx.SetItem( "category", a.Category );

            //设置评论到Read页面
            string sqlStr = string.Format("select * from Comment where ArticleId = {0} order by OrderBy asc", id);
            List<Comment> commentsByArticleId = db.findBySql<Comment>(sqlStr);

            IBlock blockComment = getBlock("listcomment");
            foreach (Comment comment in commentsByArticleId)
            {
                blockComment.Set("comment.OrderBy", comment.OrderBy);
                blockComment.Set("comment.Created", comment.Created);
                blockComment.Set("comment.BlogAddress", comment.BlogAddress);
                blockComment.Set("comment.UserName", comment.UserName);
                blockComment.Set("comment.Content", comment.Content);

                blockComment.Next();
            }

            //显示标签
            List<Tag> tags = Tag.findAll();
            List<ArticleAndTag> articleAndtags = ArticleAndTag.findAll();

            bindArticleAndTag(a, tags, articleAndtags);
        }

        private void bindArticleAndTag(Article articles, List<Tag> tags, List<ArticleAndTag> articleAndtags)
        {

            //设置评论次数
            List<Comment> tempListComment = db.find<Comment>("ArticleId = " + articles.Id).list();
            set("article.ReadLink", to(new ArticleController().Read, articles.Id));
            set("article.CommentCount", tempListComment.Count);

            //根据文章找标签
            IBlock tagBlock = getBlock("listTags");
            List<Tag> tagsByAritcle = filterTags(tags, articles, articleAndtags);

            foreach (Tag tag in tagsByAritcle)
            {
                tagBlock.Set("article.Tag.Name", tag.Name);
                tagBlock.Set("article.ListLink", to(new ArticleController().List, tag.Id));

                tagBlock.Next();
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

        public void SaveComment(int id)
        {

            Comment comment = ctx.PostValue<Comment>();

            //comment.UserName = ctx.Post("userName");
            //comment.BlogAddress = ctx.Post("blogUrl");
            //comment.EmailAddress = ctx.Post("email");
            //comment.Content = ctx.Post("content");

            comment.UserName = ctx.Get("userName");
            comment.BlogAddress = ctx.Get("blogUrl");
            comment.EmailAddress = ctx.Get("email");
            comment.Content = ctx.Get("content");

            //comment.Article.Id = id;
            comment.Article = new Article { Id = id };

            comment.Created = DateTime.Now;
            List<Comment> templist = db.find<Comment>("ArticleId = " + id).list();
            comment.OrderBy = templist.Count + 1;

            Result result = db.insert(comment);

            if (result.HasErrors)
            {
                errors.Join(result);
                run(Read, id);
            }
            else
            {
                Article a = Article.findById(id);
                a.ReadCount--;
                db.update(a);
                redirect(Read, id);
            }

        }

        public void Listone()
        {
            //List<Article> articles = Article.findAll();
            DataPage<Article> articles = db.findPage<Article>("");
            IBlock block = getBlock("list");
            foreach (Article a in articles.Results)
            {
                block.Set("article.Id", a.Id);
                block.Set("article.ReadLink", to(new ArticleController().Read, a.Id));
                block.Set("article.Title", a.Title);
                block.Set("article.Created", a.Created);
                block.Set("article.ReadCount", a.ReadCount);

                block.Next();
            }

            set("page", articles.PageBar);
        }

        public void List(int id)
        {
            //通过Tag的id在ArticleAndTag找到所要Article的id
            List<ArticleAndTag> articleAndtag = db.find<ArticleAndTag>("tagId = " + id).list();

            DataPage<Article> articles = db.findPage<Article>("");
            IBlock block = getBlock("list");
            foreach (Article a in articles.Results)
            {
                foreach (ArticleAndTag aAndt in articleAndtag)
                {
                    if (aAndt.article.Id == a.Id)
                    {
                        //bind("list", a);
                        block.Set("article.Id", a.Id);
                        block.Set("article.ReadLink", to(new ArticleController().Read, a.Id));
                        block.Set("article.Title", a.Title);
                        block.Set("article.Created", a.Created);
                        block.Set("article.ReadCount", a.ReadCount);

                        block.Next();
                    }
                }
            }
            set("page", articles.PageBar);
        }

        public void ReadByCreated()
        {
            string datestr = ctx.Get("time");//挺重要的

            string nextMonthStr = getNextMonth(datestr);

            //            string queryStr = @"SELECT *
            //FROM dbo.Article
            //WHERE Created BETWEEN '" + datestr + "' AND '" + nextMonthStr + "'";

            //DataTable dt = db.RunTable<Article>(queryStr);

            //IBlock block = getBlock("list");
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dt.Rows[i];
            //    block.Set("article.Id", dr["Id"].ToString());
            //    block.Set("article.Title", dr["Title"].ToString());
            //    block.Set("article.ReadLink", to(new ArticleController().Read, int.Parse(dr["Id"].ToString())));
            //    block.Set("article.Created", dr["Created"].ToString());
            //    block.Set("article.ReadCount", dr["ReadCount"].ToString());
            //    block.Next();
            //}

            //尝试使用另一种方法
            string queryStr = @"Created BETWEEN '" + datestr + "' AND '" + nextMonthStr + "'";
            DataPage<Article> list = db.findPage<Article>(queryStr);
            IBlock block = getBlock("list");
            foreach (Article a in list.Results)
            {
                block.Set("article.Id", a.Id);
                block.Set("article.Title", a.Title);
                block.Set("article.ReadLink", to(new ArticleController().Read, a.Id));
                block.Set("article.Created", a.Created);
                block.Set("article.ReadCount", a.ReadCount);
                block.Next();
            }
            set("page", list.PageBar);
        }

        private string getNextMonth(string thisMonth)
        {
            DateTime date1 = DateTime.ParseExact(thisMonth, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            date1 = date1.AddMonths(1);

            return date1.Year.ToString() + date1.Month.ToString().PadLeft(2, '0') + date1.Day.ToString().PadLeft(2, '0');
        }
        //



        #region Layout有必要进一步改进

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
            List<ArticleAndTag> articleAndtag = ArticleAndTag.find("tagId = " + tag.Id).list();
            return articleAndtag.Count.ToString();
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
