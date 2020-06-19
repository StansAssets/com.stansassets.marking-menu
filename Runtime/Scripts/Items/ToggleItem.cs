using System;
using UnityEngine;
using UnityEngine.UIElements;
using StansAssets.Foundation.UIElements;

namespace StansAssets.MarkingMenu
{
    class ToggleItem: MarkingMenuItem
    {
        public event Action<ItemExecutedEventArgs> OnItemExecuted;
        readonly string m_ActionId;
        bool m_State;
        const string k_On = " (ON)";
        const string k_Off = " (OFF)";

        public ToggleItem(MarkingMenuItemModel model, bool state)
            : base(model.GetHashCode(), model)
        {
            m_ActionId = model.CustomItemId;
            m_State = state;
            StatusChangeVisualization(m_State);
        }
        
        public override void Execute()
        {
            m_State = !m_State;
            StatusChangeVisualization(m_State);
            OnItemExecuted?.Invoke(new ItemExecutedEventArgs()
            {
                Id = m_ActionId,
                Type = Model.Type,
                Item = this,
            });
        }

        public override void UpdateDataFromModel()
        {
            base.UpdateDataFromModel();
            StatusChangeVisualization(m_State);
        }

        void StatusChangeVisualization(bool state)
        {
            m_VisualElementName.ClearClassList();
            m_VisualElementName.AddToClassList("markingMenuItemAdapter-label");
            var style = "";
            var prefix = "";
            if (state)
            {
                style = "label-on";
                prefix = k_On;
            }
            else
            {
                style = "label-off";
                prefix = k_Off;
            }
            m_VisualElementName.AddToClassList(style);
            m_VisualElementName.text = Model.DisplayName + prefix;
        }
    }
}
