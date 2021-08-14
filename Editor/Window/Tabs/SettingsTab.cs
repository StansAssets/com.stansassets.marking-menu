#if UNITY_2019_4_OR_NEWER
using StansAssets.Plugins.Editor;
using UnityEngine.UIElements;

namespace StansAssets.MarkingMenu
{
    public class SettingsTab : BaseTab
    {
        public SettingsTab()
            : base($"{MarkingMenuPackage.WindowTabsPath}/SettingsTab")
        {
            var landingSceneField = Root.Q<Toggle>("scene-view-menu");
            landingSceneField.SetValueWithoutNotify(MarkingMenuSettings.Instance.SceneViewMenuActive);

            landingSceneField.RegisterValueChangedCallback((e) =>
            {
                MarkingMenuSettings.Instance.SceneViewMenuActive = e.newValue;
                MarkingMenuSettings.Save();
            });
        }
    }
}
#endif