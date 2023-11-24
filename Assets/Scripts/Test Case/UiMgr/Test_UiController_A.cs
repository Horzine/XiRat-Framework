using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_UiController_A : UiBaseController<Test_UiWindow>
    {
        protected override (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath
            => (AssetGroupNameConst.kAddressableGroupName_GameplayUi, UiFeatureNameConst.kSystem_A, UiPrefabNameConst.kSystem_A_name);
        protected override UiEnum UiEnum => UiEnum.TestCase_A;
        public override void BeforeClose() { }
        public void Init_A() => Debug.Log(nameof(Test_UiController_A));
        public new async UniTask OpenAsync()
        {
            if (!CanOpen)
            {
                return;
            }

            await base.OpenAsync();
        }
    }
}
