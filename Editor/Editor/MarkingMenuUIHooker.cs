using System;
using UnityEditor;
using UnityEngine;

namespace StansAssets.MarkingMenu
{
    /// <summary>
    /// Class-hooker, that performs updating scene GUI.
    /// Creates and fills <see cref="SceneViewMarkingMenu"/> with <see cref="ActionsData"/>.
    /// Processing user interacting with menu
    /// </summary>
    [InitializeOnLoad]
    internal class MarkingMenuUIHooker
    {
        private static SceneViewMarkingMenu s_DefaultMarkingMenu = null;

        private static ActionsData m_ActionData;

        private static bool s_MouseDownFired = false;
        private static float s_MouseDownTime = 0.0f;
        private static Vector2 s_MouseRightClickPosition = Vector2.zero;
        private const float k_MenuShowupDelay = 0.125f;
        private const float k_MouseDragThreshold = 15.0f;

        public static bool Enabled = true;

        public static bool IsMenuVisible
        {
            get
            {
                if (s_DefaultMarkingMenu == null)
                {
                    return false;
                }
                return s_DefaultMarkingMenu.IsOpened;
            }
        }

        public const int MarkingMenuHeight = 20;
        public const int ItemSpread = 50;

        static MarkingMenuUIHooker()
        {
            m_ActionData = ActionsDataHolder.Instance.ActionsData;
            SceneView.duringSceneGui += OnGUI;
        }

        public static MarkingMenu DefaultMarkingMenu
        {
            get
            {
                if (s_DefaultMarkingMenu == null)
                {
                    CreateDefaultMenu();
                }

                return s_DefaultMarkingMenu;
            }
        }

        public void Reset() => s_DefaultMarkingMenu = null;

        public static void CreateDefaultMenu()
        {
            s_DefaultMarkingMenu = new SceneViewMarkingMenu();

            foreach (var data in m_ActionData.Data)
            {
                if (data.Action == null || !typeof(IMarkingMenuButton).IsAssignableFrom(data.Action.GetClass()))
                    continue;

                IMarkingMenuButton action = (IMarkingMenuButton)Activator.CreateInstance(data.Action.GetClass());
                float radAngle = data.Angle * Mathf.Deg2Rad;
                s_DefaultMarkingMenu.AddItem(
                    new ActionBasedMarkingMenuItem(data, action),
                    new Vector2(Mathf.Sin(radAngle) * data.Length, -Mathf.Cos(radAngle) * data.Length)
                );
            }
        }

        public static void OnGUI(SceneView sceneView)
        {
            //Just a quikc fix need to fugire out a better way
            if (EditorInput.RightMouseDown && !Enabled)
            {
                Enabled = true;
            }

            if (EditorInput.AltDown && EditorInput.RightMouseDown)
            {
                Enabled = false;
            }

            if (!Enabled)
            {
                return;
            }

            if (s_MouseDownFired)
            {
                Rect cursorRect = new Rect(0, 0, sceneView.camera.pixelWidth, sceneView.camera.pixelHeight);
                EditorGUIUtility.AddCursorRect(cursorRect, MouseCursor.Arrow);
            }

            Event e = Event.current;
            // note - important to
            if (e.type == EventType.Repaint || e.type == EventType.Layout)
            {

                if (e.type == EventType.Layout)
                {
                    float timeDiff = Time.realtimeSinceStartup - s_MouseDownTime;
                    if (s_MouseDownFired && timeDiff >= k_MenuShowupDelay)
                    {
                        s_MouseDownFired = false;
                        DefaultMarkingMenu.Open(s_MouseRightClickPosition);
                    }
                }

                if (DefaultMarkingMenu.IsOpened)
                {
                    DefaultMarkingMenu.OnGUI(sceneView);
                }
                return;
            }

            if (e.type == EventType.MouseUp)
            {
                s_MouseDownFired = false;

                if (DefaultMarkingMenu.IsOpened)
                {
                    DefaultMarkingMenu.Finalise();
                }

                SceneView.currentDrawingSceneView.Repaint();
            }

            if (!e.isMouse || e.button != 1 || e.control || e.alt)
            {
                return;
            }

            if (EditorInput.RightMouseDown)
            {
                s_MouseRightClickPosition = e.mousePosition;
                s_MouseDownFired = true;
                s_MouseDownTime = Time.realtimeSinceStartup;

                SceneView.currentDrawingSceneView.Repaint();
            }
            else if (e.type == EventType.MouseDrag)
            {
                e.Use();

                Vector2 diff = e.mousePosition - s_MouseRightClickPosition;
                if (s_MouseDownFired && (Mathf.Abs(diff.x) >= k_MouseDragThreshold || Mathf.Abs(diff.y) >= k_MouseDragThreshold))
                {
                    s_MouseDownFired = false;
                    DefaultMarkingMenu.Open(s_MouseRightClickPosition);
                }

                SceneView.currentDrawingSceneView.Repaint();
            }
        }
    }
}