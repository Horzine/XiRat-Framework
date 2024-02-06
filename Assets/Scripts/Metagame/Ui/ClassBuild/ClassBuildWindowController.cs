using Xi.Framework;
using static Xi.Metagame.Ui.ClassBuildWindowController;

namespace Xi.Metagame.Ui
{
    public class ClassBuildWindowController : UiBaseController<ClassBuildWindow, InitParams>
    {
        public struct InitParams : IUiInitParams
        {

        }
        protected override UiEnum UiEnumValue => UiEnum.Metagame_ClassBuild;
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_MetagameUi, UiFeatureNameConst.kMetagame_ClassBuild, UiPrefabNameConst.kMetagame_ClassBuild);
        protected override bool IsOverlayMode => false;

        protected override void OnCloseAccomplishCallback() { }
        protected override void OnOpenAccomplishCallback() { }
        protected override void OnWindowInstantiateCallback() { }
        protected override void OnWindowDestoryCallback() { }
    }
}
