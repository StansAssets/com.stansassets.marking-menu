using System;
using UnityEditor;
using UnityEngine;

namespace StansAssets.MarkingMenuB
{
    class MarkingMenuDefaultRenderer : IMarkingMenuRenderer
    {
        IMarkingMenuInternal m_MarkingMenu;

        Vector2 m_Center;
        Action<SceneView> m_PreGUI;

        public void Init(IMarkingMenuInternal menu)
        {
            m_MarkingMenu = menu;

            m_PreGUI += InitItemViews;

            m_MarkingMenu.OnOpened += MarkingMenuOpened;
            m_MarkingMenu.OnClosed += MarkingMenuClosed;

            // TODO: Remove
            SceneView.duringSceneGui += SceneViewOnDuringSceneGui;
        }

        public void Reset()
        {
            if (m_MarkingMenu != null)
            {
                m_MarkingMenu.OnOpened -= MarkingMenuOpened;
                m_MarkingMenu.OnOpened -= MarkingMenuClosed;
            }

            // TODO: Remove
            SceneView.duringSceneGui -= SceneViewOnDuringSceneGui;
        }

        void MarkingMenuOpened()
        {
            m_Center = m_MarkingMenu.Center;
            m_PreGUI += (sceneView) =>
            {
                for (var i = 0; i < m_MarkingMenu.Items.Count; ++i)
                {
                    var item = m_MarkingMenu.Items[i];
                    var adapter = MarkingMenuFactory.MMAdapterFactory.GetAdapter(item);
                    adapter?.SetRootPosition(m_Center);
                    adapter?.Enable();
                }
            };
        }

        void MarkingMenuClosed()
        {
            for (var i = 0; i < m_MarkingMenu.Items.Count; ++i)
            {
                var item = m_MarkingMenu.Items[i];
                var adapter = MarkingMenuFactory.MMAdapterFactory.GetAdapter(item);
                adapter?.Disable();
            }
        }

        void InitItemViews(SceneView sceneView)
        {
            for (var i = 0; i < m_MarkingMenu.Items.Count; ++i)
            {
                var item = m_MarkingMenu.Items[i];
                var adapter = MarkingMenuFactory.MMAdapterFactory.GetAdapter(item);
                // TODO: remove is
                if (adapter is IVisualElementItemAdapter itemAdapter)
                {
                    itemAdapter.SetRootElement(sceneView.rootVisualElement);
                }
            }
        }

        void SceneViewOnDuringSceneGui(SceneView sceneView)
        {
            m_PreGUI?.Invoke(sceneView);
            m_PreGUI = null;

            if (m_MarkingMenu.Active)
            {
                Event e = Event.current;

                Handles.BeginGUI();

                if (e.type == EventType.Repaint)
                {
                    Handles.color = Color.black;
                    Handles.DrawLine(m_Center, e.mousePosition);
                    Handles.color = Color.white;

                    // Center
                    Rect pivotRect = new Rect(Vector2.zero, Vector2.one * 10f);
                    pivotRect.center = m_Center;
                    Handles.DrawSolidRectangleWithOutline(pivotRect, Color.grey, Color.grey);
                }

                // Debug Mode
                if (m_MarkingMenu.DebugMode)
                {
                    for (var i = 0; i < m_MarkingMenu.Items.Count; ++i)
                    {
                        var adapter = MarkingMenuFactory.MMAdapterFactory.GetAdapter(m_MarkingMenu.Items[i]);
                        adapter.UpdateDataFromModel();
                    }
                }

                Handles.EndGUI();

                sceneView.Repaint();
            }
        }
    }
}
