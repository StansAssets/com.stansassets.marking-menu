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

        static MarkingMenu MarkingMenu
        {
            get
            {
                if (s_MarkingMenu == null)
                {
                    var model = Resources.Load("MarkingMenuModel") as MarkingMenuModel;
                    s_MarkingMenu = new MarkingMenu();
                    // Prevent default event handle
                    s_MarkingMenu.Root.RegisterCallback<MouseUpEvent>((args) =>
                    {
                        args.PreventDefault();
                    }, TrickleDown.TrickleDown);

                    s_MarkingMenu.Init(model);
                }
                return s_MarkingMenu;
            }
        }

        static SceneViewMarkingMenuHook()
        {
            EditorApplication.delayCall += () =>
            {
                SceneView.duringSceneGui += SceneViewOnDuringSceneGui;
            };
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

            MarkingMenu.Open(sceneView.rootVisualElement, localSceneViewRect.center);
        }

        static void SceneViewOnDuringSceneGui(SceneView sceneView)
        {
            if (MarkingMenuSettings.Instance.SceneViewMenuActive && MarkingMenu != null)
            {
                // Set mouse position manually for SceneView because Unity Editor steals events
                if (Event.current.type == EventType.Repaint)
                {
                    MarkingMenu.SetMousePosition(Event.current.mousePosition);
                }

                HandleInput(sceneView);
            }
        }

        static void HandleInput(SceneView sceneView)
        {
            Event e = Event.current;

            if (e.alt || e.control || e.button != 1)
            {
                return;
            }

            // Open Marking Menu
            switch (e.type)
            {
                case EventType.MouseDown:
                    s_MouseDownContext = new MouseDownContext(e.button == 1, e.mousePosition);
                    if (s_MouseDownContext.IsMouseDown)
                    {
                        MarkingMenu.Open(sceneView.rootVisualElement, s_MouseDownContext.Position);

                        // Mark event as used to allow the hovering UI elements by the cursor
                        // Otherwise UI elements will ignore mouse cursor until user pressed 'left' mouse button
                        e.Use();
                    }
                    break;
            }

            if (MarkingMenu.Active)
            {
                Rect cursorRect = new Rect(0, 0, sceneView.camera.pixelWidth, sceneView.camera.pixelHeight);
                EditorGUIUtility.AddCursorRect(cursorRect, MouseCursor.Arrow);
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
