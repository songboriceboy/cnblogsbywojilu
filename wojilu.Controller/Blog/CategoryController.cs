/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public partial class CategoryController : ControllerBase {

        public IBlogCategoryService categoryService { get; set; }
        public IBlogPostService postService { get; set; }

        public CategoryController() {
            postService = new BlogPostService();
            categoryService = new BlogCategoryService();
        }

        public void Show( int id ) {


            BlogCategory category = categoryService.GetById(id, ctx.owner.Id);

            if (category == null)
            {
                echoRedirect(lang("exDataNotFound"));
                return;
            }

            //List<BlogCategory> lstblogcategory = categoryService.GetByIdName(id);

            BlogApp app = ctx.app.obj as BlogApp;
            BlogSetting s = app.GetSettingsObj();


            DataPage<BlogPost> list = postService.GetPageByCategory( ctx.app.Id, id, s.PerPageBlogs );
            //bindCategory("list", "post", blogs, bindLink);

            //bindCategory( category );
            


            //IBlock block = getBlock("topblog");
            //if (ctx.route.page == 1)
            //{
            //    bindTopPosts(s, block);
            //}

            bindPostList(list);

          

            set( "pager", list.PageBar );
        }

        /// <summary>
        /// 列出相应发表日期的文章
        /// </summary>
        /// <param name="id"></param>
        public void List(int id)
        {

            

            BlogApp app = ctx.app.obj as BlogApp;
            BlogSetting s = app.GetSettingsObj();

            DataPage<BlogPost> list = postService.GetPageByCreatedCate(ctx.app.Id, id, s.PerPageBlogs);
            //bindCategory("list", "post", blogs, bindLink);

            //bindCategory( category );
            bindPostList(list);

            set("pager", list.PageBar);
        }
    }
}

