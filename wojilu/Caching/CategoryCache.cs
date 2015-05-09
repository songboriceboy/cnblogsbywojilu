//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace wojilu.Caching {

//    public abstract class CategoryCache {

//        private static IApplicationCache cacheTool = new ApplicationCache();

//        public abstract String getCacheKey();

//        public Dictionary<String, String> GetDB() {

//            Dictionary<String, String> pageCache = cacheTool.Get( getCacheKey() ) as Dictionary<String, String>;

//            return pageCache;
//        }

//        public String GetContent( String key ) {

//            Dictionary<String, String> db = this.GetDB();
//            if (db == null) return null;

//            String pageContent;
//            GetDB().TryGetValue( key, out pageContent );

//            return pageContent;
//        }

//        public void AddCache( String key, String content ) {

//            Dictionary<String, String> db = this.GetDB();
//            if (db == null) {
//                db = new Dictionary<String, String>();

//                db.Add( key, content );

//                cacheTool.Put( getCacheKey(), db );

//            }

//            else {

//                db.Add( key, content );

//            }
//        }

//        public Boolean Remove( String key ) {

//            Dictionary<String, String> db = this.GetDB();
//            if (db == null) return false;

//            return db.Remove( key );

//        }



//    }
//}
