using System;
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
            _createMetagameGameInstanceFunc = createMetagameGameInstanceFunc;
            _createGameplayGameInstanceFunc = createGameplayGameInstanceFunc;
            _gameSceneManager = gameSceneManager;
            await UniTask.Yield();
        }

        public async UniTaskVoid ChangeSceneToMetagameScene() => await ChangeSceneToGameInstance(_createMetagameGameInstanceFunc);

        public async UniTaskVoid ChangeSceneToGameplayScene(string sceneName) => await ChangeSceneToGameInstance(_createGameplayGameInstanceFunc, sceneName);

        public void RunGC()
        {
            GC.Collect();
            XiLogger.Log(string.Empty);
        }

        private async UniTask ChangeSceneToGameInstance(Func<IGameInstance> createGameInstanceFunc, string sceneName = null)
        {
            var newGameInstance = createGameInstanceFunc?.Invoke();
            if (!string.IsNullOrEmpty(sceneName))
            {
                newGameInstance.SceneName = sceneName;
            }

            newGameInstance.OnCreate();
            var op = await _gameSceneManager.LoadActiveSceneAsync(newGameInstance.SceneName, false);
            await op.Result.ActivateAsync();
            newGameInstance.OnNewSceneActive(CurrentGameInstance);
            CurrentGameInstance?.WillBeReplaced();
            CurrentGameInstance = newGameInstance;
            RunGC();
        }
    }
}
