using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using StansAssets.Foundation.UIElements;
using UnityEditor;

namespace StansAssets.MarkingMenu
{
    abstract class MarkingMenuItem
    {
        private const string k_DefaultItemUxmlName = "MarkingMenuItemAdapter";
        private const string k_DefaultItemUssName = "MarkingMenuItemAdapterPersonal";
        private const string k_ProItemUssName = "MarkingMenuItemAdapterPro";

        public string DisplayName => Model.DisplayName;
        public MarkingMenuItemModel Model { get; }
        protected MarkingMenuVisualElement VisualElement { get; }
        public bool MouseOver => m_MouseOver;
        Vector2 Position => new Vector2(m_CenterPosition.x + Model.RelativePosition.x - Model.Pivot.x * Model.Size.x , m_CenterPosition.y + Model.RelativePosition.y + Model.Pivot.y * Model.Size.y);
        
        protected readonly Label m_MenuItemLabel;
        private readonly VisualElement m_MenuItemContainer;
        private VisualElement m_RootElement;
        private Vector2 m_CenterPosition;
        private bool m_MouseOver;

        protected MarkingMenuItem(MarkingMenuItemModel model)
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

        /// <summary>
        /// Enable marking menu ite
        /// </summary>
        /// <param name="rootElement">Parent for menu item</param>
        /// <param name="center">Center of marking menu</param>
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

        /// <summary>
        /// Disable marking menu
        /// </summary>
        public void Disable()
        {
            m_RootElement?.Remove(VisualElement);

            VisualElement?.UnregisterCallback<MouseOverEvent>(MouseOverEventHandler, TrickleDown.TrickleDown);
            VisualElement?.UnregisterCallback<MouseOutEvent>(MouseOutEventHandler, TrickleDown.TrickleDown);
        }

        /// <summary>
        /// Execute marking menu item
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Update marking menu item data
        /// </summary>
        public virtual void UpdateDataFromModel()
        {
            VisualElement.transform.position = Position;
            m_MenuItemLabel.text = Model.DisplayName;
        }

        /// <summary>
        /// Highlight marking menu item
        /// </summary>
        /// <param name="highlighted"></param>
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
