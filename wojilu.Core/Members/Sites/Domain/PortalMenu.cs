/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu;
using wojilu.Data;
using wojilu.ORM;
using wojilu.Web.Mvc;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Menus.Interface;
using System.Collections.Generic;

namespace wojilu.Members.Sites.Domain {

    [Serializable]
    public class PortalMenuCategory : CacheObject {
    }


    [Serializable]
    public class PortalMenu : CacheObject, IMenu, IComparable {
        
        public User Creator { get; set; }

        public int OrderId { get; set; }

        public int CategoryId { get; set; }

        [NotSave]
        public int OwnerId {
            get { return Site.Instance.Id; }
            set { }
        }

        [NotSave]
        public String OwnerUrl {
            get { return ""; }
            set { }
        }

        [NotSave]
        public String OwnerType {
            get { return typeof( Site ).FullName; }
            set { }
        }

        public int ParentId { get; set; }
        public String RawUrl { get; set; }
        public String Url { get; set; }

        public String Style { get; set; }

        public DateTime Created { get; set; }
        public int OpenNewWindow { get; set; }


        public int CompareTo( object obj ) {

            SiteMenu menu = obj as SiteMenu;

            if (OrderId > menu.OrderId) return -1;
            if (OrderId < menu.OrderId) return 1;
            if (base.Id > menu.Id) return 1;
            if (base.Id < menu.Id) return -1;
            return 0;
        }

        public void updateOrderId() {
            this.update();
        }


        public static List<PortalMenu> getByCategory( int categoryId ) {

            List<PortalMenu> list = cdb.findAll<PortalMenu>();
            List<PortalMenu> results = new List<PortalMenu>();
            foreach (PortalMenu menu in list) {
                if (menu.CategoryId == categoryId) results.Add( menu );
            }

            return results;
        }


    }
}
