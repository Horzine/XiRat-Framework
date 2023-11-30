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

        private void Awake() => AssetManager.Instance.InitAsync().Forget();

        private void Start()
        {
            TestLoadAsset().Forget();

            TestInstantiateScript().Forget();
        }

        private async UniTask TestLoadAsset()
        {
            var (success, asset, operationHandle) = await AssetManager.Instance.LoadAssetAsync<GameObject>($"{kGroupName}/AssetMgr/{kCubeName}.prefab", this.GetCancellationTokenOnDestroy());
            if (!success)
            {
                return;
            }

            _operationHandle = operationHandle;
            print(asset.name);
        }

        private async UniTask TestInstantiateScript()
        {
            var cube = await AssetManager.Instance.InstantiateScriptAsync<Test_AssetMgr_Cube>($"{kGroupName}/AssetMgr/{kCubeName}.prefab", Vector3.zero, Quaternion.identity, null, this.GetCancellationTokenOnDestroy());
            if (cube)
            {
                cube.Test();
            }
        }

        private void OnDestroy()
        {
            if (AssetManager.Instance && _operationHandle.IsValid())
            {
                AssetManager.Instance.Release(_operationHandle);
            }
        }
    }
}
