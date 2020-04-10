using System;
using UnityEngine;

namespace StansAssets.MarkingMenu
{
    struct ItemExecutedEventArgs
    {
        internal string Id;
        internal ItemType Type;
        internal MarkingMenuItem Item;
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

        public override void Execute()
        {
            OnItemExecuted?.Invoke(new ItemExecutedEventArgs()
            {
                Id = m_ActionId,
                Type = Model.Type,
                Item = this,
            });
        }
    }
}
