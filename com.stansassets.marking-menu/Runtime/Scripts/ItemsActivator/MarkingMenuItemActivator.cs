namespace StansAssets.MarkingMenu
{
    struct ItemCreationContext
    {
        public readonly MarkingMenu MarkingMenu;

        public ItemCreationContext(MarkingMenu markingMenu)
        {
            MarkingMenu = markingMenu;
        }
    }

    class VisualElementMarkingMenuItemActivator : IMarkingMenuItemActivator<MarkingMenuItem>
    {
        public MarkingMenuItem CreateItem(MarkingMenuItemModel model, ref ItemCreationContext ctx)
        {
            switch (model.Type)
            {
                case ItemType.Action:
                    var actionItem = new ActionItem(model);
                    ctx.MarkingMenu.RegisterActionItem(actionItem);
                    
                    return actionItem;

                case ItemType.Toggle:
                    var toggleContext = ctx.MarkingMenu.TryGetToggleContext(model.CustomItemId);
                    var toggleItem = new ToggleItem(model, toggleContext.Get());
                    ctx.MarkingMenu.RegisterToggleItem(toggleItem);

                    return toggleItem;
                
                case ItemType.Menu:
                    var toggleMenuContext = ctx.MarkingMenu.TryGetToggleMenuContext(model.CustomItemId);
                    var toggleMenuItem = new ToggleMenuItem(model, toggleMenuContext.Get());
                    ctx.MarkingMenu.RegisterToggleMenuItem(toggleMenuItem);
                    
                    return toggleMenuItem;
                
                default:
                    return null;
            }
        }
    }
}
