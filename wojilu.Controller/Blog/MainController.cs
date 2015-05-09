/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;


using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common.Resource;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Blog.Caching;
using System.Collections.Generic;
using System.Data;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public partial class MainController : ControllerBase {

        public IBlogPostService postService { get; set; }
        public IUserService userService { get; set; }
        public IPickedService pickedService { get; set; }
        public ISysBlogService sysblogService { get; set; }
        public IBlogCategoryService categoryService { get; set; }
        public IBlogCommentService blogcommentService { get; set; }
        private String key = "";

        public MainController() {
            postService = new BlogPostService();
            userService = new UserService();
            pickedService = new PickedService();
            sysblogService = new SysBlogService();
            categoryService = new BlogCategoryService();
            blogcommentService = new BlogCommentService();

            HideLayout( typeof( LayoutController ) );
        }

        [CacheAction( typeof( BlogMainLayoutCache ) )]
        public override void Layout() {

            // TODO 博客排行
            IList userRanks = User.find("order by Hits desc, id desc").list(21);
            bindUsers(userRanks);
            //userRanks = User.find("1 = 1 and gender = 1 order by Hits desc, id desc").list(21);
            //bindTechUsers(userRanks);
            //userRanks = User.find("1 = 1 and gender = 2 order by Hits desc, id desc").list(21);
            //bindHumanityUsers(userRanks);
            //IList tops = pickedService.GetTop( 10 );
            //IList hits = sysblogService.GetSysHit( 20 );
            //IList replies = sysblogService.GetSysReply( 30 );

         //   bindSidebar( tops, hits, replies );

            List<BlogCategory> categories = categoryService.GetAll();

         //   bindCategories(categories);


            String condition = "";
            DateTime timeReadLine = DateTime.Now.AddHours(-72);

            //condition = string.Format("IsPick = 1 and Created>#{0}# order by Hits desc,Created", timeReadLine);
            condition = string.Format("IsPick = 1 and Created>'{0}' order by Hits desc,Created", timeReadLine);
            DataPage<BlogPost> blogs = sysblogService.GetSysPageBySearch(condition);

            IBlock catblock = getBlock("bloglist");
            foreach (BlogPost blog in blogs.Results)
            {


                catblock.Set("blogpost.Title", blog.Title);
                catblock.Set("blogpost.Url", alink.ToAppData(blog));

                catblock.Next();
            }




            condition = string.Format("IsPic = 1 order by Hits desc,Created");
            DataPage<BlogPost> blogbests = sysblogService.GetSysPageBySearch(condition);

            IBlock catbestblock = getBlock("bloglistbest");
            foreach (BlogPost blog in blogbests.Results)
            {


                catbestblock.Set("blogpost.Title", blog.Title);
                catbestblock.Set("blogpost.Url", alink.ToAppData(blog));

                catbestblock.Next();
            }

            bindStatData();
        }

        private void bindCategories(List<BlogCategory> categories)
        {
            Dictionary<string, int> dicCateName = new Dictionary<string, int>();
            int CateNameCount = 0;
            List<string> lstCateName = new List<string>();

            foreach (BlogCategory category in categories)
            {
                if (!dicCateName.ContainsKey(category.Name))
                    CateNameCount = 0;

                dicCateName[category.Name] = ++CateNameCount;
            }


            IBlock catblock = getBlock("category");
            foreach (BlogCategory category in categories)
            {
                if (!lstCateName.Contains(category.Name))
                    lstCateName.Add(category.Name);
                else
                    continue;

                catblock.Set("category.Title", category.Name);
                catblock.Set("category.Url", Link.To(new CategoryController().Show, category.Id));
                catblock.Set("category.Count", dicCateName[category.Name]);
                catblock.Next();
            }
        }

        private void bindStatData()
        {
            //User user = ctx.owner.obj as User;
            set("m.UserCount", userService.GetUserCount());
            set("m.BlogCount", postService.GetBlogCount());
            set("m.BlogCommentCount", blogcommentService.GetBlogCommentCount());
            
            //ctx.viewer
        }

       // [CachePage( typeof( BlogMainPageCache ) )]
       // [CacheAction( typeof( BlogMainCache ) )]
        public void Index() {

            WebUtils.pageTitle( this, lang( "blog" ) );

            //// TODO 博客排行
            //IList userRanks = User.find( "order by Hits desc, id desc" ).list( 21 );
            //bindUsers( userRanks );

           // IList blogs = sysblogService.GetSysNew(-1, 5);
            String condition = "";
            condition = "IsPick = 1 order by Created desc";
            DataPage<BlogPost> results = sysblogService.GetSysPageBySearch(condition);
            bindList("list", "post", results.Results, bindLink);
            
            set("pager", results.PageBar);
            //set( "recentLink", to( Recent ) );
            //set( "recentLink2", Link.AppendPage( to( Recent ), 2 ) );
            //set( "droplist", getDropList( 0 ) );
        }


        public void Recent() {

            WebUtils.pageTitle( this, alang( "allBlogPost" ) );

        

            String condition = "";
            condition = "IsPick = 1 order by Created desc";

            

            DataPage < BlogPost > results = sysblogService.GetSysPageBySearch(condition);
            //set( "qword", qword );
            //set( "droplist", getDropList( qtype ) );

            bindList("list", "post", results.Results, bindLink);
            set("pager", results.PageBar);
            //set( "page", blogs.PageBar );
            //set( "recentLink", to( Recent ) );
        }

        public void ReadHits()
        {

            WebUtils.pageTitle(this, alang("allBlogPost"));

            DataPage<BlogPost> blogs = null;

            String condition = "";
            DateTime timeReadLine = DateTime.Now.AddHours(-72);

            //condition = string.Format("IsPick = 1 and Created>#{0}# order by Hits desc,Created", timeReadLine);
            condition = string.Format("IsPick = 1 and Created>'{0}' order by Hits desc,Created", timeReadLine);
            blogs = sysblogService.GetSysPageBySearch(condition);

            bindList("list", "post", blogs.Results, bindLink);
            set("pager", blogs.PageBar);
            //set( "recentLink", to( Recent ) );
        }

        public void Results()
        {
            key = ctx.Get("txtSearch");
            ctx.SetItem("searchKey", key);

            set("searchKey", key);

            DataPage<BlogPost> results = postService.Search(key, 20);

            bindList("list", "post", results.Results, bindLink);
            set("pager", results.PageBar);

        }

    }

}
