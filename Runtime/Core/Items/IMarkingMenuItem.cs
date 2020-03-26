namespace StansAssets.MarkingMenuB {
    public interface IMarkingMenuItem
    {
        int Id { get; }
        string DisplayName { get; }

        MarkingMenuItemModel Model { get; }
    }
}
