using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models.Blocks;
using Umbraco.Web.Mvc;

namespace Our.Umbraco.BlockListMvc.Controllers
{
    public abstract class BlockListMvcSurfaceController : BlockListMvcSurfaceController<BlockListItem>
    {
    }

    public abstract class BlockListMvcSurfaceController<TModel> : SurfaceController
    {
        public TModel Model
        {
            get { return (TModel)ControllerContext.RouteData.Values["blockListItem"]; }
        }

        public string ViewPath
        {
            get { return ControllerContext.RouteData.Values["blockListItemViewPath"] as string ?? string.Empty; }
        }

        public string ContentTypeAlias
        {
            get { return ControllerContext.RouteData.Values["blockListItemContentTypeAlias"] as string ?? string.Empty; }
        }

        protected PartialViewResult CurrentPartialView(object model = null)
        {
            if (model == null)
                model = Model;

            var viewName = ContentTypeAlias ?? ControllerContext.RouteData.Values["action"].ToString();

            return PartialView(viewName, model);
        }

        protected new PartialViewResult PartialView(string viewName)
        {
            return PartialView(viewName, Model);
        }

        protected override PartialViewResult PartialView(string viewName, object model)
        {
            if (string.IsNullOrWhiteSpace(ViewPath) == false)
            {
                var viewPath = GetFullViewPath(viewName, ViewPath);
                if (ViewEngines.Engines.FindPartialView(ControllerContext, viewPath).View != null)
                    return base.PartialView(viewPath, model);
            }

            return base.PartialView(viewName, model);
        }

        protected string GetFullViewPath(string viewName, string baseViewPath)
        {
            if (viewName.StartsWith("~") ||
                viewName.StartsWith("/") ||
                viewName.StartsWith(".") ||
                string.IsNullOrWhiteSpace(baseViewPath))
            {
                return viewName;
            }

            return string.Concat(baseViewPath.EnsureEndsWith('/'), viewName, ".cshtml");
        }
    }
}
