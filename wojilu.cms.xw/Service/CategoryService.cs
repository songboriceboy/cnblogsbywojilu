using System;
using System.Collections.Generic;
using System.Text;
using wojilu.cms.xw.Domain;
using wojilu.cms.xw.Interface;

namespace wojilu.cms.xw.Service {

    public class CategoryService : ICategoryService {

        public Category GetById( int id ) {
            return db.findById<Category>( id );
        }

        public List<Category> GetAll() {
            return db.findAll<Category>();
        }

        public Result Insert( Category c ) {
            return db.insert( c );
        }

        public Result Update( Category c ) {
            return db.update( c );
        }

    }

}
