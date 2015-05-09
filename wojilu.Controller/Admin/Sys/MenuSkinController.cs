using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Admin.Sys {

    public class MenuSkinController : ControllerBase {

        public void Index() {
        }

        public void List() {

            set( "addCategoryLink", to( AddCategory ) );

            List<PortalMenuCategory> cats = cdb.findAll<PortalMenuCategory>();
            IBlock block = getBlock( "category" );
            foreach (PortalMenuCategory cat in cats) {

                bindMenuList( block, cat );

                block.Next();
            }

            IBlock cmdBlock = getBlock( "catCmd" );
            foreach (PortalMenuCategory cat in cats) {

                cmdBlock.Set( "addMenuLink", to( AddMenu, cat.Id ) );

                cmdBlock.Next();
            }


        }

        private void bindMenuList( IBlock cblock, PortalMenuCategory cat ) {
            IBlock block = cblock.GetBlock( "menuList" );

            List<PortalMenu> menuList = PortalMenu.getByCategory( cat.Id );
            foreach (PortalMenu menu in menuList) {
                block.Set( "menu.Name", menu.Name );
                block.Set( "menu.Url", menu.Url );
                block.Next();
            }

        }

        [HttpPost]
        public void AddCategory() {
            PortalMenuCategory cat = new PortalMenuCategory();
            cat.insert();

            redirect( List );
        }



        public void DeleteCategory() {
        }

        //--------------------------------------------------

        public void AddMenu( int id ) {
            target( SaveMenu, id );
        }

        [HttpPost]
        public void SaveMenu( int id ) {

            PortalMenu menu = new PortalMenu();
            menu.Name = ctx.Post( "Name" );
            menu.Url = ctx.Post( "Url" );
            menu.CategoryId = id;
            menu.insert();

            echoToParentPart( lang("opok") );

        }

        public void EditMenu( int id ) {
            target( UpdateMenu, id );
        }

        public void UpdateMenu( int id ) {
        }

        public void DeleteMenu( int id ) {
        }

        //--------------------------------------------------

    }

}
