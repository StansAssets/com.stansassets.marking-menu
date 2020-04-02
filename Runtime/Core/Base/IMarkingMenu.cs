using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu {
    public interface IMarkingMenu
    {
        event Action OnOpened;
        event Action OnClosed;

        bool Active { get; }
        VisualElement Parent { get; }
        VisualElement Root { get; }
        Vector2 Center { get; }

        IList<IMarkingMenuItem> Items { get; }

        void Init(MarkingMenuModel model);
        void Open(VisualElement root, Vector2 center);
        void Close();

        void SendEvent(EventBase e);

        void Register(string id, Action action);
        void Register(string id, ToggleContext ctx);
    }
}
