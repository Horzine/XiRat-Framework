using Cysharp.Threading.Tasks;
using Xi.Framework;

namespace Xi.Metagame.Ui
{
    public class MainMenuWindowController : UiBaseController<MainMenuWindow>
    {
        protected override UiEnum UiEnum => UiEnum.Metagame_MainMenu;
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_MetagameUi, UiFeatureNameConst.kMetagame_MainMenu, UiPrefabNameConst.kMetagame_MainMenu);
        protected override bool IsOverlayMode => false;

        public void Open() => OpenAsync().Forget();

        public void Close() => CloseAsync().Forget();

        protected override void OnOpenAccomplishCallback() => WindowObj.AddCallback(SelectMapBtnCallback, ClassBuildBtnCallback);

        protected override void CleanControllerDependency() => WindowObj.CleanCallback();

        protected override void OnCloseAccomplishCallback() { }

        public void SelectMapBtnCallback()
        {
            var selectMapCtrl = UiManager.Instance.GetController<SelectMapWindowController>(UiEnum.Metagame_MapSelect);
            selectMapCtrl.Open();
        }

        public void ClassBuildBtnCallback()
        {
            var classBuildCtrl = UiManager.Instance.GetController<ClassBuildWindowController>(UiEnum.Metagame_ClassBuild);
            classBuildCtrl.Open();
        }
    }
}
