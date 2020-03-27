﻿using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenuB
{
    abstract class BaseVisualElementItemAdapter<T> : IVisualElementItemAdapter<T>
        where T : IMarkingMenuItem
    {
        public event Action OnClicked;

        protected const string k_DefaultUXMLPath = "MarkingMenuItemAdapter";

        protected readonly IMarkingMenuItem m_Item;
        protected VisualElement m_RootElement;
        protected Vector2 m_CenterPosition;

        public Rect Rect => VisualElement?.worldBound ?? Rect.zero;
        public VisualElement VisualElement { get; }

        protected BaseVisualElementItemAdapter(IMarkingMenuItem item)
        {
            m_Item = item;
            var visualAsset = Resources.Load<VisualTreeAsset>(k_DefaultUXMLPath);
            VisualElement = visualAsset.CloneTree();

            VisualElement.Q<Label>().text = m_Item.Model.DisplayName;
        }

        public void SetRootElement(VisualElement rootElement)
        {
            Disable();
            m_RootElement = rootElement;
            m_RootElement.RegisterCallback<PointerUpEvent>(PointerUpEventHandler, TrickleDown.TrickleDown);
            m_RootElement.RegisterCallback<PointerDownEvent>(PointerDownEventHandler, TrickleDown.TrickleDown);
        }

        public void Enable()
        {
            m_RootElement.Add(VisualElement);
            UpdateDataFromModel();
        }

        public void Disable()
        {
            m_RootElement?.Remove(VisualElement);
            m_RootElement?.UnregisterCallback<PointerUpEvent>(PointerUpEventHandler, TrickleDown.TrickleDown);
            m_RootElement?.UnregisterCallback<PointerDownEvent>(PointerDownEventHandler, TrickleDown.TrickleDown);
        }

        public void SetRootPosition(Vector2 center)
        {
            m_CenterPosition = center;
        }

        public void UpdateDataFromModel()
        {
            var model = m_Item.Model;
            VisualElement.transform.position = new Vector2(m_CenterPosition.x + model.RelativePosition.x - model.Pivot.x * model.Size.x, m_CenterPosition.y + model.RelativePosition.y + model.Pivot.y * model.Size.y);
            VisualElement.Q<Label>().text = model.DisplayName;
        }

        void PointerUpEventHandler(PointerUpEvent evt)
        {
            evt.StopImmediatePropagation();
            evt.PreventDefault();
            Debug.Log("PointerUpEvent");

            OnClicked?.Invoke();
        }

        void PointerDownEventHandler(PointerDownEvent evt)
        {
            evt.StopImmediatePropagation();
            evt.PreventDefault();
            Debug.Log("PointerDownEvent");
        }

        void MouseUpEventHandler(MouseUpEvent evt)
        {
            evt.StopImmediatePropagation();
            evt.PreventDefault();
            Debug.Log("MouseUpEventHandler");
        }
    }
}
