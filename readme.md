# Our.Umbraco.BlockListMvc 
WIP port of the [hijacking feature from Doc Type Grid Editor](https://github.com/skttl/umbraco-doc-type-grid-editor/blob/develop/docs/developers-guide.md#doctypegrideditorsurfacecontroller), to be used when rendering Block Lists in Umbraco.

Still very much work in progress, but to try it out, you render a block list like this:
```cshtml
@using Our.Umbraco.BlockListMvc.Helpers
@foreach (var block in Model.Blocks)
{
    @Html.BlockListItem(block, Model.Blocks)
}
```

By default, it will look in `Views/Partials/Blocks/{contentTypeAlias}.cshtml` for views, but this can be customised.

The hijacking part works just like in Dtge with the differiences being, that you have to inherit from `BlockListMvcSurfaceController` and the `Model` is `BlockListItem`. Example:
```cs
    public class FeatureSurfaceController : BlockListMvcSurfaceController
    {
        public ActionResult Feature()
        {
            return CurrentPartialView();
        }
    }
```

It also adds a new [ViewPage called `BlockListItemViewPage`](https://github.com/skttl/umbraco-blocklistmvc/blob/master/src/Our.Umbraco.BlockListMvc/Mvc/BlockListItemViewPage.cs) which inherits `UmbracoViewPage` and adds a BlockContext property, from which you can get [contextual information](https://github.com/skttl/umbraco-blocklistmvc/blob/master/src/Our.Umbraco.BlockListMvc/Models/BlockListItemContext.cs) like the list the block belongs too, and whether the block is the first/last in the list etc.


