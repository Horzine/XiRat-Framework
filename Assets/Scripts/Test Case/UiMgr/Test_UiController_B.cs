using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_UiController_B : UiBaseController<Test_UiWindow>
    {
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_GameplayUi, UiFeatureNameConst.kSystem_B, UiPrefabNameConst.kSystem_B_name);
        protected override UiEnum UiEnum => UiEnum.TestCase_B;
        protected override bool IsOverlayMode => true;
        protected override void CleanControllerDependency() { }
        public void Init_B() => Debug.Log(nameof(Test_UiController_B));
        public new async UniTask OpenAsync()
        {
            if (!CanOpen)
            {
                return;
            }

            CurrentWindowState = WindowState.Opening;
            await UniTask.Delay(15 * 1000);
            await DoOpenAsync();
        }
        protected override void OnCloseAccomplishCallback() { }
        protected override void OnOpenAccomplishCallback() { }
    }
}
