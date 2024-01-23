using System.Threading;
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

        private void Awake() => AssetManager.Instance.InitAsync(GameSceneManager.Instance).Forget();

        private void Start()
        {
            TestLoadAsset().Forget();

            TestInstantiateScript().Forget();
        }

        private async UniTask TestLoadAsset()
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

        private async UniTask TestInstantiateScript()
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

        private void OnDestroy()
        {
            if (AssetManager.Instance && _operationHandle.IsValid())
            {
                AssetManager.Instance.Release(_operationHandle);
            }
        }
    }
}
