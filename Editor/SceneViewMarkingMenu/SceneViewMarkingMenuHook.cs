using UnityEditor;
using UnityEngine;

namespace StansAssets.MarkingMenuB
{
    [InitializeOnLoad]
    static class SceneViewMarkingMenuHook
    {
        static MarkingMenuModel s_CurrentModel;
        static IMarkingMenu s_MarkingMenu;
        static MouseDownContext s_MouseDownContext;

        static SceneViewMarkingMenuHook()
        {
            Refresh();
            SceneView.duringSceneGui += SceneViewOnduringSceneGui;
        }

        [MenuItem("Stans Assets/Marking Menu/Refresh")]
        static void Refresh()
        {
            var model = Resources.Load("MarkingMenuModel") as MarkingMenuModel;
            s_MarkingMenu = MarkingMenuService.CreateMenu();
            s_MarkingMenu.Init(model);
        }

        [MenuItem("Stans Assets/Marking Menu/Open")]
        static void Open()
        {
            SceneView.duringSceneGui += OpenMarkingMenuHook;
        }

        [MenuItem("Stans Assets/Marking Menu/Close")]
        static void Close()
        {
            s_MarkingMenu.Close();
        }

        [MenuItem("Stans Assets/Marking Menu/Toggle Debug Mode")]
        static void ToggleDebug()
        {
            s_MarkingMenu.DebugMode = !s_MarkingMenu.DebugMode;
        }

        static void OpenMarkingMenuHook(SceneView sceneView)
        {
            SceneView.duringSceneGui -= OpenMarkingMenuHook;

            Rect localSceneViewRect = sceneView.position;
            localSceneViewRect.position = Vector2.zero;

            s_MarkingMenu.Open(localSceneViewRect.center);
        }

        static void SceneViewOnduringSceneGui(SceneView obj)
        {
            if (s_MarkingMenu != null)
            {
                HandleInput();
            }
        }

        static void HandleInput()
        {
            Event e = Event.current;

            // Open Marking Menu
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        s_MouseDownContext = new MouseDownContext(true, e.mousePosition);
                    }
                    break;

                // case EventType.MouseUp:
                //     s_MouseDownContext = new MouseDownContext(false, Vector2.zero);
                //     if (s_MarkingMenu.Active)
                //     {
                //         s_MarkingMenu.Close();
                //     }
                //     break;

                case EventType.MouseDrag:
                    e.Use();

                    if (s_MarkingMenu.Active == false && s_MouseDownContext.IsMouseDown)
                    {
                        if ((s_MouseDownContext.Position - e.mousePosition).sqrMagnitude > 25)
                        {
                            s_MarkingMenu.Open(s_MouseDownContext.Position);
                        }
                    }
                    break;
            }

            if (s_MarkingMenu.Active)
            {
                var sceneView = SceneView.lastActiveSceneView;
                Rect cursorRect = new Rect(0, 0, sceneView.camera.pixelWidth, sceneView.camera.pixelHeight);
                EditorGUIUtility.AddCursorRect(cursorRect, UnityEditor.MouseCursor.Arrow);
            }

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);
        }

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
    }
}
