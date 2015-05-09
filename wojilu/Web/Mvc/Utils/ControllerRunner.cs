using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using System.Reflection;
using System.Web;

namespace wojilu.Web.Mvc.Utils {

    public class ControllerRunner {


        public static String Run( MvcContext ctx, aAction action ) {

            ControllerBase targetController = action.Target as ControllerBase;

            ControllerFactory.InjectController( targetController, ctx );
            targetController.view( action.Method.Name );
            action();
            return targetController.utils.getActionResult();
        }

        public static String Run( MvcContext ctx, aActionWithId action, int id ) {

            ControllerBase targetController = action.Target as ControllerBase;
            ControllerFactory.InjectController( targetController, ctx );
            targetController.view( action.Method.Name );
            action( id );
            return targetController.utils.getActionResult();
        }

        /// <summary>
        /// 运行某action，id由ctx.route.id自动注入。
        /// 如果action有参数，请预先设置 ctx.route.id；
        /// 如果方法中涉及到owner，请预先设置 ctx.owner；
        /// controller是经过依赖注入处理的。
        /// 注意1：未处理action过滤器批注。
        /// 注意2：因为通过controller的字符串运行，所以经过反射调用。
        /// </summary>
        /// <param name="ctx">提供 ctx.route.id 和 ctx.owner 等可能需要的参数</param>
        /// <param name="controllerFullName">控制器的完整类型type的full name</param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static String Run( MvcContext ctx, String controllerFullName, String actionName ) {

            ControllerBase controller = ControllerFactory.FindController( controllerFullName, ctx );
            if (controller == null) throw new Exception( "controller not found" );

            controller.view( actionName );
            controller.utils.runAction( actionName );

            return controller.utils.getActionResult();
        }

        /// <summary>
        /// 运行某个controller的action方法，ctx已经注入controller
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        internal static String runAction( ControllerBase controller, String actionName ) {

            MethodInfo method = getMethod( controller, actionName );
            if (method == null) {
                throw new Exception( "action " + wojilu.lang.get( "exNotFound" ) );
            }

            MvcContext ctx = controller.ctx;

            ParameterInfo[] parameters = getParameters( method );
            if (parameters.Length == 1) {
                if (parameters[0].ParameterType == typeof( String )) {
                    method.Invoke( controller, new object[] { HttpUtility.UrlDecode( ctx.route.query ) } );
                }
                else {
                    method.Invoke( controller, new object[] { ctx.route.id } );
                }
            }
            else if (parameters.Length == 0) {
                method.Invoke( controller, null );
            }
            else {
                throw new Exception( "action " + wojilu.lang.get( "exNotFound" ) );
            }

            return controller.utils.getActionResult();
        }


        /// <summary>
        /// 运行某 action
        /// </summary>
        /// <param name="actionName"></param>
        public static String Run( MvcContext ctx, String controllerName, String actionName, params object[] args ) {

            if (controllerName == null) throw new NullReferenceException();

            ControllerBase controller = ControllerFactory.FindController( controllerName, ctx );
            if (controller == null) throw new Exception( "controller not found" );

            controller.view( actionName );

            MethodInfo method = getMethod( controller, actionName );
            if (method == null) {
                throw new Exception( "action " + wojilu.lang.get( "exNotFound" ) );
            }

            method.Invoke( controller, args );

            return controller.utils.getActionResult();
        }

        /// <summary>
        /// 根据名称获取某 action 的方法信息
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static MethodInfo getMethod( ControllerBase controller, String actionName ) {

            return controller.GetType().GetMethod( actionName );
        }

        /// <summary>
        /// 获取某方法的所有参数信息
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static ParameterInfo[] getParameters( MethodInfo method ) {
            return method.GetParameters();
        }



    }

}
