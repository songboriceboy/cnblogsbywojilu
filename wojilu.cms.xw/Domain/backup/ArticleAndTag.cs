using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.cms.xw.Domain.backup
{
    public class ArticleAndTag : ObjectBase<ArticleAndTag>
    {
        public Article article { get; set; }

        public Tag tag { get; set; }
    }
}
