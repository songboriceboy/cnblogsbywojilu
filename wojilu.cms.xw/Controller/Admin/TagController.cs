using System;
using System.Collections.Generic;
using System.Text;
using wojilu.cms.xw.Domain.backup;
using wojilu.Web;
using wojilu.Web.Mvc;

namespace wojilu.cms.xw.Controller.Admin
{
    public class TagController : ControllerBase
    {
        public void Index()
        {
            List<Tag> list = Tag.findAll();
            bindList("list", "t", list, bindLink);
        }

        private void bindLink(IBlock block, int id)
        {
            block.Set("t.EditLink", to(Edit, id));
            block.Set("t.DeleteLink", to(Delete, id));
        }

        //public void Showone()
        //{
        //    List<Tag> list = Tag.findAll();
        //    bindList("list", "t", list);
        //    target(new ArticleController().AddTagToText);

        //}

        public void Showone()
        {
            HideLayout(typeof(wojilu.cms.xw.Controller.Admin.LayoutController));

            List<Tag> list = Tag.findAll();

            IBlock tagsblock = getBlock("list");
            foreach (Tag tag in list)
            {
                tagsblock.Set("tag.Name", tag.Name);
                tagsblock.Next();
            }
        }



        public void Add()
        {
            target(Create);
        }

        public void Create()
        {
            Tag t = ctx.PostValue<Tag>();
            Result result = db.insert(t);
            if (result.HasErrors)
            {
                errors.Join(result);
                run(Add);
            }
            else
                redirect(Index);
        }

        public void Edit(int id)
        {
            target(Update, id);
            Tag t = Tag.findById(id);
            bind(t);
        }

        public void Update(int id)
        {
            Tag c = Tag.findById(id);
            c = ctx.PostValue(c) as Tag;
            db.update(c);
            redirect(Index);
        }

        public void Delete(int id)
        {
            Tag t = Tag.findById(id);
            db.delete(t);
            redirect(Index);
        }
    }
}
