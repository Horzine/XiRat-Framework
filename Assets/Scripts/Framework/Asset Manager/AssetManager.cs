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
        private GameSceneManager _gameSceneManager;

        void ISingleton.OnCreate() { }

        public async UniTask InitAsync(GameSceneManager gameSceneManager)
        {
            _gameSceneManager = gameSceneManager;
            await Addressables.InitializeAsync(true);
        }

        public async UniTask<(bool success, TObject asset, AsyncOperationHandle operationHandle)> LoadAssetAsync<TObject>(string key,
            CancellationToken cancellationToken,
            bool currentActiveSceneOnly = true)
        {
            string lastSceneName = string.Empty;
            if (currentActiveSceneOnly)
            {
                lastSceneName = _gameSceneManager.CurrentActiveSceneName;
            }

            var loadOperation = Addressables.LoadAssetAsync<TObject>(key);
            await loadOperation.WithCancellation(cancellationToken);
            var asset = loadOperation.Result;
            if (currentActiveSceneOnly)
            {
                string newSceneName = _gameSceneManager.CurrentActiveSceneName;
                if (lastSceneName != newSceneName)
                {
                    XiLogger.LogWarning($"CurrentActiveSceneName changed, lastSceneName: {lastSceneName}, currentSceneName = {newSceneName}, key = {key}");
                    Release(loadOperation);
                    return (false, default, default);
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                XiLogger.LogWarning($"CancellationToken IsCancellationRequested, key = {key}");
                Release(loadOperation);
                return (false, default, default);
            }

            return (true, asset, loadOperation);
        }

        public async UniTask<GameObject> InstantiateGameObjectAsync(string key,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            CancellationToken cancellationToken,
            bool currentActiveSceneOnly = true)
            => await InstantiateGameObjectAsync(key, new InstantiationParameters(position, rotation, parent), cancellationToken, currentActiveSceneOnly);

        public async UniTask<GameObject> InstantiateGameObjectAsync(string key,
            InstantiationParameters instantiateParameters,
            CancellationToken cancellationToken,
            bool currentActiveSceneOnly = true)
        {
            string lastSceneName = string.Empty;
            if (currentActiveSceneOnly)
            {
                lastSceneName = _gameSceneManager.CurrentActiveSceneName;
            }

            var loadOperation = Addressables.InstantiateAsync(key, instantiateParameters);
            await loadOperation.WithCancellation(cancellationToken);
            var asset = loadOperation.Result;
            if (currentActiveSceneOnly)
            {
                string newSceneName = _gameSceneManager.CurrentActiveSceneName;
                if (lastSceneName != newSceneName)
                {
                    XiLogger.LogWarning($"CurrentActiveSceneName changed, lastSceneName: {lastSceneName}, currentSceneName = {newSceneName}, key = {key}");
                    Release(loadOperation);
                    return null;
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                XiLogger.LogWarning($"CancellationToken IsCancellationRequested, key = {key}");
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
            CancellationToken cancellationToken,
            bool currentActiveSceneOnly = true) where TScript : MonoBehaviour
            => await InstantiateScriptAsync<TScript>(key, new InstantiationParameters(position, rotation, parent), cancellationToken, currentActiveSceneOnly);

        public async UniTask<TScript> InstantiateScriptAsync<TScript>(string key,
            InstantiationParameters instantiateParameters,
            CancellationToken cancellationToken,
            bool currentActiveSceneOnly = true) where TScript : MonoBehaviour
        {
            var go = await InstantiateGameObjectAsync(key, instantiateParameters, cancellationToken, currentActiveSceneOnly);
            if (!go)
            {
                XiLogger.LogError($"GameObject is null, key = {key}");
                return null;
            }

            if (!go.TryGetComponent<TScript>(out var script))
            {
                XiLogger.LogError($"No '{typeof(TScript)}' this component, key = {key}");
                return null;
            }

            return script;
        }

        public void Release(AsyncOperationHandle operationHandle)
        {
            if (!operationHandle.IsValid())
            {
                return;
            }

            Addressables.Release(operationHandle);
        }
    }
}
