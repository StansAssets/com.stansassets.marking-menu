using UnityEditor;
using UnityEngine;

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

    class BaseEditorActions
    {
        // [ActionItemAttribute()]
        public static void Register(MarkingMenu menu)
        {
            menu.Register("Action1", () => Debug.Log("Action1 executed!"));
            menu.Register("Action2", () => Debug.Log("Action2 executed!"));
            menu.Register("Action3", () => Debug.Log("Action3 executed!"));
            menu.Register("Action4", () => Debug.Log("Action4 executed!"));
            menu.Register("Action5", () => Debug.Log("Action5 executed!"));
        }
    }

    abstract class ItemView<T> where T : MarkingMenuItem
    {

        public ItemView(T item)
        {

        }
    }

    class ActionItemView : ItemView<ActionItem>
    {
        public ActionItemView(ActionItem item)
            : base(item)
        {
            item.Execute();
        }

        public void Render()
        {

        }
    }
}
