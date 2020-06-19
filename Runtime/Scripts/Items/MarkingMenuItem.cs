using UnityEngine;
using UnityEngine.UIElements;
using StansAssets.Foundation.UIElements;
using UnityEditor;

namespace StansAssets.MarkingMenu
{
    abstract class MarkingMenuItem
    {
        protected const string k_DefaultItemUxmlName = "MarkingMenuItemAdapter";
        protected const string k_DefaultItemUssName = "MarkingMenuItemAdapter";
        protected const string k_ProItemUssName = "MarkingMenuItemAdapterPro";

        protected VisualElement m_RootElement;
        protected Vector2 m_CenterPosition;
        protected bool m_MouseOver;

        public int Id { get; }
        public string DisplayName => Model.DisplayName;
        public MarkingMenuItemModel Model { get; }
        public VisualElement VisualElement { get; }
        public bool MouseOver => m_MouseOver;

        protected Label m_VisualElementName;

        Vector2 Position
        {
            get { return new Vector2(m_CenterPosition.x + Model.RelativePosition.x - Model.Pivot.x * 80f, m_CenterPosition.y + Model.RelativePosition.y + Model.Pivot.y * 20f); }
        }

        protected MarkingMenuItem(int id, MarkingMenuItemModel model)
        {
            Id = id;
            Model = model;

            var visualAsset = Resources.Load<VisualTreeAsset>(k_DefaultItemUxmlName);
            VisualElement = visualAsset.CloneTree();
            m_VisualElementName = VisualElement.Q<Label>("markingMenuItemAdapterName");
            m_VisualElementName.text = Model.DisplayName;
            m_VisualElementName.pickingMode = PickingMode.Ignore;
        }

        public void Enable(VisualElement rootElement, Vector2 center)
        {
            m_RootElement = rootElement;
            
            var ussName  = EditorGUIUtility.isProSkin ? k_ProItemUssName : k_DefaultItemUssName;
            var stylesheet = Resources.Load<StyleSheet>(ussName);
            m_RootElement.styleSheets.Add(stylesheet);
            
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
            VisualElement.Q<Label>().text = Model.DisplayName;
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
