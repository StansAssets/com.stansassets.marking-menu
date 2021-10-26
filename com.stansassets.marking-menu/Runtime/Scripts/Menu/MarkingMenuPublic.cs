using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    public partial class MarkingMenu
    {
        public bool Active { get; private set; }
        public VisualElement Root => this;
        public static bool DebugMode { get; set; }

        public MarkingMenu()
        {
            style.flexGrow = 1.0f;
            schedule.Execute(MarkDirtyRepaint).Every(16);
            schedule.Execute(HandleInput).Every(16);

#if UNITY_EDITOR
            schedule.Execute(UpdateDebugMode).Every(16);
#endif
        }
        
        /// <summary>
        /// Init MarkingMenu
        /// </summary>
        /// <param name="model">Marking menu model</param>
        public void Init(MarkingMenuModel model)
        {
            Reset();

            m_Model = model;

            m_Activator = new VisualElementMarkingMenuItemActivator();

            InitVisual();
        }

        /// <summary>
        /// Open MarkingMenu
        /// </summary>
        /// <param name="root">Parent for visual element</param>
        /// <param name="center">Center of marking menu</param>
        public void Open(VisualElement root, Vector2 center)
        {
            if (Active) {
                return;
            } 
            
            UpdateItems(m_Model);
            OpenCore(root, center);
            OpenVisual();

           
        }

        private void Close()
        {
            CloseCore();
            CloseVisual();
        }
        
#if UNITY_EDITOR
        public void SetMousePosition(Vector2 mousePosition)
        {
            m_MousePosition = mousePosition;
        }
#endif

#if UNITY_EDITOR
        void UpdateDebugMode()
        {
            if (DebugMode)
            {
                for (var i = 0; i < m_Items.Count; ++i)
                {
                    var item = m_Items[i];
                    item.UpdateDataFromModel();
                }
            }
        }
#endif
    }
}
