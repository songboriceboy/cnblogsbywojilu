using System;
using System.Collections.Generic;
using wojilu.cms.xw.Domain;

namespace wojilu.cms.xw.Interface {

    public interface ICategoryService {

        List<Category> GetAll();
        Category GetById( int id );

        Result Insert( Category c );
        Result Update( Category c );

    }

}
