using Our.Umbraco.BlockListMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.Mvc;

namespace Our.Umbraco.BlockListMvc.Mvc
{
    public abstract class BlockListItemViewPage<T> : UmbracoViewPage<T>
    {
        private BlockListItemContext _blockContext;
        public BlockListItemContext BlockContext => _blockContext ?? (_blockContext = ViewData[Constants.BlockListItemContextViewDataKey] as BlockListItemContext);

        protected override void InitializePage()
        {
            base.InitializePage();
        }
    }
}
