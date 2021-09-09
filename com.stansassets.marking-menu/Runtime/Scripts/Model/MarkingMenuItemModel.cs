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

        public MarkingMenuItemModel()
        {
            UnityEvent = new UnityEvent();
        }
    }
}
