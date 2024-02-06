using UnityEngine;
using Xi.Framework;
using Xi.Metagame.Client.System.User;
using Xi.Metagame.Feature;
using static Xi.Metagame.Ui.MainMenuWindowController;

namespace Xi.Metagame.Ui
{
    public class MainMenuWindowController : UiBaseController<MainMenuWindow, InitParams>,
        ISystemObserver_User
    {
        public struct InitParams : IUiInitParams
        {
            public MetagameSystem_User UserSystem { get; set; }
            public MetagameFeatureController FeatureController { get; set; }
        }
        protected override UiEnum UiEnumValue => UiEnum.Metagame_MainMenu;
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_MetagameUi, UiFeatureNameConst.kMetagame_MainMenu, UiPrefabNameConst.kMetagame_MainMenu);
        protected override bool IsOverlayMode => false;

        protected override void OnWindowInstantiateCallback() => CachedInitParams.UserSystem.AddObserver(this);

        protected override void OnOpenAccomplishCallback()
        {
            WindowObj.AddCallback(SelectMapBtnCallback, ClassBuildBtnCallback, ClaimUserTestIntStrCallback, RandomTestIntCallback);
            WindowObj.Refresh();
        }

        protected override void OnCloseAccomplishCallback() => WindowObj.CleanCallback();

        protected override void OnWindowDestoryCallback() => CachedInitParams.UserSystem.RemoveObserver(this);

        private void SelectMapBtnCallback()
        {
            if (CachedInitParams.FeatureController)
            {
                CachedInitParams.FeatureController.ActiveFeature(MetagameFeatureEnum.Mission);
            }
        }

        private void ClassBuildBtnCallback()
        {
            if (CachedInitParams.FeatureController)
            {
                CachedInitParams.FeatureController.ActiveFeature(MetagameFeatureEnum.Arsenal);
            }
        }

        private string ClaimUserTestIntStrCallback()
            => CachedInitParams.UserSystem == null ? string.Empty : CachedInitParams.UserSystem.GetTestInt().ToString();

        private void RandomTestIntCallback()
            => CachedInitParams.UserSystem?.SetupTestInt(Random.Range(0, 1000));

        void ISystemObserver_User.TestIntChange(int num) => WindowObj.Refresh();
    }
}
