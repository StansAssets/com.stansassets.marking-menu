using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace StansAssets.MarkingMenu
{
    internal abstract class MarkingMenu : ISceneViewEditorTool
    {
        public static readonly LazyInitialization<Texture2D> LineSegment = new LazyInitialization<Texture2D>(LoadLineTexture);
        public static readonly LazyInitialization<Texture2D> CenterCircle = new LazyInitialization<Texture2D>(LoadCenterCircle);

        protected Dictionary<int, IMarkingMenuItem> m_MenuItems = new Dictionary<int, IMarkingMenuItem>();
        protected Dictionary<int, Rect> m_MenuItemsRects = new Dictionary<int, Rect>();
        protected Dictionary<int, Vector2> m_MenuPositions = new Dictionary<int, Vector2>();

        private int m_Slot = 0;
        private bool m_IsOpened = false;
        private Vector2 m_MenuPosition;
        private int m_CurrentlyHighlightedSlot = -1;
        private IMarkingMenuItem m_CurrentlyHighlightedItem;

        protected const float k_MenuItemShift = 100.0f;
        protected const int k_MaxMenuItemsCount = 8;

        public MarkingMenu() { }

        private static Texture2D LoadLineTexture() => new ResourceBasedTexture("markingMenuSelectionLine").Texture;

        private static Texture2D LoadCenterCircle() => new ResourceBasedTexture("markingMenuCenterCircle").Texture;

        public void Open(Vector2 position)
        {
            m_IsOpened = true;
            m_MenuPosition = position;
            m_MenuItemsRects.Clear();
        }

        public void Close()
        {
            m_IsOpened = false;
        }

        public void Finalise()
        {
            Close();
            if (m_CurrentlyHighlightedItem != null)
            {
                MarkingMenuEvent menuEvent = new MarkingMenuEvent();
                menuEvent.E = Event.current;
                menuEvent.Highlighted = true;
                menuEvent.Selected = true;
                menuEvent.Position = m_MenuPositions[m_CurrentlyHighlightedSlot];

                m_CurrentlyHighlightedItem.OnGUI(menuEvent);
            }
        }

        public void OnGUI(SceneView view)
        {
            if (!m_IsOpened) return;

            Event e = Event.current;
            float relativeMouseRotationAngle = Vector2.Angle(Vector2.right, e.mousePosition - m_MenuPosition);
            if ((e.mousePosition - m_MenuPosition).y < 0)
            {
                relativeMouseRotationAngle *= -1;
            }

            // Draw the center of the marking menu.
            Handles.BeginGUI();
            Rect cursorRect = new Rect(0, 0, view.camera.pixelWidth, view.camera.pixelHeight);
            EditorGUIUtility.AddCursorRect(cursorRect, MouseCursor.Arrow);

            Vector2 centerPointTextureSize = new Vector2(32, 32);
            Rect textureRectangle = new Rect(m_MenuPosition.x - centerPointTextureSize.x / 2f,
                m_MenuPosition.y - centerPointTextureSize.y / 2f,
                centerPointTextureSize.x,
                centerPointTextureSize.y);

            GUI.DrawTexture(textureRectangle, CenterCircle.Value, ScaleMode.ScaleToFit, true);

            m_CurrentlyHighlightedItem = GetClosestMarkingMenuItem(m_MenuPosition, e.mousePosition, relativeMouseRotationAngle);

            Vector2 pos = Vector2.one;
            m_CurrentlyHighlightedSlot = -1;
            foreach (KeyValuePair<int, IMarkingMenuItem> menuItem in m_MenuItems)
            {
                pos = new Vector2(m_MenuPositions[menuItem.Key].x + m_MenuPosition.x, m_MenuPositions[menuItem.Key].y + m_MenuPosition.y);
                if (menuItem.Value == m_CurrentlyHighlightedItem)
                {
                    m_CurrentlyHighlightedSlot = menuItem.Key;
                }

                //Create the event for menu item
                MarkingMenuEvent menuEvent = new MarkingMenuEvent();
                menuEvent.E = Event.current;
                menuEvent.Highlighted = menuItem.Value == m_CurrentlyHighlightedItem;
                menuEvent.Selected = false;
                menuEvent.Position = pos;

                m_MenuItemsRects[menuItem.Key] = menuItem.Value.OnGUI(menuEvent);
            }

            Vector2 lineSegmentSize = new Vector2(Vector2.Distance(m_MenuPosition, e.mousePosition), 24);
            Rect lineSegmentRectangle = new Rect(0.0f, -lineSegmentSize.y / 2,
                lineSegmentSize.x,
                lineSegmentSize.y);

            GUI.matrix = Matrix4x4.TRS(new Vector3(m_MenuPosition.x, m_MenuPosition.y, 0.0f), Quaternion.Euler(0.0f, 0.0f, relativeMouseRotationAngle), Vector3.one);
            GUI.DrawTexture(lineSegmentRectangle, LineSegment.Value);
            GUI.matrix = Matrix4x4.identity;

            Handles.EndGUI();
        }

        public void AddItem(IMarkingMenuItem item, Vector2 relativePosition)
        {
            if (m_MenuItems.Count < k_MaxMenuItemsCount)
            {
                m_MenuItems.Add(m_Slot, item);
                m_MenuPositions.Add(m_Slot, relativePosition);
                m_Slot++;
            }
        }

        public void SetItemAt(int slot, IMarkingMenuItem item, Vector2 relativePosition)
        {
            if (slot < k_MaxMenuItemsCount - 1)
            {
                m_MenuItems[slot] = item;
                m_MenuPositions[slot] = relativePosition;
            }
        }

        public virtual IMarkingMenuItem GetClosestMarkingMenuItem(Vector2 menuPosition, Vector2 currentMousePos, float relativeMouseAngle)
        {
            foreach (KeyValuePair<int, Rect> itemRect in m_MenuItemsRects)
            {
                if (itemRect.Value.Contains(currentMousePos))
                {
                    return m_MenuItems[itemRect.Key];
                }
            }

            return null;
        }

        public bool IsOpened
        {
            get => m_IsOpened;
            set => m_IsOpened = value;
        }

        public IMarkingMenuItem HighlightedItem => m_CurrentlyHighlightedItem;
    }
}
