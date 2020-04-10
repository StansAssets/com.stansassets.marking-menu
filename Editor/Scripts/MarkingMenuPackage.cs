using StansAssets.Foundation.Editor;
using UnityEditor.PackageManager;

namespace StansAssets.MarkingMenu
{
    internal static class MarkingMenuPackage
    {
        public static readonly string RootPath = PackageManagerUtility.GetPackageRootPath(MarkingMenuSettings.Instance.PackageName);
        public static readonly PackageInfo Info = PackageManagerUtility.GetPackageInfo(MarkingMenuSettings.Instance.PackageName);

        public static readonly string MenuUIPath = $"{RootPath}/UI";
        public static readonly string MenuItemsPath = $"{MenuUIPath}/Items";
    }
}
