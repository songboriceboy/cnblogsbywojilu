using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Jobs;
using wojilu.Common.Spider.Domain;
using wojilu.Data;
using System.Threading;
using wojilu.DI;
using wojilu.Common.Spider.Interface;
using wojilu.Members.Users.Domain;
using System.Collections;

namespace wojilu.Common.Spider.Service {

    public class SpiderJob : IWebJobItem {

        private static Random rd = new Random();

        private static readonly ILog logger = LogManager.GetLogger( typeof( SpiderJob ) );

        public ISpiderTool defaultSpider { get; set; }

        public SpiderJob() {
            defaultSpider = new SpiderTool();
        }

        public void Execute() {

           // List<SpiderTemplate> list = SpiderTemplate.find( "IsDelete=0" ).list();
            DbContext.closeConnectionAll();

            

            StringBuilder log = new StringBuilder();


            IList userRanks = User.find("order by Hits desc, id desc").list(1000);
            logger.Info("begin SpiderJob=" + userRanks.Count);

            foreach (User user in userRanks)
            {
                if (string.IsNullOrEmpty(user.Profile.Address))
                    continue;
                SpiderTemplate s = new SpiderTemplate();
                s.ListUrl = user.Profile.Address;
                s.ListEncoding = user.QQ;
                s.ListBodyPattern = user.Profile.Tel;
                s.ListPattern = user.Profile.WebSite;
                s.DetailPattern = user.MSN;
                s.IsDelete = user.Id;
                s.SiteName = user.Url;
                ISpiderTool spider = getSpider(s);

                spider.DownloadPage(s, log, new int[] { SpiderConfig.SuspendFrom, SpiderConfig.SuspendTo }); // 2~6秒暂停
                DbContext.closeConnectionAll();

                int sleepms = rd.Next(SpiderConfig.SuspendFrom, SpiderConfig.SuspendTo);
                Thread.Sleep(sleepms);
            }
            //foreach (SpiderTemplate s in list) {

            //    ISpiderTool spider = getSpider( s );

            //    spider.DownloadPage( s, log, new int[] { SpiderConfig.SuspendFrom, SpiderConfig.SuspendTo } ); // 2~6秒暂停
            //    DbContext.closeConnectionAll();

            //    int sleepms = rd.Next( SpiderConfig.SuspendFrom, SpiderConfig.SuspendTo );
            //    Thread.Sleep( sleepms );
            //}

            String[] arrLog = log.ToString().Split( '\n' );
            StringBuilder errorLog = new StringBuilder();
            foreach (String item in arrLog) {
                if (item.Trim().StartsWith( "error=" )) errorLog.AppendLine( item.Trim() );
            }

            SpiderLog sg = new SpiderLog();
            sg.Msg = errorLog.ToString();
            sg.insert();
            DbContext.closeConnectionAll();

        }

        private ISpiderTool getSpider( SpiderTemplate s ) {

            if (strUtil.IsNullOrEmpty( s.SpiderType )) return defaultSpider;

            ISpiderTool spider = ObjectContext.GetByType( s.SpiderType ) as ISpiderTool;
            if (spider == null) return defaultSpider;

            return spider;
        }

        public void End() {
            DbContext.closeConnectionAll();
        }

    }

}
