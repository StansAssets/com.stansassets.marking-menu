using System;
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
        }
        
        public void Init(MarkingMenuActions markingMenuActions)
        {
            InitWithDefaultItems(markingMenuActions);
        }

        private void InitWithDefaultItems(MarkingMenuActions markingMenuActions)
        {
            if(Items.Count != 0) return;
            
            Items.Add(new MarkingMenuItemModel(markingMenuActions.RegisterPlayAction)
            {
                DisplayName = "Play",
                RelativePosition = new Vector2(0, -60),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(80f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Play",
            });

            Items.Add(new MarkingMenuItemModel(markingMenuActions.RegisterBuildSettingsAction)
            {
                DisplayName = "Build Settings",
                RelativePosition = new Vector2(0, 60),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(100f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Build Settings"
            });

            Items.Add(new MarkingMenuItemModel(markingMenuActions.RegisterPlayerSettingsAction)
            {
                DisplayName = "Player Settings",
                RelativePosition = new Vector2(130, 0),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(100f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Player Settings"
            });

            Items.Add(new MarkingMenuItemModel(markingMenuActions.RegisterMarkingMenuSettingsAction)
            {
                DisplayName = "Marking Menu Settings",
                RelativePosition = new Vector2(-130, 0),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(150f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Marking Menu Settings"
            });

            Items.Add(new MarkingMenuItemModel(markingMenuActions.RegisterGitHubPageAction)
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
