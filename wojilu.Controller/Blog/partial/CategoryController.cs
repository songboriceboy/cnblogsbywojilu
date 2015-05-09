/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Web.Mvc;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Controller.Common;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Blog {

    public partial class CategoryController : ControllerBase {


        private void bindCategory( BlogCategory category ) {

            WebUtils.pageTitle( this, category.Name );
            set( "categoryName", category.Name );
            set( "categoryDescription", category.Description );
        }

        private void bindPostList( DataPage<BlogPost> list ) {
            IBlock block = getBlock( "bloglist" );
            foreach (BlogPost p in list.Results) {
                bindPostOne( block, p );
                block.Next();
            }
            set( "pager", list.PageBar );
        }

        private void bindPostOne( IBlock listBlock, BlogPost post ) {

            String status = string.Empty;
           // if (post.IsTop == 1) status = "<span class=\"lblTop\">[" + lang( "sticky" ) + "]</span>";
         //   if (post.IsPick == 1) status = status + "<span class=\"lblTop\">[" + lang( "picked" ) + "]</span>";


           // listBlock.Set( "blogpost.Status", status );
            listBlock.Set( "blogpost.Title", post.Title );
            listBlock.Set( "blogpost.Url", alink.ToAppData( post ) );
            listBlock.Set( "blogpost.Body", strUtil.ParseHtml( post.Content, 300 ) );
            listBlock.Set( "author", ctx.owner.obj.Name );
            listBlock.Set( "authroUrl", Link.ToMember( ctx.owner.obj ) );
            listBlock.Set( "blogpost.CreateTime", post.Created.ToShortTimeString() );
            listBlock.Set( "blogpost.CreateDate", post.Created.ToShortDateString() );
            listBlock.Set( "blogpost.Hits", post.Hits );
            listBlock.Set( "blogpost.ReplyCount", post.Replies );
            if (ctx.viewer.IsLogin && (ctx.viewer.Id == ctx.owner.Id))
            {
                listBlock.Set("EditUrlStyle", "");

            }
            else
            {
                listBlock.Set("EditUrlStyle", "display:none");

            }
            listBlock.Set("blogpost.EditUrl", to(new Admin.PostController().Edit, post.Id));

        }
        private void bindTopPosts(BlogSetting s, IBlock block)
        {
            List<BlogPost> top = postService.GetTop(ctx.app.Id, s.StickyCount);
            foreach (BlogPost post in top)
            {
                bindPostOne(block, post);
                block.Next();
            }
        }

    }
}

