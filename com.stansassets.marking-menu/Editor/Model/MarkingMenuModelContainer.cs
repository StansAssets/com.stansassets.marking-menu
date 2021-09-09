using StansAssets.MarkingMenu;
using StansAssets.Plugins;

namespace Editor.Model
{
    public class MarkingMenuModelContainer : PackageScriptableSettingsSingleton<MarkingMenuModelContainer>
    {
        public override string PackageName => MarkingMenuPackage.PackageName;
        protected override bool IsEditorOnly => true;

        public MarkingMenuModel MarkingMenuModel;
    }
}