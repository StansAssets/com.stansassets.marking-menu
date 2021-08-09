namespace StansAssets.MarkingMenu
{
    interface IMarkingMenuItemActivator<out T> where T : MarkingMenuItem
    {
        T CreateItem(MarkingMenuItemModel model, ref ItemCreationContext ctx);
    }
}
