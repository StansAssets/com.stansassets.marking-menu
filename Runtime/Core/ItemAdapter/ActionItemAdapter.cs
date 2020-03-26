using JetBrains.Annotations;

namespace StansAssets.MarkingMenuB
{
    [UsedImplicitly]
    [MarkingMenuFactory.MMAdapterAttribute(typeof(ActionItem))]
    class ActionItemAdapter : BaseVisualElementItemAdapter<ActionItem>
    {
        public ActionItemAdapter(IMarkingMenuItem item)
            : base(item) { }
    }
}
