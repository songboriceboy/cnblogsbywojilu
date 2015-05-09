using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.cms.xw.Domain
{
    public class Comment : ObjectBase<Comment>
    {
        //用户名
        public string UserName { get; set; }
        //博客地址
        public string BlogAddress { get; set; }
        //邮箱地址
        public string EmailAddress { get; set; }
        //留言内容
        public string Content { get; set; }
        //文章外键
        public Article Article { get; set; }
        //几楼,排列顺序
        public int OrderBy { get; set; }
        //评论时间
        public DateTime Created { get; set; }
    }
}
