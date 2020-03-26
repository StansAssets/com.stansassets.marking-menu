using UnityEngine.UIElements;

namespace StansAssets.MarkingMenuB
{
    interface IVisualElementItemAdapter : IMarkingMenuItemAdapter
    {
        VisualElement VisualElement { get; }
        void SetRootElement(VisualElement rootElement);
    }

    interface IVisualElementItemAdapter<T> : IVisualElementItemAdapter
        where T : IMarkingMenuItem
    {

    }
}
