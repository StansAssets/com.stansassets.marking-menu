using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;

namespace StansAssets.MarkingMenu
{
    internal static class MarkingMenuPackage
    {
        public const string PackageName = "com.stansassets.marking-menu";
        public const string DisplayName = "Marking Menu";
        public const string RootMenu = PluginsDevKitPackage.RootMenu + "/" + DisplayName + "/";

        public static readonly string RootPath = PackageManagerUtility.GetPackageRootPath(PackageName);

#if UNITY_2019_4_OR_NEWER
        public static readonly UnityEditor.PackageManager.PackageInfo Info = PackageManagerUtility.GetPackageInfo(PackageName);
#endif

        internal static readonly string WindowTabsPath = $"{RootPath}/Editor/Window/Tabs";
    }
}
