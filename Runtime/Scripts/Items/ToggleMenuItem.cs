using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    class ToggleMenuItem: MarkingMenuItem
    {
        public event Action<ItemExecutedEventArgs> OnItemExecuted;
        readonly string m_ActionId;
        string m_CurrentItem;
        string m_LastMouseOverItem;
        bool m_MouseOverItem;
        
        protected const string k_LabelStyleItem = "markingMenuItemAdapter-menu-item";
        protected const string k_LabelStyleItemActive = "markingMenuItemAdapter-menu-item-active";
        
        protected readonly VisualElement m_ToggleMenuItemsContainer;
        protected readonly Dictionary<string, Label> m_ToggleMenuItemsLabel = new Dictionary<string, Label>();

        public ToggleMenuItem(MarkingMenuItemModel model, ToggleMenuContextModel items)
            : base(model.GetHashCode(), model)
        {
            m_ActionId = model.CustomItemId;
            m_CurrentItem = items.CurrentItem;
            
            m_ToggleMenuItemsContainer = VisualElement.Q<VisualElement>("markingMenuItemAdapterMenu");
            m_ToggleMenuItemsContainer.style.display = DisplayStyle.Flex;
            
            foreach (var it in items.List)
            {
                var labelElement = new Label(it);
                labelElement.AddToClassList(k_LabelStyleItem);
                labelElement.RegisterCallback<MouseEnterEvent>(MouseEnterEventItem);
                labelElement.RegisterCallback<MouseLeaveEvent>(MouseLeaveEventItem);
                m_ToggleMenuItemsLabel.Add(it, labelElement);
                m_ToggleMenuItemsContainer.Add(labelElement);
            }

            m_LastMouseOverItem = m_CurrentItem;
            ChangeActiveItem(m_CurrentItem);
        }

        public override void Execute()
        {
            if (m_MouseOverItem)
            {
                ChangeActiveItem(m_LastMouseOverItem);
                OnItemExecuted?.Invoke(new ItemExecutedEventArgs()
                {
                    Id = m_ActionId,
                    Type = Model.Type,
                    Item = this,
                    Value = m_CurrentItem
                });
            }
        }

        void MouseEnterEventItem(MouseEnterEvent ev)
        {
            var el = ev.target as Label;
            if (m_ToggleMenuItemsLabel.ContainsValue(el))
            {
                m_LastMouseOverItem = m_ToggleMenuItemsLabel.FirstOrDefault(x => x.Value == el).Key;
                m_MouseOverItem = true;
            }
                
        }
        void MouseLeaveEventItem(MouseLeaveEvent ev)
        {
            m_MouseOverItem = false;
        }

        void ChangeActiveItem(string itemNewName)
        {
            var oldItem = m_ToggleMenuItemsLabel[m_CurrentItem];
            oldItem.EnableInClassList(k_LabelStyleItemActive, false);
            oldItem.RemoveFromClassList(k_LabelStyleItemActive);
            var newItem = m_ToggleMenuItemsLabel[itemNewName];
            newItem.ClearClassList();
            newItem.AddToClassList(k_LabelStyleItem);
            newItem.AddToClassList(k_LabelStyleItemActive);
            m_CurrentItem = itemNewName;
        }
    }
}
