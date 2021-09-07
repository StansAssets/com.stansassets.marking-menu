﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StansAssets.MarkingMenu
{
    [Serializable]
    public class MarkingMenuModel
    {
        [Header("Settings")]
        public int AngleSelectionDeadZone = 35;
        public int MaxSelectableAngle = 40;

        [Header("Items Collection")]
        public List<MarkingMenuItemModel> Items;

        public MarkingMenuModel()
        {
            Items = new List<MarkingMenuItemModel>();

            InitWithDefaultItems();
        }

        private void InitWithDefaultItems()
        {
            Items.Add(new MarkingMenuItemModel()
            {
                DisplayName = "Play",
                RelativePosition = new Vector2(0, -60),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(80f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Play",
                unityEvent = new UnityEvent()
            });

            Items.Add(new MarkingMenuItemModel()
            {
                DisplayName = "Build Settings",
                RelativePosition = new Vector2(0, 60),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(100f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Build Settings"
            });

            Items.Add(new MarkingMenuItemModel()
            {
                DisplayName = "Player Settings",
                RelativePosition = new Vector2(130, 0),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(100f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Player Settings"
            });

            Items.Add(new MarkingMenuItemModel()
            {
                DisplayName = "Marking Menu Settings",
                RelativePosition = new Vector2(-130, 0),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(150f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Marking Menu Settings"
            });

            Items.Add(new MarkingMenuItemModel()
            {
                DisplayName = "GitHub Page",
                RelativePosition = new Vector2(-130, -30f),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(90f, 30f),
                Type = ItemType.Action,
                CustomItemId = "GitHub Page"
            });
        }
    }
}
