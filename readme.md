# Our.Umbraco.BlockListMvc 
If you are not the type of developer that likes to put business logic in your views, then the ability to have a controller for you partial view is a must. Block List MVC gives you the ability, to create Surface Controllers, to use when rendering items from a Block List in Umbraco.


---

## Getting started

### Installation
> *Note:* Full Text Search has been developed against **Umbraco v8.1.0** and will support that version and above.

Full Text Search can be installed from either Our Umbraco package repository, or NuGet. See developers guide for more information.

#### Our Umbraco package repository

To install from Our Umbraco, please download the package from:

> [https://our.umbraco.com/packages/website-utilities/full-text-search-8/](https://our.umbraco.com/packages/website-utilities/full-text-search-8/)

#### NuGet package repository

To [install from NuGet](https://www.nuget.org/packages/Our.Umbraco.BlockListMvc), you can run the following command from within Visual Studio:

	PM> Install-Package Our.Umbraco.BlockListMvc

---

## Developers Guide

#### BlockListMvcSurfaceController

**BlockListMvc** comes with a base surface controller you can used called `BlockListMvcSurfaceController`. This is the heart of Block List MVC, and the controller that your block list item gets routed through when rendering.

Simply create your controller inheriting from the above class, giving it a class name of `{DocTypeAlias}SurfaceController` and an action name of `{DocTypeAlias}` and then **BlockListMvc** will automatically wire it up for you and use it at render time.

```csharp
public class TestDocTypeSurfaceController
	: BlockListMvcSurfaceController
{
	public ActionResult TestDocType()
	{
		// Do your thing...
		return CurrentPartialView();
	}
}
```

By inheriting from the `BlockListMvcSurfaceController` base class, you'll also have instant access to the following helper properties / methods.

| Member                                            | Type                     | Description |
|---------------------------------------------------|--------------------------|-------------|
| Model                                             | BlockListItem            | The `BlockListItem` instance of your block's data. |
| BlockContext                                      | BlockListItemContext     | The context of the block list item, including the list that owns it. |
| ViewPath                                          | String                   | A reference to the currently configured ViewPath |
| CurrentPartialView(object model = null)           | Method                   | Helper method to return you to the default partial view for this cell. If no model is passed in, the standard Model will be passed down. |
| PartialView(string viewName, object model = null) | Method                   | Helper method to return you to an alternative partial view for this cell. If no model is passed in, the standard Model will be passed down. |

---

#### Rendering using BlockListMvc
To render a `BlockListItem` using BlockListMvc, in stead of doing the usual `@Html.Partial(...)` you need to use the provided helper methods in `Our.Umbraco.BlockListMvc.Helpers`

##### [@Html.BlockListItem(BlockListItem item)](https://github.com/skttl/umbraco-blocklistmvc/blob/master/src/Our.Umbraco.BlockListMvc/Helpers/RenderHelper.cs#L27)
This one returns an IHtmlString of the rendering. You can also add the list model (BlockListModel), so you can read the context of the block list item (get the index, IsLast, next/previous block etc.), and specify the viewpath with this.

Example:

```cshtml
@using Our.Umbraco.BlockListMvc.Helpers
@foreach (var block in Model.Blocks)
{
    @Html.BlockListItem(block, Model.Blocks)
}
```

##### [Html.RenderBlockListItem(BlockListItem item)](https://github.com/skttl/umbraco-blocklistmvc/blob/master/src/Our.Umbraco.BlockListMvc/Helpers/RenderHelper.cs#L44)
The same as the above, this one just doesn't return the HtmlString but adds the rendering to the output.

Example:

```cshtml
@using Our.Umbraco.BlockListMvc.Helpers
@foreach (var block in Model.Blocks)
{
    Html.RenderBlockListItem(block, Model.Blocks)
}
```

---

#### Individual block views
By default, BlockListMvc will look for your block item views in Views/Partials/Blocks/{contentTypeAlias}.cshtml, but you can configure the path when rendering with BlockListItem/RenderBlockListItem.

The block view is then created like you are used to.

BlockListMvc comes with its own [ViewPage called `BlockListItemViewPage`](https://github.com/skttl/umbraco-blocklistmvc/blob/master/src/Our.Umbraco.BlockListMvc/Mvc/BlockListItemViewPage.cs) which inherits `UmbracoViewPage` and adds a BlockContext property, from which you can get [contextual information](https://github.com/skttl/umbraco-blocklistmvc/blob/master/src/Our.Umbraco.BlockListMvc/Models/BlockListItemContext.cs) like the list the block belongs too, and whether the block is the first/last in the list etc.

Example:

```cshtml
@using Umbraco.Core.Models.Blocks
@using Our.Umbraco.BlockListMvc.Mvc
@inherits BlockListItemViewPage<BlockListItem<Feature>>

<div class="@(BlockContext.IsFirst == false ? "mt-4" : "")">
    <h2>@Model.Content.FeatureName</h2>
    <p>@Model.Content.FeatureDetails</p>
</div>
```

---

#### Default BlockListMvcSurfaceController
**BlockListMvc** comes with a default BlockListMvcSurfaceController, which controls all block items that doesn't have their own SurfaceController. If you wish to override this, you can do it using a Composer, and the extension method `SetDefaultBlockListMvcController` from the `Our.Umbraco.BlockListMvc.Composing` namespace.

Example:
```cs
using System.Web.Mvc;
using Umbraco.Core.Composing;
using Our.Umbraco.BlockListMvc.Composing;
using Our.Umbraco.BlockListMvc.Controllers;

namespace Our.Umbraco.BlockListMvc.Site
{
    public class MyDefaultBlockListMvcSurfaceController : BlockListMvcSurfaceController
    {
        public ActionResult Index()
        {
            /* TODO: Amass unicorns */
            return CurrentPartialView();
        }
    }

    public class DefaultBlockListMvcSurfaceControllerComponent : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.SetDefaultBlockListMvcController<MyDefaultBlockListMvcSurfaceController>();
        }
    }
}
```