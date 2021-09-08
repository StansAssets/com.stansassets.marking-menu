using JetBrains.Annotations;
using UnityEditor;

namespace StansAssets.MarkingMenu
{
    public class MarkingMenuEditorMenu
    {
        [MenuItem(MarkingMenuPackage.RootMenu + "Settings", false, 0), UsedImplicitly]
        public static void OpenSettings()
        {
            var windowTitle = MarkingMenuSettingsWindow.WindowTitle;
            MarkingMenuSettingsWindow.ShowTowardsInspector(windowTitle.text, windowTitle.image);
        }
    }
}