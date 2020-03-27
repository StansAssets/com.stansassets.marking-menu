using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StansAssets.MarkingMenuB
{
    interface IMarkingMenuDisplayContext
    {
        IMarkingMenuRenderer Renderer {get;}
        IMarkingMenuInputController InputController {get;}

        void Reset();
    }

    class MarkingMenuDefaultDisplayContext : IMarkingMenuDisplayContext
    {
        public IMarkingMenuRenderer Renderer { get; }
        public IMarkingMenuInputController InputController { get; }

        public MarkingMenuDefaultDisplayContext(IMarkingMenuInternal markingMenu)
        {
            Renderer = new MarkingMenuDefaultRenderer();
            InputController = new MarkingMenuDefaultInputController();

            Renderer.Init(markingMenu);
        }

        public void Reset()
        {
            Renderer.Reset();
        }
    }

    public static class MarkingMenuService
	{
		public static IMarkingMenu CreateMenu() {
			// Reset prev state
			MarkingMenuFactory.MMAdapterFactory.ResetAdapters();

			// Init adapter factory for MarkingMenuItem adapters
			MarkingMenuFactory.MMAdapterFactory.CreateAdapterFactory();
			return new MarkingMenu();
		}
	}

    class MarkingMenu : IMarkingMenuInternal
    {
        public event Action OnOpened;
        public event Action OnClosed;

        protected readonly Dictionary<string, Action> m_Actions = new Dictionary<string, Action>();
        protected readonly Dictionary<string, ToggleContext> m_Toggles = new Dictionary<string, ToggleContext>();

        protected MarkingMenuModel m_Model;
        protected readonly List<IMarkingMenuItem> m_Items = new List<IMarkingMenuItem>(10);

        public bool Active { get; private set; }
        public Vector2 Center { get; private set; }
        public IList<IMarkingMenuItem> Items => m_Items;
        public IMarkingMenuDisplayContext DisplayContext { get; private set; }

        public bool DebugMode { get; set; }

        public void Init(MarkingMenuModel model)
        {
            Reset();

            m_Model = model;
            CreateItems(m_Model);

            DisplayContext = new MarkingMenuDefaultDisplayContext(this);
        }

        public void Open(Vector2 center)
        {
            Active = true;
            Center = center;
            OnOpened?.Invoke();
        }

        public void Close()
        {
            Active = false;
            OnClosed?.Invoke();
        }

        // Register all action from Marking Menu
        public void Register(string id, Action action)
        {
            if (m_Actions.ContainsKey(id))
            {
                Debug.LogError($"Action with id: {id} is already added!");
                return;
            }

            m_Actions[id] = action;
        }

        public void Register(string id, ToggleContext ctx)
        {
            if (m_Actions.ContainsKey(id))
            {
                Debug.LogError($"ToggleContext with id: {id} is already added!");
                return;
            }

            m_Toggles[id] = ctx;
        }

        void CreateItems(MarkingMenuModel model)
        {
            for (var i = 0; i < model.Items.Count; ++i)
            {
                var item = CreateItem(model.Items[i]);
                if (item != null)
                {
                    m_Items.Add(item);
                }
                else
                {
                    Debug.LogError($"Item {model.Items[i].DisplayName} wasn't created, check serialization!");
                }
            }
        }

        void Reset()
        {
            m_Model = null;
            Active = false;

            m_Actions.Clear();
            m_Toggles.Clear();
            m_Items.Clear();
        }

        IMarkingMenuItem CreateItem(MarkingMenuItemModel model)
        {
            switch (model.Type)
            {
                case ItemType.Action:
                    ActionItem actionItem = new ActionItem(model);
                    actionItem.OnItemExecuted += OnItemClicked;
                    return actionItem;

                //case ItemType.Toggle:
                    //break;

                //case ItemType.Menu:
                    //break;
            }

            return null;
        }

        // This callback is to link Items action Ids to actual Actions within registered ones in this class (Action, Toggles etc).
        void OnItemClicked(ItemExecutedEventArgs args)
        {
            Debug.Log($"Item with Id {args.Id} and Type {args.Type} clicked!");
        }

        bool ValidateCreatedMenu()
        {
            return EnsureAllItemsRegistered();
        }

        bool EnsureAllItemsRegistered()
        {
            return true;
        }
    }
}
