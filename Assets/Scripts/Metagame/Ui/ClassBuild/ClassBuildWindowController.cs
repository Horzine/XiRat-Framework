using Xi.Framework;
using Xi.Tools;
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

        protected override void OnCloseAccomplishCallback() => WindowObj.RemoveCallback();
        protected override void OnOpenAccomplishCallback() => WindowObj.AddCallback();
        protected override void OnWindowInstantiateCallback() => XiLogger.CallMark();
        protected override void OnWindowDestoryCallback() => XiLogger.CallMark();
    }
}
