using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.cms.xw.Domain.backup
{
    public class Tag:ObjectBase<Tag>
    {
        [Column(Length = 10), NotNull("请输入名称")]
        public string Name { get; set; }


    }
}
