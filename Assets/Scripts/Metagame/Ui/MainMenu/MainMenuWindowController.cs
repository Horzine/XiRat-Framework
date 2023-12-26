﻿using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;
using Xi.Metagame.Client.System.User;

namespace Xi.Metagame.Ui
{
    public class MainMenuWindowController : UiBaseController<MainMenuWindow>,
        ISystemObserver_User
    {
        protected override UiEnum UiEnum => UiEnum.Metagame_MainMenu;
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_MetagameUi, UiFeatureNameConst.kMetagame_MainMenu, UiPrefabNameConst.kMetagame_MainMenu);
        protected override bool IsOverlayMode => false;
        private MetagameSystem_User _userSystem;

        public void Open(MetagameSystem_User userSystem)
        {
            _userSystem = userSystem;
            OpenAsync().Forget();
        }

        public void Close() => CloseAsync().Forget();

        protected override void OnOpenAccomplishCallback()
        {
            _userSystem.AddObserver(this);
            WindowObj.AddCallback(SelectMapBtnCallback, ClassBuildBtnCallback, ClaimUserTestIntStrCallback, RandomTestIntCallback);
            WindowObj.Refresh();
        }

        protected override void CleanControllerDependency()
        {
            _userSystem.RemoveObserver(this);
            _userSystem = null;
            WindowObj.CleanCallback();
        }

        protected override void OnCloseAccomplishCallback() { }

        private void SelectMapBtnCallback()
        {
            var selectMapCtrl = UiManager.Instance.GetController<SelectMapWindowController>(UiEnum.Metagame_SelectMap);
            selectMapCtrl.Open();
        }

        private void ClassBuildBtnCallback()
        {
            var classBuildCtrl = UiManager.Instance.GetController<ClassBuildWindowController>(UiEnum.Metagame_ClassBuild);
            classBuildCtrl.Open();
        }

        private string ClaimUserTestIntStrCallback() => _userSystem == null ? string.Empty : _userSystem.GetTestInt().ToString();

        private void RandomTestIntCallback() => _userSystem?.SetupTestInt(Random.Range(0, 1000));

        void ISystemObserver_User.TestIntChange(int num) => WindowObj.Refresh();
    }
}
