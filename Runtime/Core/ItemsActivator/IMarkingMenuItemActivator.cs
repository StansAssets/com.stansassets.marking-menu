namespace StansAssets.MarkingMenu
{
    interface IMarkingMenuItemActivator<T> where T : IMarkingMenuItem
    {
        T CreateItem(MarkingMenuItemModel model, ref ItemCreationContext ctx);
    }
}
