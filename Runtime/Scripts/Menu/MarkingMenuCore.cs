using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    public partial class MarkingMenu
    {
        readonly Dictionary<string, Action> m_Actions = new Dictionary<string, Action>();
        readonly Dictionary<string, ToggleContext> m_Toggles = new Dictionary<string, ToggleContext>();
        readonly Dictionary<string, ToggleMenuContext> m_ToggleMenus = new Dictionary<string, ToggleMenuContext>();

        MarkingMenuModel m_Model;
        readonly List<MarkingMenuItem> m_Items = new List<MarkingMenuItem>(10);

        IMarkingMenuItemActivator<MarkingMenuItem> m_Activator;

        internal VisualElement Parent { get; private set; }
        internal Vector2 Center { get; private set; }
        internal IList<MarkingMenuItem> Items => m_Items;

        void OpenCore(VisualElement root, Vector2 center)
        {
            Active = true;
            Center = center;
            Parent = root;

            Parent.Add(this);
            EnableItems(this, center);
        }

        void CloseCore()
        {
            if (Active)
            {
                Parent.Remove(this);
                DisableItems();
            }

            Active = false;
        }

        void CreateItems(MarkingMenuModel model)
        {
            ItemCreationContext ctx = new ItemCreationContext(this);
            for (var i = 0; i < model.Items.Count; ++i)
            {
                var item = CreateItem(model.Items[i], ref ctx);
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

        internal bool TryRegisterActionItem(ActionItem item)
        {
            item.OnItemExecuted += OnItemExecutedHandler;

            if (string.IsNullOrEmpty(item.Model.CustomItemId))
            {
                throw new ArgumentException($"Item {item.Model.DisplayName} has Action type but CustomItemId is null or empty!");
            }
            else if (m_Actions.ContainsKey(item.Model.CustomItemId) == false)
            {
                throw new ArgumentException($"Registration for action with id \"{item.Model.CustomItemId}\" not found!");
            }

            return true;
        }
        
        internal bool TryRegisterToggleItem(ToggleItem item)
        {
            item.OnItemExecuted += OnItemExecutedHandler;

            if (string.IsNullOrEmpty(item.Model.CustomItemId))
            {
                throw new ArgumentException($"Item {item.Model.DisplayName} has Action type but CustomItemId is null or empty!");
            }
            else if (m_Toggles.ContainsKey(item.Model.CustomItemId) == false)
            {
                throw new ArgumentException($"Registration for action with id \"{item.Model.CustomItemId}\" not found!");
            }

            return true;
        }
        
        internal bool TryRegisterToggleMenuItem(ToggleMenuItem item)
        {
            item.OnItemExecuted += OnItemExecutedHandler;

            if (string.IsNullOrEmpty(item.Model.CustomItemId))
            {
                throw new ArgumentException($"Item {item.Model.DisplayName} has Action type but CustomItemId is null or empty!");
            }
            else if (m_ToggleMenus.ContainsKey(item.Model.CustomItemId) == false)
            {
                throw new ArgumentException($"Registration for action with id \"{item.Model.CustomItemId}\" not found!");
            }

            return true;
        }
        
        internal ToggleMenuContext TryGetToggleMenuContext(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"Item {key} has Action type but CustomItemId is null or empty!");
            }
            if (m_ToggleMenus.ContainsKey(key) == false)
            {
                throw new ArgumentException($"Registration for action with id \"{key}\" not found!");
            }

            return m_ToggleMenus[key];
        }

        internal ToggleContext TryGetToggleContext(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"Item {key} has Action type but CustomItemId is null or empty!");
            }
            if (m_Toggles.ContainsKey(key) == false)
            {
                throw new ArgumentException($"Registration for action with id \"{key}\" not found!");
            }

            return m_Toggles[key];
        }

        void EnableItems(VisualElement root, Vector2 center)
        {
            for (var i = 0; i < m_Items.Count; ++i)
            {
                var item = m_Items[i];
                item.Enable(root, center);
            }
        }

        void DisableItems()
        {
            for (var i = 0; i < m_Items.Count; ++i)
            {
                var item = m_Items[i];
                item.Disable();
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

        MarkingMenuItem CreateItem(MarkingMenuItemModel model, ref ItemCreationContext ctx)
        {
            return m_Activator.CreateItem(model, ref ctx);
        }

        // This callback is to link Items action Ids to actual Actions within registered ones in this class (Action, Toggles etc).
        void OnItemExecutedHandler(ItemExecutedEventArgs args)
        {
            if (MarkingMenu.DebugMode)
            {
                Debug.Log($"Item with Id {args.Id} and Type {args.Type} executed!");
            }

            switch (args.Item.Model.Type)
            {
                case ItemType.Action:
                    m_Actions[args.Id]?.Invoke();
                    break;

                case ItemType.Toggle:
                    bool currentState = m_Toggles[args.Id].Get.Invoke();
                    m_Toggles[args.Id].Set.Invoke(!currentState);
                    break;
                case ItemType.Menu:
                    var currentStateMenu = m_ToggleMenus[args.Id].Get.Invoke().CurrentItem;
                    m_ToggleMenus[args.Id].Set.Invoke(currentStateMenu);
                    break;
            }
        }

        void RegisterActionsOutside()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attributes = type.GetCustomAttributes(typeof(CustomActionRegistrationAttribute), false);
                    if (attributes.Length > 0 && ((CustomActionRegistrationAttribute)attributes[0]).Enabled)
                    {
                        var method = type.GetMethod("Register", BindingFlags.Public | BindingFlags.Static);
                        if (method != null)
                        {
                            method.Invoke(null, new object[] {this});
                        }
                        else
                        {
                            throw new InvalidOperationException($"Class {type.Name} has CustomActionsRegistration attribute, but doesn't implement Register(IMarkingMenu menu) method");
                        }
                    }
                }
            }
        }
    }
}
