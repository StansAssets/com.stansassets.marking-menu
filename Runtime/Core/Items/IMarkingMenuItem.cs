using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu {
    public interface IMarkingMenuItem
    {
        int Id { get; }
        string DisplayName { get; }

        bool MouseOver { get; }

        MarkingMenuItemModel Model { get; }
        VisualElement VisualElement { get; }

        void Enable(VisualElement rootElement, Vector2 center);
        void Disable();

        void Execute();
        bool MouseOverItem();

        #if UNITY_EDITOR
        void UpdateDataFromModel();
        #endif
    }
}
