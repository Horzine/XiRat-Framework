using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_AssetMgr : MonoBehaviour
    {
        private const string kGroupName = "Test Case";
        private const string kCubeName = "Cube";
        private AsyncOperationHandle _operationHandle;

        private async void Start()
        {
            //await TestLoadAsset();
            var cube = await TestInstantiateScript();
            cube.Test();
        }

        private async UniTaskVoid TestLoadAsset()
        {
            var (asset, operationHandle) = await AssetManager.Instance.LoadAssetAsync<GameObject>($"AssetMgr/Cube.prefab");
            _operationHandle = operationHandle;
            print(asset.name);
        }

        private async UniTask<Test_AssetMgr_Cube> TestInstantiateScript() => await AssetManager.Instance.InstantiateScriptAsync<Test_AssetMgr_Cube>($"AssetMgr/Cube.prefab", Vector3.zero, Quaternion.identity);

        private void OnDestroy()
        {
            if (AssetManager.Instance && _operationHandle.IsValid())
            {
                AssetManager.Instance.Release(_operationHandle);
            }
        }
    }
}
