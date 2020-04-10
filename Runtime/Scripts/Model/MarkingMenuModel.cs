using System.Collections.Generic;
using UnityEngine;

namespace StansAssets.MarkingMenu
{
    public class MarkingMenuModel : ScriptableObject
    {
        [Header("Settings")]
        public int AngleSelectionDeadZone = 25;
        public int MaxSelectableAngle = 30;

        [Header("Items Collection")]
        public List<MarkingMenuItemModel> Items;

        public MarkingMenuModel()
        {
            Items = new List<MarkingMenuItemModel>();
        }
    }
}
