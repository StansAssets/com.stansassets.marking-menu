using StansAssets.Foundation.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    [InitializeOnLoad]
    static class SceneViewMarkingMenuHook
    {
        static MarkingMenuModel s_CurrentModel;
        static MarkingMenu s_MarkingMenu;
        static MouseDownContext s_MouseDownContext;

        static SceneViewMarkingMenuHook()
        {
            Refresh();
            SceneView.duringSceneGui += SceneViewOnDuringSceneGui;
        }

        [MenuItem("Stans Assets/Marking Menu/Refresh")]
        static void Refresh()
        {
            s_MarkingMenu?.Close();

            var model = Resources.Load("MarkingMenuModel") as MarkingMenuModel;
            s_MarkingMenu = new MarkingMenu();
            // Prevent default event handle
            s_MarkingMenu.Root.RegisterCallback<MouseUpEvent>((args) =>
            {
                args.PreventDefault();
            }, TrickleDown.TrickleDown);

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
            MarkingMenu.DebugMode = !MarkingMenu.DebugMode;
        }

        static void OpenMarkingMenuHook(SceneView sceneView)
        {
            SceneView.duringSceneGui -= OpenMarkingMenuHook;

            Rect localSceneViewRect = sceneView.position;
            localSceneViewRect.position = Vector2.zero;

            s_MarkingMenu.Open(sceneView.rootVisualElement, localSceneViewRect.center);
        }

        static void SceneViewOnDuringSceneGui(SceneView sceneView)
        {
            if (s_MarkingMenu != null)
            {
                // Set mouse position manually for SceneView because Unity Editor steals events
                if (Event.current.type == EventType.Repaint)
                {
                    s_MarkingMenu.SetMousePosition(Event.current.mousePosition);
                }

                HandleInput(sceneView);
            }
        }

        static void HandleInput(SceneView sceneView)
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
                    else
                    {
                        s_MouseDownContext = new MouseDownContext(false, e.mousePosition);
                    }
                    break;

                case EventType.MouseUp:
                    var visualElementEvent = UIElementsUtility.CreateEvent(e);
                    s_MarkingMenu.SendEvent(visualElementEvent);
                    break;

                case EventType.MouseDrag:
                    e.Use();

                    if (s_MarkingMenu.Active == false)
                    {
                        if (s_MouseDownContext.IsMouseDown && (s_MouseDownContext.Position - e.mousePosition).sqrMagnitude > 25)
                        {
                            s_MarkingMenu.Open(sceneView.rootVisualElement, s_MouseDownContext.Position);
                        }
                    }
                    break;
            }

            if (s_MarkingMenu.Active)
            {
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
