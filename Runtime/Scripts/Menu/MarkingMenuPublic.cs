using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    public partial class MarkingMenu
    {
        public event Action OnOpened;
        public event Action OnClosed;

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

        public void Init(MarkingMenuModel model)
        {
            Reset();

            m_Model = model;

            m_Activator = new VisualElementMarkingMenuItemActivator();
            
            CreateItems(m_Model);

            InitVisual(model);
        }

        public void Open(VisualElement root, Vector2 center)
        {
            CreateItems(m_Model);
            
            OpenCore(root, center);
            OpenVisual(root, center);

            OnOpened?.Invoke();
        }

        private void Close()
        {
            CloseCore();
            CloseVisual();

            OnClosed?.Invoke();
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

        protected void FireOnOpened()
        {
            OnOpened?.Invoke();
        }

        protected void FireOnClosed()
        {
            OnClosed?.Invoke();
        }
    }
}
