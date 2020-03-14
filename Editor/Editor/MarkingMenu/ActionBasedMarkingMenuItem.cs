using UnityEngine;
using UnityEditor;

namespace StansAssets.MarkingMenu
{
    /// <summary>
    /// Menu item that is being drawed on GUI.
    /// Performs action on click on it.
    /// </summary>
    internal class ActionBasedMarkingMenuItem : IMarkingMenuItem
    {
        protected ActionsData.ActionData m_Data;
        protected IMarkingMenuButton m_Action;
        protected bool m_IsTogglable;

        protected static Texture2D s_SelectionTexture;

        protected static Texture2D GetSelectionTexture()
        {
            if (s_SelectionTexture == null)
            {
                Color c = GUI.skin.settings.selectionColor;
                c.a = 0.9f;
                s_SelectionTexture = CreateTexture(c);
            }
            return s_SelectionTexture;
        }

        protected static Texture2D CreateTexture(Color c)
        {
            var texture = new Texture2D(2, 2);
            texture.SetPixels(new[] { c, c, c, c });
            texture.Apply();
            texture.hideFlags = HideFlags.HideAndDontSave;
            return texture;
        }

        public ActionBasedMarkingMenuItem(ActionsData.ActionData data, IMarkingMenuButton action)
        {
            m_Data = data;
            m_Action = action;
            m_IsTogglable = action is IMarkingMenuToggle;
        }

        public virtual Rect OnGUI(MarkingMenuEvent e)
        {
            float width = 130, height = 20;
            Rect area = new Rect(e.Position.x - width / 2, e.Position.y - height / 2, width, height);

            GUIStyle style = EditorStyles.toolbarButton;
            Texture2D originalBackground = style.normal.background;
            if (e.Highlighted)
            {
                style.normal.background = GetSelectionTexture();
            }

            bool active;
            string label;

            if (m_IsTogglable)
            {
                IMarkingMenuToggle toggle = m_Action as IMarkingMenuToggle;
                label = m_Data.Label + " (" + toggle.IsToggle + ")";
                active = toggle.IsToggle;
            }
            else
            {
                label = m_Data.Label;
                active = false;
            }

            GUI.Toggle(area, active, label, EditorStyles.toolbarButton);

            style.normal.background = originalBackground;

            if (e.Selected && m_Action != null)
            {
                m_Action.PerformAction();
            }

            return area;
        }

        public string Label => m_Data.Label;
    }
}
