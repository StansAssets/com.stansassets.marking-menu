namespace StansAssets.MarkingMenu
{
    interface IMarkingMenuItemActivator<out T> where T : MarkingMenuItem
    {
        /// <summary>
        /// Create marking menu item
        /// </summary>
        /// <param name="model">Marking menu model</param>
        /// <param name="ctx">Item creation context</param>
        /// <returns></returns>
        T CreateItem(MarkingMenuItemModel model, ref ItemCreationContext ctx);
    }
}
