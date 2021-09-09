using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    public class MarkingMenuSettingsWindow : PackageSettingsWindow<MarkingMenuSettingsWindow>
    {
        protected override PackageInfo GetPackageInfo()
            => PackageManagerUtility.GetPackageInfo(MarkingMenuSettings.Instance.PackageName);

        protected override void OnWindowEnable(VisualElement root)
        {
            AddTab("Settings", new SettingsTab());
            AddTab("About", new AboutTab());
        }

        public static GUIContent WindowTitle => new GUIContent(MarkingMenuPackage.DisplayName);
    }
}