using StansAssets.Foundation.Patterns;
using UnityEngine;

namespace StansAssets.MarkingMenu
{
    public class MarkingMenuSettings : PackageScriptableSettingsSingleton<MarkingMenuSettings>
    {
        public override string PackageName => "com.stansassets.marking-menu";
        public override string SettingsLocations => $"Assets/Settings/";
    }
}
