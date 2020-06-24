namespace StansAssets.MarkingMenu
{
    struct ItemCreationContext
    {
        public readonly MarkingMenu MarkingMenu;
        public int ActionsCreated;

        public ItemCreationContext(MarkingMenu markingMenu)
        {
            MarkingMenu = markingMenu;
            ActionsCreated = 0;
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
                    if (ctx.MarkingMenu.TryRegisterActionItem(actionItem))
                    {
                        ctx.ActionsCreated += 1;
                    }

                    return actionItem;

                case ItemType.Toggle:
                    var toggleContext = ctx.MarkingMenu.TryGetToggleContext(model.CustomItemId);
                    var toggleItem = new ToggleItem(model, toggleContext.Get());
                    if (ctx.MarkingMenu.TryRegisterToggleItem(toggleItem))
                    {
                        ctx.ActionsCreated += 1;
                    }

                    return toggleItem;

                case ItemType.Menu:
                    var toggleMenuContext = ctx.MarkingMenu.TryGetToggleMenuContext(model.CustomItemId);
                    var toggleMenuItem = new ToggleMenuItem(model, toggleMenuContext.Get());
                    if (ctx.MarkingMenu.TryRegisterToggleMenuItem(toggleMenuItem))
                    {
                        ctx.ActionsCreated += 1;
                    }

                    return toggleMenuItem;
            }

            return null;
        }
    }
}
