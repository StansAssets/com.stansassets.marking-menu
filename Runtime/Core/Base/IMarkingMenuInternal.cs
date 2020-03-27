namespace StansAssets.MarkingMenuB {
    interface IMarkingMenuInternal : IMarkingMenu
    {
        IMarkingMenuDisplayContext DisplayContext { get; }
    }
}
