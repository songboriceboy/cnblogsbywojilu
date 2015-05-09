/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Common.Comments;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;

namespace wojilu.Web.Controller.Blog {

    public partial class LayoutController : ControllerBase {


        private void bindAppInfo( BlogApp blog ) {
            
            int nComments = (int)getBlogComments( blog );

            set( "blog.Count", blog.BlogCount );
            set( "blog.Hits", blog.Hits );
            set( "blog.CommentCount", nComments );
            set("blog.Score", blog.BlogCount * 10 + 5 * nComments + blog.Hits);
            //set( "blog.RssUrl", Link.To( new BlogController().Rss ) );

            Page.RssLink = Link.To( new BlogController().Rss );

        }

        private object getBlogComments( BlogApp blog ) {

            return BlogPostComment.count( "AppId=" + blog.Id );

            //return blog.CommentCount;
        }
     
        private void bindCategories( List<BlogCategory> categories ) {
            IBlock catblock = getBlock( "category" );
            foreach (BlogCategory category in categories) {
                int nBlogCount = postService.GetCountByCategory(ctx.app.Id, category.Id);
                if(nBlogCount <= 0)
                    continue;
                catblock.Set( "category.Title", category.Name );
                catblock.Set( "category.Url", Link.To( new CategoryController().Show, category.Id ) );
                //在此分类下共有多少文章
                
                //int blogcount = BlogPost.find("CategoryId=" + category.Id).list().Count;
                catblock.Set("category.BlogCount", nBlogCount);
                catblock.Next();
            }
        }

        private void bindPostList( List<BlogPost> newBlogs ) {
            IBlock postblock = getBlock( "newpost" );
            foreach (BlogPost post in newBlogs) {
                postblock.Set( "post.Title", strUtil.SubString( post.Title, 14 ) );
                postblock.Set( "post.Url", alink.ToAppData( post ) );
                postblock.Next();
            }
        }

        private void bindBlogroll( List<Blogroll> blogrolls ) {
            IBlock rollblock = getBlock( "myBlogroll" );
            foreach (Blogroll blogroll in blogrolls) {
                rollblock.Set( "roll.Name", strUtil.SubString( blogroll.Name, 10 ) );
                rollblock.Set( "roll.Link", blogroll.Link );
                rollblock.Next();
            }

        }

        private void bindBlogByDateTime(List<BlogPost> BlogPosts)
        {
            List<string> lstCreatedYM = new List<string>();
            IBlock blogfileblock = getBlock("blogfile");
            foreach (BlogPost blogpost in BlogPosts)
            {
                DateTime dtCreated = blogpost.Created;
                //string strCreatedYM2 = dtCreated.ToString("yyyy-M");
                string strCreatedYM2 = dtCreated.ToString("yyyyMM");
                //int nYM = Int32.Parse(strCreatedYM2);
                int nYM = Int32.Parse(dtCreated.ToString("yyyyMM"));

                if (!lstCreatedYM.Contains(strCreatedYM2))
                    lstCreatedYM.Add(strCreatedYM2);
                else
                    continue;

                blogfileblock.Set("blogfile.Created", strCreatedYM2);
                //在此发表年月的文章有多少
                string startdate = strCreatedYM2;
                //string enddate = dtCreated.AddMonths(1).ToString("yyyy-M");
                string enddate = dtCreated.AddMonths(1).ToString("yyyyMM");
                int blogcount = postService.GetCountByCreated(blogpost.AppId, startdate, enddate);
                blogfileblock.Set("blogfile.BlogCount", blogcount);

                blogfileblock.Set("blogfile.Url", Link.To(new CategoryController().List, nYM));
                blogfileblock.Next();
            }

            //String commentMoreLink = BlogCommentController.GetCommentMoreLink(newComments.Count, ctx);
            //set("commentMoreLink", commentMoreLink);
        }

        private void bindComments( List<BlogPostComment> newComments ) {
            IBlock commentblock = getBlock( "comment" );
            foreach (BlogPostComment comment in newComments) {
                commentblock.Set( "comment.Title", strUtil.SubString( comment.Content, 14 ) );
                commentblock.Set( "comment.Url", Link.To( new PostController().Show, comment.RootId ) + "#comments" );
                commentblock.Next();
            }

            //String commentMoreLink = BlogCommentController.GetCommentMoreLink( newComments.Count, ctx );
            //set( "commentMoreLink", commentMoreLink );
        }

    }

}
