using System;
using System.Collections.Generic;
using UnityEngine;

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
        public Vector2 Size = new Vector2(100f, 20f);
        public Vector2 Pivot = new Vector2(0.5f, 0.5f);

        public ItemType Type;
        public string CustomItemId;//PlaymodeActionItem

        // TODO: place to think
        // public int Id;
        // public List<int> Children;
        // public string UxmlPath;
    }
}
