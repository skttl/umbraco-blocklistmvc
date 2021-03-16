using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Our.Umbraco.BlockListMvc.Controllers
{
    public class DefaultBlockListMvcSurfaceController : BlockListMvcSurfaceController
    {
        public ActionResult Index()
        {
            return CurrentPartialView();
        }
    }
}
