using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.cms.xw.Domain.backup;

namespace wojilu.cms.xw.Domain {

    public class Article : ObjectBase<Article> {

        public Category Category { get; set; }

        [NotNull( "请输入标题" ), Column( Length = 500 )]
        public string Title { get; set; }

        [LongText, NotNull( "请输入内容" )]
        public string Content { get; set; }

        public DateTime Created { get; set; }

        public int ReadCount { get; set; }

    }

}
