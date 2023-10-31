using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Xi.Framework
{
    public class AssetManager : MonoSingleton<AssetManager>, ISingleton
    {
        void ISingleton.OnCreate() => Addressables.InitializeAsync(true);

        public async UniTask<(TObject asset, AsyncOperationHandle operationHandle)> LoadAssetAsync<TObject>(string key)
        {
            var loadOperation = Addressables.LoadAssetAsync<TObject>(key);
            await loadOperation;
            var asset = loadOperation.Result;
            return (asset, loadOperation);
        }

        public async UniTask<GameObject> InstantiateGameObjectAsync(string key, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var loadOperation = Addressables.InstantiateAsync(key, position, rotation, parent);
            await loadOperation;
            var asset = loadOperation.Result;
            asset.AddComponent<AutoReleaseAsset>().Init(loadOperation);
            return asset;
        }

        public async UniTask<GameObject> InstantiateGameObjectAsync(string key, InstantiationParameters instantiateParameters)
        {
            var loadOperation = Addressables.InstantiateAsync(key, instantiateParameters);
            await loadOperation;
            var asset = loadOperation.Result;
            asset.AddComponent<AutoReleaseAsset>().Init(loadOperation);
            return asset;
        }

        public async UniTask<TScript> InstantiateScriptAsync<TScript>(string key, Vector3 position, Quaternion rotation, Transform parent = null) where TScript : MonoBehaviour
        {
            var go = await InstantiateGameObjectAsync(key, position, rotation, parent);
            return go.GetComponent<TScript>();
        }

        public async UniTask<TScript> InstantiateScriptAsync<TScript>(string key, InstantiationParameters instantiateParameters) where TScript : MonoBehaviour
        {
            var go = await InstantiateGameObjectAsync(key, instantiateParameters);
            return go.GetComponent<TScript>();
        }

        public void Release(AsyncOperationHandle operationHandle) => Addressables.Release(operationHandle);
    }
}
