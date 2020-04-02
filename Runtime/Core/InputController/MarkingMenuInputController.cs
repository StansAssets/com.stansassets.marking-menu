using StansAssets.MarkingMenu;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    interface IMarkingMenuInputController
    {
        void HandleEvent(PointerUpEvent e);
    }

    class MarkingMenuInputController : IMarkingMenuInputController
    {
        MarkingMenu m_MarkingMenu;

        public MarkingMenuInputController(MarkingMenu menu)
        {
            m_MarkingMenu = menu;
        }

        public void HandleEvent(PointerUpEvent e)
        {
            for (var i = 0; i < m_MarkingMenu.Items.Count; ++i)
            {
                if (HandleItemInput(m_MarkingMenu.Items[i], e))
                {
                    break;
                }
            }

            m_MarkingMenu.Close();
        }

        bool HandleItemInput(IMarkingMenuItem item, PointerUpEvent e)
        {
            if (item.MouseOver)
            {
                item.Execute();
                return true;
            }

            return false;
        }
    }
}
