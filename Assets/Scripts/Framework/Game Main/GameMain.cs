﻿using System;
using Cysharp.Threading.Tasks;
using Xi.Tools;

namespace Xi.Framework
{
    public class GameMain : MonoSingleton<GameMain>, ISingleton
    {
        public IGameInstance CurrentGameInstance { get; private set; }
        private Func<IGameInstance> _createMetagameGameInstanceFunc;
        private Func<IGameInstance> _createGameplayGameInstanceFunc;
        private GameSceneManager _gameSceneManager;

        void ISingleton.OnCreate()
        {

        }

        public async UniTask InitAsync(GameSceneManager gameSceneManager,
            Func<IGameInstance> createMetagameGameInstanceFunc,
            Func<IGameInstance> createGameplayGameInstanceFunc)
        {
            Init(gameSceneManager, createMetagameGameInstanceFunc, createGameplayGameInstanceFunc);
            await UniTask.Yield();
        }

        public void Init(GameSceneManager gameSceneManager,
            Func<IGameInstance> createMetagameGameInstanceFunc,
            Func<IGameInstance> createGameplayGameInstanceFunc)
        {
            _createMetagameGameInstanceFunc = createMetagameGameInstanceFunc;
            _createGameplayGameInstanceFunc = createGameplayGameInstanceFunc;
            _gameSceneManager = gameSceneManager;
        }

        public async UniTaskVoid ChangeSceneToMetagameScene() => await DoChangeSceneToGameInstance(_createMetagameGameInstanceFunc);

        public async UniTaskVoid ChangeSceneToGameplayScene(string sceneName) => await DoChangeSceneToGameInstance(_createGameplayGameInstanceFunc, sceneName);

        public void RunGC()
        {
            GC.Collect();
            XiLogger.Log(string.Empty);
        }

        private async UniTask DoChangeSceneToGameInstance(Func<IGameInstance> createGameInstanceFunc, string sceneName = null)
        {
            var newGameInstance = createGameInstanceFunc?.Invoke();
            if (!string.IsNullOrEmpty(sceneName))
            {
                newGameInstance.SceneName = sceneName;
            }

            newGameInstance.OnCreate();
            var op = await _gameSceneManager.LoadActiveSceneAsync(newGameInstance.SceneName, false);
            await op.Result.ActivateAsync();// Will call all new scene 'Awake' here
            newGameInstance.OnAfterNewSceneActive(CurrentGameInstance);
            CurrentGameInstance?.WillBeReplaced();
            CurrentGameInstance = newGameInstance;
            RunGC();
        }
    }
}
