using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models.Blocks;
using Umbraco.Core.Models.PublishedContent;

namespace Our.Umbraco.BlockListMvc.Models
{
    public class BlockListItemWithContext : BlockListItem
    {
        public BlockListItemWithContext(BlockListItem item, BlockListModel list) : base(item.ContentUdi, item.Content, item.SettingsUdi, item.Settings)
        {
            Item = item;
            List = list;
        }

        private BlockListItem Item;
        public BlockListModel List;

        private int? _index;
        public int Index => (int)(_index ?? (_index = List != null && Item != null ? List.IndexOf(Item) : -1));

        private bool? _isFirst;
        public bool IsFirst => (bool)(_isFirst ?? (_isFirst = List != null && Item != null && List.FirstOrDefault().Equals(Item)));

        private bool? _isLast;
        public bool IsLast => (bool)(_isLast ?? (_isLast = List != null && Item != null && List.LastOrDefault().Equals(Item)));

        private BlockListItem _previousBlock;
        public BlockListItem PreviousBlock => _previousBlock ?? (_previousBlock = List != null && Index > 0 ? List[Index - 1] : null);

        private BlockListItem _nextBlock;
        public BlockListItem NextBlock => _nextBlock ?? (_nextBlock = List != null && !IsLast ? List[Index + 1] : null);
    }

    public class BlockListItemWithContext<T> : BlockListItemWithContext
        where T : IPublishedElement
    {
        public BlockListItemWithContext(BlockListItem item, BlockListModel list) : base(item, list)
        {

        }
        public new T Content { get; }
    }

    public class BlockListItemWithContext<TContent, TSettings> : BlockListItemWithContext
        where TContent : IPublishedElement
        where TSettings : IPublishedElement
    {
        public BlockListItemWithContext(BlockListItem item, BlockListModel list) : base(item, list)
        {

        }
        public new TContent Content { get; }
        public new TSettings Settings { get; }
    }
}
