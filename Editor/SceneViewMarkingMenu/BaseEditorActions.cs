using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace StansAssets.MarkingMenu
{
    [CustomActionRegistration(true)] [UsedImplicitly]
    public class BaseEditorActions
    {
        static ItemType item = ItemType.Action;

        public static void Register(MarkingMenu menu)
        {
            menu.Register("Action1", () => EditorApplication.isPlaying = !EditorApplication.isPlaying);
            menu.Register("Action2", () => EditorApplication.Exit(0));
            menu.Register("Action3", () => EditorApplication.Beep());
            menu.Register("Action4", () => EditorGUIUtility.PingObject(Selection.activeObject));
            menu.Register("Action5", new ToggleContext((s) => { EditorApplication.isPlaying = s; }, () => { return EditorApplication.isPlaying;}));
            menu.Register("Action6", new ToggleMenuContext((s) => { Enum.TryParse(s, out item); }, () => { return new ToggleMenuContextModel(Enum.GetNames(typeof(ItemType)), item.ToString());}));
        }
    }
}
