using System;
using Cysharp.Threading.Tasks;
using Xi.Framework;

namespace Xi.Metagame.Ui
{
    public class SelectMapWindowController : UiBaseController<SelectMapWindow>
    {
        protected override UiEnum UiEnum => UiEnum.Metagame_SelectMap;
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_MetagameUi, UiFeatureNameConst.kMetagame_SelectMap, UiPrefabNameConst.kMetagame_SelectMap);
        protected override bool IsOverlayMode => false;

        protected override void CleanControllerDependency() { }
        protected override void OnCloseAccomplishCallback() { }
        protected override void OnOpenAccomplishCallback() { }

        public void Open() => OpenAsync().Forget();
    }
}
