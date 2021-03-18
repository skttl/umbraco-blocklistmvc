using Our.Umbraco.BlockListMvc.Controllers;
using Our.Umbraco.BlockListMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Umbraco.Core;
using Umbraco.Core.Models.Blocks;
using BlockListCurrent = Our.Umbraco.BlockListMvc.Composing.Current;

namespace Our.Umbraco.BlockListMvc.Helpers
{
    public static class RenderHelper
    {
        /// <summary>
        /// Renders the specified item and adds the specified list to the ViewDataDictionary for context.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="item">Block list item to be rendered</param>
        /// <param name="list">The block list the item is a part of</param>
        /// <param name="viewPath">Path to where the block list items partial view is located</param>
        /// <returns></returns>
        public static HtmlString BlockListItem(this HtmlHelper helper, BlockListItem item, BlockListModel list = null, string viewPath = "/Views/Partials/Blocks/", ViewDataDictionary viewData = null)
        {
            var rendering = GetBlockListItemData(item, list, viewPath, viewData);
            if (rendering == null)
                return new HtmlString(string.Empty);

            return helper.Action(rendering.ActionName, rendering.ControllerName, rendering.RouteValues);
        }

        /// <summary>
        /// Renders the specified item and adds the specified list to the ViewDataDictionary for context.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="item">Block list item to be rendered</param>
        /// <param name="list">The block list the item is a part of</param>
        /// <param name="viewPath">Path to where the block list items partial view is located</param>
        /// <returns></returns>
        public static void RenderBlockListItem(this HtmlHelper helper, BlockListItem item, BlockListModel list = null, string viewPath = "/Views/Partials/Blocks/", ViewDataDictionary viewData = null)
        {
            var rendering = GetBlockListItemData(item, list, viewPath, viewData);
            if (rendering != null)
                helper.RenderAction(rendering.ActionName, rendering.ControllerName, rendering.RouteValues);
        }

        internal static BlockListItemRendering GetBlockListItemData(BlockListItem item, BlockListModel list, string viewPath, ViewDataDictionary viewData)
        {

            if (item == null)
                return null;

            var contentTypeAlias = item.Content?.ContentType.Alias;

            if (contentTypeAlias.IsNullOrWhiteSpace())
            {
                return null;
            }

            var rendering = new BlockListItemRendering();

            var controllerName = $"{contentTypeAlias}Surface";

            if (!viewPath.IsNullOrWhiteSpace())
                viewPath = viewPath.EnsureEndsWith("/");

            rendering.RouteValues = new
            {
                blockListItem = item,
                blockListItemContext = new BlockListItemContext(item, list),
                blockListItemContentTypeAlias = contentTypeAlias,
                blockListItemViewPath = viewPath
            };

            if (SurfaceControllerHelper.SurfaceControllerExists(controllerName, contentTypeAlias, cacheResult: true))
            {
                rendering.ControllerName = controllerName;
                rendering.ActionName = contentTypeAlias;
            }
            else
            {
                //// See if a default surface controller has been registered
                var defaultController = BlockListCurrent.DefaultBlockListMvcControllerType;
                if (defaultController != null)
                {
                    var defaultControllerName = defaultController.Name.Substring(0, defaultController.Name.LastIndexOf("Controller"));
                    rendering.ControllerName = defaultControllerName;

                    // Try looking for an action named after the content type alias
                    if (string.IsNullOrWhiteSpace(contentTypeAlias) == false && SurfaceControllerHelper.SurfaceControllerExists(defaultControllerName, contentTypeAlias, true))
                    {
                        rendering.ActionName = contentTypeAlias;
                    }
                    // else, just go with a default action name
                    else
                    {
                        rendering.ActionName = "Index";
                    }
                }
                else
                {
                    // fall back to using the default default controller
                    rendering.ControllerName = "DefaultBlockListMvcSurface";
                    rendering.ActionName = "Index";
                }
            }

            return rendering;
        }
    }
}
