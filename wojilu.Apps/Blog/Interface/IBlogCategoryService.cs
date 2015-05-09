/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;
using System.Data;

namespace wojilu.Apps.Blog.Interface {


    public interface IBlogCategoryService {

        List<BlogCategory> GetByApp( int appId );
        BlogCategory GetById( int id, int ownerId );
        void Insert( BlogCategory category );
        void Delete( BlogCategory category );

        void RefreshCache( BlogCategory category );
        List<BlogCategory> GetAll();
        List<BlogCategory> GetByIdName(int id);

    }
}

