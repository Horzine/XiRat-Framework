using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Xi.Framework
{
    public class GameSceneManager : MonoSingleton<GameSceneManager>, ISingleton
    {
        public IReadOnlyDictionary<string, Scene> SubScenes => _subScenes;
        private readonly Dictionary<string, Scene> _subScenes = new();
        public string CurrentActiveSceneName { get; private set; }

        void ISingleton.OnCreate()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChangedAsync;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChangedAsync;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (_subScenes.ContainsKey(scene.name))
            {
                _subScenes.Remove(scene.name);
            }
        }

        private void OnActiveSceneChangedAsync(Scene oldScene, Scene newScene)
        {
            string newSceneName = newScene.name;
            string oldSceneName = oldScene.name;
            CurrentActiveSceneName = newSceneName;

            if (!string.IsNullOrEmpty(oldSceneName) && !_subScenes.ContainsKey(oldSceneName))
            {
                _subScenes.Add(oldSceneName, oldScene);
            }

            if (_subScenes.ContainsKey(newSceneName))
            {
                _subScenes.Remove(newSceneName);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Additive)
            {
                _subScenes.Add(scene.name, scene);
            }
        }

        public async UniTask<AsyncOperationHandle<SceneInstance>> LoadSubSceneAsync(string sceneName, bool autoActivateOnLoad = false)
            => await LoadSceneAsync(sceneName, true, autoActivateOnLoad);

        public async UniTask<AsyncOperationHandle<SceneInstance>> LoadActiveSceneAsync(string sceneName, bool autoActivateOnLoad = false)
            => await LoadSceneAsync(sceneName, false, autoActivateOnLoad);

        private async UniTask<AsyncOperationHandle<SceneInstance>> LoadSceneAsync(string sceneName, bool isAdditive, bool autoActivateOnLoad)
        {
            float startTime = Time.realtimeSinceStartup;
            var op = Addressables.LoadSceneAsync(SceneNameConst.SceneAddressableName(sceneName), isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single, activateOnLoad: autoActivateOnLoad);
            await op;
            LoadSceneAsyncOnCompleted(sceneName, isAdditive, op, startTime);
            return op;
        }

        public async UniTask UnloadSubSceneAsync(AsyncOperationHandle<SceneInstance> op)
        {
            if (op.IsValid() && SubScenes.ContainsKey(op.Result.Scene.name))
            {
                await Addressables.UnloadSceneAsync(op);
            }
        }

        public void SetActiveScene(AsyncOperationHandle<SceneInstance> op)
        {
            if (op.IsValid())
            {
                SceneManager.SetActiveScene(op.Result.Scene);
            }
        }

        private void LoadSceneAsyncOnCompleted(string sceneName, bool isAdditive, AsyncOperationHandle<SceneInstance> op, float startTime)
            => Debug.Log($"[{nameof(GameSceneManager)}]<{nameof(LoadSceneAsyncOnCompleted)}>:SceneName: {sceneName}, IsAdditive: {isAdditive}, TimePassed: {Time.realtimeSinceStartup - startTime}");
    }
}
