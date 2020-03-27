using System;

namespace StansAssets.MarkingMenuB
{
    struct ItemExecutedEventArgs
    {
        internal string Id;
        internal ItemType Type;
    }

    class ActionItem : MarkingMenuItem
    {
        public event Action<ItemExecutedEventArgs> OnItemExecuted;
        readonly string m_ActionId;

        public ActionItem(MarkingMenuItemModel model)
            : base(model.GetHashCode(), model)
        {
            m_ActionId = model.CustomItemId;
        }

        public void Execute()
        {
            OnItemExecuted?.Invoke(new ItemExecutedEventArgs()
            {
                Id = m_ActionId,
                Type = Model.Type,
            });
        }
    }
}
