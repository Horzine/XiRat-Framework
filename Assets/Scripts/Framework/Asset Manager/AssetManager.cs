using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Xi.Tools;

namespace Xi.Framework
{
    public class AssetManager : MonoSingleton<AssetManager>, ISingleton
    {
        void ISingleton.OnCreate() { }

        public async UniTask InitAsync() => await Addressables.InitializeAsync(true);

        public async UniTask<(bool success, TObject asset, AsyncOperationHandle operationHandle)> LoadAssetAsync<TObject>(string key,
            CancellationToken cancellationToken)
        {
            var loadOperation = Addressables.LoadAssetAsync<TObject>(key);
            await loadOperation;
            var asset = loadOperation.Result;
            if (cancellationToken.IsCancellationRequested)
            {
                XiLogger.LogWarning($"cancellationToken IsCancellationRequested, key = {key}");
                Release(loadOperation);
                return (false, default, default);
            }

            return (true, asset, loadOperation);
        }

        public async UniTask<GameObject> InstantiateGameObjectAsync(string key,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            CancellationToken cancellationToken)
            => await InstantiateGameObjectAsync(key, new InstantiationParameters(position, rotation, parent), cancellationToken);

        public async UniTask<GameObject> InstantiateGameObjectAsync(string key,
            InstantiationParameters instantiateParameters,
            CancellationToken cancellationToken)
        {
            var loadOperation = Addressables.InstantiateAsync(key, instantiateParameters);
            await loadOperation;
            var asset = loadOperation.Result;
            if (cancellationToken.IsCancellationRequested)
            {
                XiLogger.LogWarning($"cancellationToken IsCancellationRequested, key = {key}");
                Destroy(asset);
                Release(loadOperation);
                return null;
            }

            asset.AddComponent<AutoReleaseAsset>().Init(loadOperation);
            return asset;
        }

        public async UniTask<TScript> InstantiateScriptAsync<TScript>(string key,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            CancellationToken cancellationToken) where TScript : MonoBehaviour
            => await InstantiateScriptAsync<TScript>(key, new InstantiationParameters(position, rotation, parent), cancellationToken);

        public async UniTask<TScript> InstantiateScriptAsync<TScript>(string key,
            InstantiationParameters instantiateParameters,
            CancellationToken cancellationToken) where TScript : MonoBehaviour
        {
            var go = await InstantiateGameObjectAsync(key, instantiateParameters, cancellationToken);
            return go ? go.GetComponent<TScript>() : null;
        }

        public void Release(AsyncOperationHandle operationHandle) => Addressables.Release(operationHandle);
    }
}
