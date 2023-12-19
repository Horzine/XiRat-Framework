using Cysharp.Threading.Tasks;
using Xi.Framework;

namespace Xi.Metagame.Ui
{
    public class ClassBuildWindowController : UiBaseController<ClassBuildWindow>
    {
        protected override UiEnum UiEnum => UiEnum.Metagame_ClassBuild;
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_MetagameUi, UiFeatureNameConst.kMetagame_ClassBuild, UiPrefabNameConst.kMetagame_ClassBuild);
        protected override bool IsOverlayMode => false;

        protected override void CleanControllerDependency() { }
        protected override void OnCloseAccomplishCallback() { }
        protected override void OnOpenAccomplishCallback() { }

        public void Open() => OpenAsync().Forget();
    }
}
