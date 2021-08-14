using StansAssets.Plugins;

namespace StansAssets.MarkingMenu
{
    public class MarkingMenuSettings : PackageScriptableSettingsSingleton<MarkingMenuSettings>
    {
        public override string PackageName => MarkingMenuPackage.PackageName;
        protected override bool IsEditorOnly => true;

        public bool SceneViewMenuActive;
    }
}
