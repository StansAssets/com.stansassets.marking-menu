using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenuB
{
    public class MarkingMenuModel : ScriptableObject
    {
        // Base UXML style
        public List<MarkingMenuItemModel> Items;

        public MarkingMenuModel()
        {
            Items = new List<MarkingMenuItemModel>();
        }
    }
}
