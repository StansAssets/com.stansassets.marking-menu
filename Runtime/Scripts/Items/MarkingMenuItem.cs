using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using StansAssets.Foundation.UIElements;
using UnityEditor;

namespace StansAssets.MarkingMenu
{
    abstract class MarkingMenuItem
    {
        protected const string k_DefaultItemUxmlName = "MarkingMenuItemAdapter";
        protected const string k_DefaultItemUssName = "MarkingMenuItemAdapterPersonal";
        protected const string k_ProItemUssName = "MarkingMenuItemAdapterPro";

        protected VisualElement m_RootElement;
        protected Vector2 m_CenterPosition;
        protected bool m_MouseOver;
        
        public string DisplayName => Model.DisplayName;
        public MarkingMenuItemModel Model { get; }
        public MarkingMenuVisualElement VisualElement { get; }
        public bool MouseOver => m_MouseOver;

        protected readonly Label m_MenuItemLabel;
        protected readonly VisualElement m_MenuItemContainer;

        Vector2 Position => new Vector2(m_CenterPosition.x + Model.RelativePosition.x - Model.Pivot.x * Model.Size.x , m_CenterPosition.y + Model.RelativePosition.y + Model.Pivot.y * Model.Size.y);

        //TODO: check the redundant parameter usefulness
        protected MarkingMenuItem(int id, MarkingMenuItemModel model)
        {
            Model = model;

            var visualAsset = Resources.Load<VisualTreeAsset>(k_DefaultItemUxmlName);
            // the first child - is a MarkingMenuVisualElement object with extended functionality
            var firstChild = visualAsset.CloneTree().Children().First();
            VisualElement = (MarkingMenuVisualElement)firstChild;
            m_MenuItemLabel = VisualElement.Q<Label>("markingMenuItemAdapterName");
            m_MenuItemContainer = VisualElement.Q<VisualElement>("markingMenuItemAdapter");
            m_MenuItemLabel.text = Model.DisplayName;
            m_MenuItemLabel.pickingMode = PickingMode.Ignore;
        }

        public void Enable(VisualElement rootElement, Vector2 center)
        {
            m_RootElement = rootElement;
            
            var ussName  = EditorGUIUtility.isProSkin ? k_ProItemUssName : k_DefaultItemUssName;
            var stylesheet = Resources.Load<StyleSheet>(ussName);
            VisualElement.styleSheets.Add(stylesheet);
            m_MenuItemContainer.style.height = Model.Size.y;
            m_MenuItemContainer.style.width = Model.Size.x;
            
            m_CenterPosition = center;
            m_MouseOver = false;

            m_RootElement.Add(VisualElement);

            VisualElement.RegisterCallback<MouseOverEvent>(MouseOverEventHandler, TrickleDown.TrickleDown);
            VisualElement.RegisterCallback<MouseOutEvent>(MouseOutEventHandler, TrickleDown.TrickleDown);

            UpdateDataFromModel();
        }

        public void Disable()
        {
            m_RootElement?.Remove(VisualElement);

            VisualElement?.UnregisterCallback<MouseOverEvent>(MouseOverEventHandler, TrickleDown.TrickleDown);
            VisualElement?.UnregisterCallback<MouseOutEvent>(MouseOutEventHandler, TrickleDown.TrickleDown);
        }

        public abstract void Execute();

        public bool MouseOverItem()
        {
            return m_MouseOver;
        }

        public virtual void UpdateDataFromModel()
        {
            VisualElement.transform.position = Position;
            m_MenuItemLabel.text = Model.DisplayName;
        }

        public void SetHighlight(bool highlighted)
        {
            var newState = highlighted ? PseudoStates.Hover : PseudoStates.Root;
            var currentState = VisualElement[0].GetPseudoState();
            if (newState != currentState)
            {
                VisualElement[0].SetPseudoState(newState);
            }
        }

        void MouseOverEventHandler(MouseOverEvent evt)
        {
            m_MouseOver = true;
            if (MarkingMenu.DebugMode)
            {
                Debug.Log("MouseOverEventHandler " + Model.CustomItemId);
            }
        }

        void MouseOutEventHandler(MouseOutEvent evt)
        {
            m_MouseOver = false;
        }
    }
}
