using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        
        /// <summary>
        /// Init MarkingMenuModel
        /// </summary>
        public void Init()
        {
            InitWithDefaultItems();
        }

        /// <summary>
        /// Update menu item buttons with default actions
        /// </summary>
        /// <param name="markingMenuActions">ScriptableObject with actions for marking menu buttons</param>
        public void UpdateDefaultItemActions(MarkingMenuActions markingMenuActions)
        {
            foreach (var item in Items)
            {
                switch (item.CustomItemId)
                {
                    case "Play":
                        item.UnityEvent.RemoveAllListeners();
                        item.UnityEvent.AddListener(markingMenuActions.RegisterPlayAction);
                        break;
                    
                    case "Build Settings":
                        item.UnityEvent.RemoveAllListeners();
                        item.UnityEvent.AddListener(markingMenuActions.RegisterBuildSettingsAction);
                        break;
                    
                    case "Player Settings":
                        item.UnityEvent.RemoveAllListeners();
                        item.UnityEvent.AddListener(markingMenuActions.RegisterPlayerSettingsAction);
                        break;
                    
                    case "Marking Menu Settings":
                        item.UnityEvent.RemoveAllListeners();
                        item.UnityEvent.AddListener(markingMenuActions.RegisterMarkingMenuSettingsAction);
                        break;
                    
                    case "GitHub Page":
                        item.UnityEvent.RemoveAllListeners();
                        item.UnityEvent.AddListener(markingMenuActions.RegisterGitHubPageAction);
                        break;
                }
            }
        }

        private void InitWithDefaultItems()
        {
            if(Items.Count != 0) return;
            
            Items.Add(new MarkingMenuItemModel()
            {
                DisplayName = "Play",
                RelativePosition = new Vector2(0, -60),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(80f, 30f),
                Type = ItemType.Action,
                CustomItemId = "Play",
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
