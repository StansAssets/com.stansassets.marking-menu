using System;
using System.Collections.Generic;
using UnityEngine;

namespace StansAssets.MarkingMenuB {
    public interface IMarkingMenu
    {
        event Action OnOpened;
        event Action OnClosed;

        bool Active { get; }
        Vector2 Center { get; }
        IList<IMarkingMenuItem> Items { get; }

        void Init(MarkingMenuModel model);
        void Open(Vector2 center);
        void Close();

        void Register(string id, Action action);
        void Register(string id, ToggleContext ctx);

        bool DebugMode { get; set; }
    }
}
