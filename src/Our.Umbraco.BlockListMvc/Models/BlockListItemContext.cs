using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models.Blocks;

namespace Our.Umbraco.BlockListMvc.Models
{
    public class BlockListItemContext
    {
        public BlockListItemContext(BlockListItem item, BlockListModel list)
        {
            Item = item;
            List = list;
        }

        public BlockListItem Item;
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
}
