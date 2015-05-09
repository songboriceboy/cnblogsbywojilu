/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Caching;

namespace wojilu.Web.Mvc.Processors {

    internal class LayoutProcessor : ProcessorBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( LayoutProcessor ) );

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginAddLayout( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MvcContext ctx = context.ctx;
            ControllerBase controller = context.getController();

            if (controller.utils.isHided( controller.GetType() )) {
                return;
            }

            int intNoLayout = ctx.utils.getNoLayout();
            IList paths = ctx.utils.getLayoutPath();
            if (intNoLayout >= paths.Count + 1) return;


            String actionContent = context.getContent();


            //检查缓存
            String cacheKey = null;
            if (MvcConfig.Instance.IsActionCache) {
                IActionCache actionCache = ControllerMeta.GetActionCache( controller.GetType(), "Layout" );

                cacheKey = getCacheKey( actionCache, ctx );
                if (strUtil.HasText( cacheKey )) {
                    Object cacheContent = checkCache( cacheKey );
                    if (cacheContent != null) {
                        logger.Info( "load from layoutCache=" + cacheKey );
                        context.setContent( HtmlCombiner.combinePage( cacheContent.ToString(), actionContent ) );
                        return;
                    }
                }
            }

            String layoutContent;
            if (controller.LayoutControllerType == null) {
                layoutContent = runCurrentLayout( controller, ctx, context, actionContent );
            }
            else {
                layoutContent = runOtherLayout( controller, ctx, context, actionContent );
            }

            // 加入缓存
            if (MvcConfig.Instance.IsActionCache) {
                if (strUtil.HasText( cacheKey )) {
                    addContentToCache( cacheKey, layoutContent );
                }
            }

            if (ctx.utils.isEnd()) {
                context.endMsgByText( layoutContent );
            }
            else {
                context.setContent( HtmlCombiner.combinePage( layoutContent, actionContent ) );
            }

        }

        private String getCacheKey( IActionCache actionCache, MvcContext ctx ) {
            if (actionCache == null) return null;
            if (ctx.HttpMethod.Equals( "GET" ) == false) return null;
            return actionCache.GetCacheKey( ctx, "Layout" );
        }

        private String runCurrentLayout( ControllerBase controller, MvcContext ctx, ProcessContext context, String actionContent ) {

            MethodInfo layoutMethod = controller.utils.getMethod( "Layout" );
            if (layoutMethod.DeclaringType != controller.GetType()) {
                String filePath = MvcUtil.getParentViewPath( layoutMethod, ctx.route.getRootNamespace( layoutMethod.DeclaringType.FullName ) );
                controller.utils.setCurrentView( controller.utils.getTemplateByFileName( filePath ) );
            }
            else {
                controller.utils.switchViewToLayout();
            }

            controller.actionContent( "" ); // 清理当前内容，否则下面的utils.getActionResult()得不到正确结果
            controller.Layout();

            return controller.utils.getActionResult();
        }

        private String runOtherLayout( ControllerBase controller, MvcContext ctx, ProcessContext context, String actionContent ) {

            ControllerBase layoutController = ControllerFactory.FindController( controller.LayoutControllerType, ctx );
            layoutController.utils.switchViewToLayout();

            ActionRunner.runLayoutAction( ctx, layoutController, layoutController.Layout );

            return layoutController.utils.getActionResult();
        }

        private static void addContentToCache( String cacheKey, String actionResult ) {
            CacheManager.GetApplicationCache().Put( cacheKey, actionResult );
            logger.Info( "add layoutCache=" + cacheKey );
        }

        private static Object checkCache( String cacheKey ) {
            return CacheManager.GetApplicationCache().Get( cacheKey );
        }

    }

}
