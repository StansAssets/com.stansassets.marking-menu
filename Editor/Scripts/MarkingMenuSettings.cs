using StansAssets.Plugins;

namespace StansAssets.MarkingMenu
{
    public class MarkingMenuSettings : PackageScriptableSettingsSingleton<MarkingMenuSettings>
    {
        public override string PackageName => "com.stansassets.marking-menu";
        protected override bool IsEditorOnly => true;
    }
}
