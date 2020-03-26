
using UnityEditor;
using UnityEngine;

namespace StansAssets.MarkingMenuB
{
    public class MarkingMenuSceneViewController
    {
        struct MouseDownContext
        {
            public bool IsMouseDown;
            public Vector2 Position;
            public MouseDownContext(bool isMouseDown, Vector2 position)
            {
                IsMouseDown = isMouseDown;
                Position = position;
            }
        }

        IMarkingMenu m_MarkingMenu;

        MouseDownContext m_MouseDownContext;

        public void Init(IMarkingMenu menu)
        {
            m_MarkingMenu = menu;
        }

        public void HandleInput()
        {
            Event e = Event.current;

            // Open Marking Menu
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        m_MouseDownContext = new MouseDownContext(true, e.mousePosition);
                    }
                    break;

                case EventType.MouseUp:
                    m_MouseDownContext = new MouseDownContext(false, Vector2.zero);
                    if (m_MarkingMenu.Active)
                    {
                        m_MarkingMenu.Close();
                    }
                    break;

                case EventType.MouseDrag:
                    e.Use();

                    if (m_MarkingMenu.Active == false && m_MouseDownContext.IsMouseDown)
                    {
                        if ((m_MouseDownContext.Position - e.mousePosition).sqrMagnitude > 25)
                        {
                            m_MarkingMenu.Open(m_MouseDownContext.Position);
                        }
                    }
                    break;
            }

            if (m_MarkingMenu.Active)
            {
                var sceneView = SceneView.lastActiveSceneView;
                Rect cursorRect = new Rect(0, 0, sceneView.camera.pixelWidth, sceneView.camera.pixelHeight);
                EditorGUIUtility.AddCursorRect(cursorRect, UnityEditor.MouseCursor.Arrow);
            }

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);
        }
    }
}
