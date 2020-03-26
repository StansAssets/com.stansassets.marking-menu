using System;

namespace StansAssets.MarkingMenuB
{
    struct ItemClickedEventArgs
    {
        internal string Id;
        internal ItemType Type;
    }

    class ActionItem : MarkingMenuItem
    {
        public Action<ItemClickedEventArgs> OnItemClicked;
        readonly string m_ActionId;

        public ActionItem(MarkingMenuItemModel model)
            : base(model.GetHashCode(), model)
        {
            m_ActionId = model.CustomItemId;
        }

        public void Execute()
        {
            OnItemClicked?.Invoke(new ItemClickedEventArgs()
            {
                Id = m_ActionId,
                Type = Model.Type,
            });
        }
    }
}
