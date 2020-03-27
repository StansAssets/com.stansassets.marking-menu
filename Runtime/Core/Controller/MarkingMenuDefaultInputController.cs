using System.Collections.Generic;

namespace StansAssets.MarkingMenuB
{
    class MarkingMenuDefaultInputController : IMarkingMenuInputController
    {
        List<IMarkingMenuItem> m_RegisteredItems = new List<IMarkingMenuItem>();

        public void Init(IMarkingMenuInternal menu)
        {
            Reset();

            // Register items callbacks
            for (var i = 0; i < menu.Items.Count; ++i)
            {
                if (RegisterItem(menu.Items[i]))
                {
                    m_RegisteredItems.Add(menu.Items[i]);
                }
            }
        }

        public void Reset()
        {
            // Unregister items callbacks
            for (var i = 0; i < m_RegisteredItems.Count; ++i)
            {
                UnregisterItem(m_RegisteredItems[i]);
            }
            m_RegisteredItems.Clear();
        }

        public void HandleInput()
        {

        }

        bool RegisterItem(IMarkingMenuItem item)
        {
            if (item is ActionItem actionItem)
            {
                actionItem.OnItemExecuted += OnItemClicked;
                return true;
            }

            return false;
        }

        void UnregisterItem(IMarkingMenuItem item)
        {
            if (item is ActionItem actionItem)
            {
                actionItem.OnItemExecuted += OnItemClicked;
                m_RegisteredItems.Add(item);
            }
        }

        void OnItemClicked(ItemExecutedEventArgs args)
        {

        }
    }
}
