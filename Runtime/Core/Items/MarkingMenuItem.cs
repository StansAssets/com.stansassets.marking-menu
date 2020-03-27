using UnityEditor;

namespace StansAssets.MarkingMenuB
{
    abstract class MarkingMenuItem : IMarkingMenuItem
    {
        protected MarkingMenuItem(int id, MarkingMenuItemModel model)
        {
            Id = id;
            Model = model;
        }

        public int Id { get; }
        public string DisplayName => Model.DisplayName;
        public MarkingMenuItemModel Model { get; }
    }
}
