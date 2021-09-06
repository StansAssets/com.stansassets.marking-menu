using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace StansAssets.MarkingMenu
{
    [CustomActionRegistration(true)] [UsedImplicitly]
    public sealed class BaseEditorActions
    {
        static ItemType item = ItemType.Action;

        public static void Register(MarkingMenu menu)
        {
            menu.Register("Play", () => EditorApplication.isPlaying = !EditorApplication.isPlaying);
            menu.Register("Build Settings", () => EditorApplication.ExecuteMenuItem("File/Build Settings"));
            menu.Register("Player Settings", () => EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player"));
            menu.Register("Marking Menu Settings", () => EditorApplication.ExecuteMenuItem(MarkingMenuPackage.RootMenu + "Settings"));
            menu.Register("GitHub Page", () => Application.OpenURL("https://github.com/StansAssets/com.stansassets.marking-menu"));
        }
    }
}
