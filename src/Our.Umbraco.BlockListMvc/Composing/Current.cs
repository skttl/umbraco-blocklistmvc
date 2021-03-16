using Our.Umbraco.BlockListMvc.Controllers;
using System;
using Umbraco.Core;

namespace Our.Umbraco.BlockListMvc.Composing
{
    public class Current
    {
        private static Type _defaultBlockListMvcControllerType;

        // internal - can only be accessed through Composition at compose time
        internal static Type DefaultBlockListMvcControllerType
        {
            get => _defaultBlockListMvcControllerType;
            set
            {
                if (value.IsOfGenericType(typeof(BlockListMvcSurfaceController<>)) == false)
                    throw new InvalidOperationException($"The Type specified ({value}) is not of type {typeof(BlockListMvcSurfaceController<>)}");
                _defaultBlockListMvcControllerType = value;
            }
        }
    }
}
