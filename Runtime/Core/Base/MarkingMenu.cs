using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    public static class MarkingMenuService
	{
		public static MarkingMenu CreateMenu() {
			return new MarkingMenu();
        }
	}

    public partial class MarkingMenu : VisualElement, IMarkingMenu
    {
        public event Action OnOpened;
        public event Action OnClosed;

        readonly Dictionary<string, Action> m_Actions = new Dictionary<string, Action>();
        readonly Dictionary<string, ToggleContext> m_Toggles = new Dictionary<string, ToggleContext>();

        MarkingMenuModel m_Model;
        readonly List<IMarkingMenuItem> m_Items = new List<IMarkingMenuItem>(10);

        IMarkingMenuInputController m_InputController;
        IMarkingMenuItemActivator<IMarkingMenuItem> m_Activator;

        public bool Active { get; private set; }
        public VisualElement Parent { get; protected set; }
        public VisualElement Root => this;
        public Vector2 Center { get; private set; }
        public IList<IMarkingMenuItem> Items => m_Items;

        public static bool DebugMode { get; set; }

        public void Init(MarkingMenuModel model)
        {
            Reset();

            m_Model = model;

            m_InputController = new MarkingMenuInputController(this);
            m_Activator = new VisualElementMarkingMenuItemActivator();

            RegisterActionsOutside();
            CreateItems(m_Model);

            InitVisual();
        }

        public void Open(VisualElement root, Vector2 center)
        {
            Active = true;
            Center = center;
            Parent = root;

            // Subscribe to events
            RegisterCallback<PointerUpEvent>(PointerUpEventHandler, TrickleDown.TrickleDown);
            RegisterCallback<MouseUpEvent>(MouseUpEventHandler, TrickleDown.TrickleDown);

            Parent.Add(this);
            // move to constructor
            this.style.flexGrow = 1.0f;

            EnableItems(this, center);

            OpenVisual();

            OnOpened?.Invoke();

            // TODO: Remove
#if UNITY_EDITOR
            SceneView.duringSceneGui += SceneViewOnDuringSceneGui;
#endif
        }

        public void Close()
        {
            Active = false;

            UnregisterCallback<PointerUpEvent>(PointerUpEventHandler, TrickleDown.TrickleDown);
            UnregisterCallback<MouseUpEvent>(MouseUpEventHandler, TrickleDown.TrickleDown);
            Parent.Remove(this);
            DisableItems();

            CloseVisual();

            OnClosed?.Invoke();

            // TODO: Remove
#if UNITY_EDITOR
            SceneView.duringSceneGui -= SceneViewOnDuringSceneGui;
#endif
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
                throw new ActionRegistrationException($"Item {item.Model.DisplayName} has Action type but CustomItemId is null or empty!");
            }
            else if (m_Actions.ContainsKey(item.Model.CustomItemId) == false)
            {
                throw new ActionRegistrationException($"Registration for action with id \"{item.Model.CustomItemId}\" not found!");
            }

            return true;
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

        // TODO: remove
#if UNITY_EDITOR
        void SceneViewOnDuringSceneGui(SceneView sceneView)
        {
            if (Active)
            {
                Event e = Event.current;

                Handles.BeginGUI();

                if (e.type == EventType.Repaint)
                {
                    Handles.color = Color.black;
                    Handles.DrawLine(Center, e.mousePosition);
                    Handles.color = Color.white;

                    // Center
                    Rect pivotRect = new Rect(Vector2.zero, Vector2.one * 10f);
                    pivotRect.center = Center;
                    Handles.DrawSolidRectangleWithOutline(pivotRect, Color.grey, Color.grey);
                }

                Handles.EndGUI();

                if (MarkingMenu.DebugMode)
                {
                    for (var i = 0; i < m_Items.Count; ++i)
                    {
                        var item = m_Items[i];
                        item.UpdateDataFromModel();
                    }
                }

                sceneView.Repaint();
            }
        }
#endif

        void Reset()
        {
            m_Model = null;
            Active = false;

            m_Actions.Clear();
            m_Toggles.Clear();
            m_Items.Clear();
        }

        IMarkingMenuItem CreateItem(MarkingMenuItemModel model, ref ItemCreationContext ctx)
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
                    if (attributes.Length > 0)
                    {
                        var method = type.GetMethod("Register", BindingFlags.Public | BindingFlags.Static);
                        if (method != null)
                        {
                            method.Invoke(null, new object[] {this});
                        }
                        else
                        {
                            throw new ActionRegistrationException($"Class {type.Name} has CustomActionsRegistration attribute, but doesn't implement Register(IMarkingMenu menu) method");
                        }
                    }
                }
            }
        }

        void PointerUpEventHandler(PointerUpEvent evt)
        {
            evt.PreventDefault();

            m_InputController.HandleEvent(evt);

        }

        void MouseUpEventHandler(MouseUpEvent evt)
        {
            evt.PreventDefault();
        }
    }
}
