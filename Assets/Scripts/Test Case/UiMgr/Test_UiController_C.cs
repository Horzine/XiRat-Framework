using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_UiController_C : UiBaseController<Test_UiWindow>
    {
        protected override UiEnum UiEnum => UiEnum.TestCase_C;
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_MetagameUi, UiFeatureNameConst.kSystem_C, UiPrefabNameConst.kSystem_C_name);
        protected override bool IsOverlayMode => false;
        public override void BeforeClose() { }
        public void Init_C() => Debug.Log(nameof(Test_UiController_C));

        public new async UniTask OpenAsync() => await base.OpenAsync();

        public override async UniTask CloseAsync()
        {
            if (!CanClose)
            {
                return;
            }

            BeforeClose();
            CurrentWindowState = WindowState.Closing;
            await UniTask.Delay(15 * 1000);
            await DoCloseAsync();
        }
    }
}
