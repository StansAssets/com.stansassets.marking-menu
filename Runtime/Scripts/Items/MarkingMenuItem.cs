using UnityEngine;
using UnityEngine.UIElements;
using StansAssets.Foundation.UIElements;

namespace StansAssets.MarkingMenu
{
    abstract class MarkingMenuItem
    {
        protected const string k_DefaultItemUxmlName = "MarkingMenuItemAdapter";

        protected VisualElement m_RootElement;
        protected Vector2 m_CenterPosition;
        protected bool m_MouseOver;

        public int Id { get; }
        public string DisplayName => Model.DisplayName;
        public MarkingMenuItemModel Model { get; }
        public VisualElement VisualElement { get; }
        public bool MouseOver => m_MouseOver;

        Vector2 Position
        {
            get { return new Vector2(m_CenterPosition.x + Model.RelativePosition.x - Model.Pivot.x * Model.Size.x, m_CenterPosition.y + Model.RelativePosition.y + Model.Pivot.y * Model.Size.y); }
        }

        protected MarkingMenuItem(int id, MarkingMenuItemModel model)
        {
            Id = id;
            Model = model;

            var visualAsset = Resources.Load<VisualTreeAsset>(k_DefaultItemUxmlName);
            VisualElement = visualAsset.CloneTree();
            VisualElement.Q<Label>().text = Model.DisplayName;
            VisualElement.Q<Label>().pickingMode = PickingMode.Ignore;
        }

        public void Enable(VisualElement rootElement, Vector2 center)
        {
            m_RootElement = rootElement;
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

        public void UpdateDataFromModel()
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
