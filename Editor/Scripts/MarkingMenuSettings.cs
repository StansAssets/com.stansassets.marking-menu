using StansAssets.Foundation.Editor;
using StansAssets.Foundation.Patterns;

namespace StansAssets.MarkingMenu
{
    public class MarkingMenuSettings : PackageScriptableSettingsSingleton<MarkingMenuSettings>
    {
        public override string PackageName => "com.stansassets.marking-menu";
        public override string SettingsLocations => FoundationConstants.EditorResourcesFolder;
    }
}
