using Xi.Framework;
using static Xi.Metagame.Ui.SelectMapWindowController;

namespace Xi.Metagame.Ui
{
    public class SelectMapWindowController : UiBaseController<SelectMapWindow, InitParams>
    {
        public struct InitParams : IUiInitParams
        {

        }
        protected override UiEnum UiEnumValue => UiEnum.Metagame_SelectMap;
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_MetagameUi, UiFeatureNameConst.kMetagame_SelectMap, UiPrefabNameConst.kMetagame_SelectMap);
        protected override bool IsOverlayMode => false;

        protected override void OnCloseAccomplishCallback() { }
        protected override void OnOpenAccomplishCallback() { }
        protected override void OnWindowInstantiateCallback() { }
        protected override void OnWindowDestoryCallback() { }
    }
}
