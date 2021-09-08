using System;
using UnityEngine;
using UnityEngine.Events;

namespace StansAssets.MarkingMenu
{
    public enum ItemType
    {
        Action,
        Toggle,
        Menu
    }

    [Serializable]
    public class MarkingMenuItemModel
    {
        public string DisplayName;

        public Vector2 RelativePosition = new Vector2(100f, 0f);
        public Vector2 Pivot = new Vector2(0.5f, 0.5f);
        public Vector2 Size = new Vector2(100f, 20f);

        public ItemType Type;
        public string CustomItemId;
        public UnityEvent UnityEvent;
        
        // TODO: place to think about
        // public int Id;
        // public List<int> Children;
        // public string UxmlPath;

        public MarkingMenuItemModel(Action actionEvent)
        {
            UnityEvent = new UnityEvent();
            UnityEvent.AddListener(() =>
            {
                actionEvent();
            });

            for (int i = 0; i < UnityEvent.GetPersistentEventCount(); i++)
            {
                UnityEvent.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);
            }
        }
    }
}
