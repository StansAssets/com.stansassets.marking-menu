namespace StansAssets.MarkingMenu
{
    interface IMarkingMenuItemActivator<T> where T : MarkingMenuItem
    {
        T CreateItem(MarkingMenuItemModel model, ref ItemCreationContext ctx);
    }
}
