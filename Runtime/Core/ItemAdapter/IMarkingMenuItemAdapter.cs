using UnityEngine;

namespace StansAssets.MarkingMenuB
{
    interface IMarkingMenuItemAdapter
    {
        void Enable();
        void Disable();

        void SetRootPosition(Vector2 center);
        void UpdateDataFromModel();

    }

    interface IMarkingMenuItemAdapter<T> :IMarkingMenuItemAdapter
        where T : IMarkingMenuItem
    {

    }
}
