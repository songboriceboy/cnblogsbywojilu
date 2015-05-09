using System;
using System.Collections.Generic;
using System.Text;
using wojilu.cms.xw.Interface;
using wojilu.cms.xw.Domain;

namespace wojilu.cms.xw.Service {

    public class NewCategoryService : ICategoryService {

        public List<Category> GetAll() {
            return new List<Category> {
                new Category{ Id=1, Name="新分类1"},
                new Category{ Id=1, Name="新分类2"}
            };
        }

        public Category GetById( int id ) {
            throw new NotImplementedException();
        }

        public Result Insert( Category c ) {
            throw new NotImplementedException();
        }

        public Result Update( Category c ) {
            throw new NotImplementedException();
        }

    }

}
