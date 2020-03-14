using UnityEngine;

namespace StansAssets.MarkingMenu
{
    internal static class EditorInput
    {
        /// <summary>
        ///  Determine if the left mouse button is down
        /// </summary>
        public static bool LeftMouseDown
        {
            get { return Event.current.type == EventType.MouseDown && Event.current.button == 0; }
        }

        /// <summary>
        /// Determine if the left mouse button is up
        /// </summary>
        public static bool LeftMouseUp
        {
            get { return Event.current.type == EventType.MouseUp && Event.current.button == 0; }
        }

        /// <summary>
        /// Determine if we are dragging with the left mouse
        /// </summary>
        public static bool LeftMouseDrag
        {
            get { return Event.current.type == EventType.MouseDrag && Event.current.button == 0; }
        }

        /// <summary>
        /// Determine if the right mouse button is down
        /// </summary>
        public static bool RightMouseDown
        {
            get { return Event.current.type == EventType.MouseDown && Event.current.button == 1; }
        }

        /// <summary>
        /// Determine if the right mouse button is up
        /// </summary>
        public static bool RightMouseUp
        {
            get { return Event.current.type == EventType.MouseUp && Event.current.button == 1; }
        }

        /// <summary>
        /// Determine if the right mouse button is up
        /// </summary>
        public static bool RightMouseDrag
        {
            get { return Event.current.type == EventType.MouseDrag && Event.current.button == 1; }
        }

        /// <summary>
        /// Determine if the middle mouse button is up
        /// </summary>
        public static bool MiddleMouseDrag
        {
            get { return Event.current.type == EventType.MouseDrag && Event.current.button == 2; }
        }

        // Determine if we are pressing a key
        public static bool KeyDown(KeyCode keyCode)
        {
            return Event.current.type == EventType.KeyDown && Event.current.keyCode == keyCode;
        }

        public static bool AltDown
        {
            get { return Event.current.alt; }
        }

        public static bool ShiftDown
        {
            get { return Event.current.shift; }
        }

        public static bool ControlDown
        {
            get { return Event.current.control; }
        }

        public static Vector2 mousePosition
        {
            get { return Event.current.mousePosition; }
        }

        public static bool MiddleMouseDown
        {
            get { return Event.current.type == EventType.MouseDown && Event.current.button == 2; }
        }

        public static bool DeletePressed
        {
            get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete; }
        }

        public static bool EscapePressed
        {
            get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape; }
        }

        public static bool P_Down
        {
            get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.P; }
        }

        public static bool P_Up
        {
            get { return Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.P; }
        }

        public static bool OnLeftMouseDoubleClickDown
        {
            get { return LeftMouseDown && Event.current.clickCount == 2; }
        }

        public static void Use()
        {
            Event.current.Use();
        }
    }
}
