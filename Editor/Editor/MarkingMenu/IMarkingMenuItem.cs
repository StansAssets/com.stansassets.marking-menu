using UnityEngine;

namespace  StansAssets.MarkingMenu
{
    interface IMarkingMenuItem
    {
        Rect OnGUI(MarkingMenuEvent e);
    }
}
