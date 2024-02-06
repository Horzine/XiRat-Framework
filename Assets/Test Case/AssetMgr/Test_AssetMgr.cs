using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_AssetMgr : MonoBehaviour
    {
        private const string kGroupName = "Test Case";
        private const string kCubeName = "Cube";
        private AsyncOperationHandle _operationHandle;

        private void Awake() => AssetManager.Instance.InitAsync(GameSceneManager.Instance).Forget();

        private async void Start()
        {
            TestLoadAssetAsync().Forget();

            TestInstantiateScriptAsync().Forget();

            TestLoadAsset();

            TestInstantiateScript();

            await UniTask.Yield();
        }

        private async UniTask TestLoadAssetAsync()
        {
            var tokenSource = new CancellationTokenSource();
            var op = AssetManager.Instance.LoadAssetAsync<GameObject>($"{kGroupName}/AssetMgr/{kCubeName}.prefab", tokenSource.Token);
            //tokenSource.Cancel();
            var (success, asset, operationHandle) = await op;
            if (!success)
            {
                return;
            }

            _operationHandle = operationHandle;
            print(asset.name);
        }

        private async UniTask TestInstantiateScriptAsync()
        {
            var tokenSource = new CancellationTokenSource();
            var op = AssetManager.Instance.InstantiateScriptAsync<Test_AssetMgr_Cube>($"{kGroupName}/AssetMgr/{kCubeName}.prefab", Vector3.zero, Quaternion.identity, null, tokenSource.Token);
            //tokenSource.Cancel();
            var cube = await op;
            if (cube)
            {
                cube.Test();
            }
        }

        private GameObject _testLoadAssetAsset;

        private void TestLoadAsset()
        {
            var (asset, operationHandle) = AssetManager.Instance.LoadAsset<GameObject>($"{kGroupName}/AssetMgr/{kCubeName}.prefab");
            asset.name = nameof(TestLoadAsset);
            _testLoadAssetAsset = asset;
            Instantiate(asset);
            AssetManager.Instance.Release(ref _testLoadAssetAsset, operationHandle);
            if (_testLoadAssetAsset)
            {
                Instantiate(asset);
            }
        }

        private void TestInstantiateScript()
        {
            var cube = AssetManager.Instance.InstantiateScript<Test_AssetMgr_Cube>($"{kGroupName}/AssetMgr/{kCubeName}.prefab", new InstantiationParameters());
            cube.name = nameof(TestInstantiateScript);
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
