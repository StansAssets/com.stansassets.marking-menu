using System.Collections.Generic;

namespace StansAssets.MarkingMenu
{
    public struct ToggleMenuContextModel
    {
        internal IEnumerable<string> List;
        internal string CurrentItem;
        public ToggleMenuContextModel(IEnumerable<string> list, string currentItem)
        {
            List = list;
            CurrentItem = currentItem;
        }
    }
}
