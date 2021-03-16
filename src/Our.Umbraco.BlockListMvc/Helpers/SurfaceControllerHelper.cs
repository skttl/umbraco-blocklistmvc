using Our.Umbraco.BlockListMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Composing;
using Umbraco.Web.Mvc;

namespace Our.Umbraco.BlockListMvc.Helpers
{
    internal static class SurfaceControllerHelper
    {
        public static bool SurfaceControllerExists(string controllerName, string actionName = "Index")
        {
            using (Current.ProfilingLogger.DebugDuration<BlockListMvcSurfaceController>($"BlockListMvcSurfaceController exists ({controllerName}, {actionName})"))
            {
                // Setup dummy route data
                var rd = new RouteData();
                rd.DataTokens.Add("area", "umbraco");
                rd.DataTokens.Add("umbraco", "true");

                // Setup dummy request context
                var rc = new RequestContext(
                    new HttpContextWrapper(HttpContext.Current),
                    rd);

                // Get controller factory
                var cf = ControllerBuilder.Current.GetControllerFactory();

                // Try and create the controller
                try
                {
                    var ctrl = cf.CreateController(rc, controllerName);
                    if (ctrl == null)
                        return false;

                    var ctrlInstance = ctrl as SurfaceController;
                    if (ctrlInstance == null)
                        return false;

                    foreach (var method in ctrlInstance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => typeof(ActionResult).IsAssignableFrom(x.ReturnType)))
                    {
                        if (method.Name.InvariantEquals(actionName))
                        {
                            return true;
                        }

                        var attr = method.GetCustomAttribute<ActionNameAttribute>();
                        if (attr != null && attr.Name.InvariantEquals(actionName))
                        {
                            return true;
                        }
                    }

                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool SurfaceControllerExists(string controllerName, string actionName = "Index", bool cacheResult = true)
        {
            if (cacheResult == false)
                return SurfaceControllerExists(controllerName, actionName);

            return (bool)Current.AppCaches.RuntimeCache.GetCacheItem(
                string.Join("_", new[] { "Our.Umbraco.BlockListMvc.Helpers.SurfaceControllerHelper.SurfaceControllerExists", controllerName, actionName }),
                () => SurfaceControllerExists(controllerName, actionName));
        }
    }
}
