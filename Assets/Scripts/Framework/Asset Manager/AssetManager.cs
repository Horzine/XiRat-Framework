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

        public void Init(GameSceneManager gameSceneManager)
        {
            _gameSceneManager = gameSceneManager;
            Addressables.InitializeAsync(true).WaitForCompletion();
        }

        #region Async 
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
            var (isCanceled, result) = await loadOperation.WithCancellation(cancellationToken).SuppressCancellationThrow();
            if (isCanceled)
            {
                XiLogger.LogWarning($"Async Operating Canceled! key = {key}, cancellationToken.IsCancellationRequested = {cancellationToken.IsCancellationRequested}");
                return (false, default, default);
            }

            var asset = result;
            if (currentActiveSceneOnly)
            {
                string newSceneName = _gameSceneManager.CurrentActiveSceneName;
                if (lastSceneName != newSceneName)
                {
                    XiLogger.LogWarning($"CurrentActiveSceneName changed! lastSceneName: {lastSceneName}, currentSceneName = {newSceneName}, key = {key}");
                    Release(loadOperation);
                    return (false, default, default);
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                XiLogger.LogWarning($"CancellationToken IsCancellationRequested! key = {key}");
                Release(loadOperation);
                return (false, default, default);
            }

            return (true, asset, loadOperation);
        }

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

            var operation = Addressables.InstantiateAsync(key, instantiateParameters);
            var (isCanceled, result) = await operation.WithCancellation(cancellationToken).SuppressCancellationThrow();
            if (isCanceled)
            {
                XiLogger.LogWarning($"Async Operating Canceled! key = {key}, cancellationToken.IsCancellationRequested = {cancellationToken.IsCancellationRequested}");
                Release(operation);
                return null;
            }

            var asset = result;
            if (currentActiveSceneOnly)
            {
                string newSceneName = _gameSceneManager.CurrentActiveSceneName;
                if (lastSceneName != newSceneName)
                {
                    XiLogger.LogWarning($"CurrentActiveSceneName changed! lastSceneName: {lastSceneName}, currentSceneName = {newSceneName}, key = {key}");
                    Release(operation);
                    return null;
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                XiLogger.LogWarning($"CancellationToken IsCancellationRequested! key = {key}");
                Release(operation);
                return null;
            }

            asset.AddComponent<AutoReleaseAsset>().Init(operation);
            return asset;
        }

        public async UniTask<TScript> InstantiateScriptAsync<TScript>(string key,
            InstantiationParameters instantiateParameters,
            CancellationToken cancellationToken,
            bool currentActiveSceneOnly = true) where TScript : MonoBehaviour
        {
            var go = await InstantiateGameObjectAsync(key, instantiateParameters, cancellationToken, currentActiveSceneOnly);
            if (!go)
            {
                XiLogger.LogError($"GameObject is null! key = {key}");
                return null;
            }

            if (!go.TryGetComponent<TScript>(out var script))
            {
                XiLogger.LogError($"No '{typeof(TScript)}' this component! key = {key}");
                return null;
            }

            return script;
        }
        #endregion

        #region Sync
        public (TObject asset, AsyncOperationHandle<TObject> operationHandle) LoadAsset<TObject>(string key)
        {
            var loadOperation = Addressables.LoadAssetAsync<TObject>(key);
            var asset = loadOperation.WaitForCompletion();
            return (asset, loadOperation);
        }

        public TScript InstantiateScript<TScript>(string key, InstantiationParameters instantiateParameters) where TScript : MonoBehaviour
        {
            var go = InstantiateGameObject(key, instantiateParameters);
            if (!go)
            {
                XiLogger.LogError($"GameObject is null! key = {key}");
                return null;
            }

            if (!go.TryGetComponent<TScript>(out var script))
            {
                XiLogger.LogError($"No '{typeof(TScript)}' this component! key = {key}");
                return null;
            }

            return script;
        }

        public GameObject InstantiateGameObject(string key, InstantiationParameters instantiateParameters)
        {
            var operation = Addressables.InstantiateAsync(key, instantiateParameters);
            var go = operation.WaitForCompletion();
            go.AddComponent<AutoReleaseAsset>().Init(operation);
            return go;
        }
        #endregion

        public bool Release(AsyncOperationHandle operationHandle)
        {
            if (!operationHandle.IsValid())
            {
                return false;
            }

            Addressables.Release(operationHandle);
            return true;
        }
    }

    public static class AssetManager_Extend
    {
        public static void Release(this AssetManager instance, ref GameObject goRef, AsyncOperationHandle operationHandle)
        {
            if (instance.Release(operationHandle))
            {
                goRef = null;
            }
        }

        public static async UniTask<GameObject> InstantiateGameObjectAsync(this AssetManager instance,
            string key,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            CancellationToken cancellationToken,
            bool currentActiveSceneOnly = true)
            => await instance.InstantiateGameObjectAsync(key, new InstantiationParameters(position, rotation, parent), cancellationToken, currentActiveSceneOnly);

        public static async UniTask<TScript> InstantiateScriptAsync<TScript>(this AssetManager instance,
            string key,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            CancellationToken cancellationToken,
            bool currentActiveSceneOnly = true) where TScript : MonoBehaviour
            => await instance.InstantiateScriptAsync<TScript>(key, new InstantiationParameters(position, rotation, parent), cancellationToken, currentActiveSceneOnly);

    }
}
