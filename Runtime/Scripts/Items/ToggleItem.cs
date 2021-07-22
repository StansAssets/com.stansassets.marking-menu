using System;

namespace StansAssets.MarkingMenu
{
    class ToggleItem: MarkingMenuItem
    {
        public event Action<ItemExecutedEventArgs> OnItemExecuted;
        readonly string m_ActionId;
        bool m_State;
        const string k_LabelPrefixOn = " (ON)";
        const string k_LabelPrefixOff = " (OFF)";
        const string k_LabelStyleOn = "label-on";
        const string k_LabelStyleOff = "label-off";
        const string k_LabelStyleDefault = "markingMenuItemAdapter-label";

        public ToggleItem(MarkingMenuItemModel model, bool state)
            : base(model.GetHashCode(), model)
        {
            m_ActionId = model.CustomItemId;
            m_State = state;
            UpdateUIByState(m_State);
        }
        
        public override void Execute()
        {
            m_State = !m_State;
            UpdateUIByState(m_State);
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
            UpdateUIByState(m_State);
        }

        void UpdateUIByState(bool state)
        {
            m_MenuItemLabel.ClearClassList();
            m_MenuItemLabel.AddToClassList(k_LabelStyleDefault);
            var style = "";
            var prefix = "";
            if (state)
            {
                style = k_LabelStyleOn;
                prefix = k_LabelPrefixOn;
            }
            else
            {
                style = k_LabelStyleOff;
                prefix = k_LabelPrefixOff;
            }
            m_MenuItemLabel.AddToClassList(style);
            m_MenuItemLabel.text = Model.DisplayName + prefix;
        }
    }
}
