using System;
using Cysharp.Threading.Tasks;

namespace Xi.Framework
{
    public interface IGameInstance
    {
        string SceneName { get; set; }
        void OnCreate();
        void OnSceneActive(IGameInstance oldGameInstance);
    }
    public abstract class GameInstance : IGameInstance
    {
        public abstract string SceneName { get; set; }
        public abstract void OnCreate();
        public abstract void OnSceneActive(IGameInstance oldGameInstance);
    }
    public partial class GameMain : MonoSingleton<GameMain>, ISingleton
    {
        void ISingleton.OnCreate()
        {

        }
        public IGameInstance CurrentGameInstance { get; private set; }
        private Func<IGameInstance> _createMetagameGameInstanceFunc;
        private Func<IGameInstance> _createGameplayGameInstanceFunc;
        private GameSceneManager _gameSceneManager;

        public async UniTask InitAsync(GameSceneManager gameSceneManager,
            Func<IGameInstance> createMetagameGameInstanceFunc,
            Func<IGameInstance> createGameplayGameInstanceFunc)
        {
            _createMetagameGameInstanceFunc = createMetagameGameInstanceFunc;
            _createGameplayGameInstanceFunc = createGameplayGameInstanceFunc;
            _gameSceneManager = gameSceneManager;
            await UniTask.Yield();
        }

        public async UniTaskVoid ChangeSceneToMetagameScene()
        {
            var metagameGameInstance = _createMetagameGameInstanceFunc?.Invoke();
            metagameGameInstance.OnCreate();
            var op = await _gameSceneManager.LoadActiveSceneAsync(metagameGameInstance.SceneName, false);
            await op.Result.ActivateAsync();
            metagameGameInstance.OnSceneActive(CurrentGameInstance);
            CurrentGameInstance = metagameGameInstance;
        }

        public async UniTaskVoid ChangeSceneToGameplayScene(string sceneName)
        {
            var gameplayGameInstance = _createGameplayGameInstanceFunc?.Invoke();
            gameplayGameInstance.SceneName = sceneName;
            gameplayGameInstance.OnCreate();
            var op = await _gameSceneManager.LoadActiveSceneAsync(sceneName, false);
            await op.Result.ActivateAsync();
            gameplayGameInstance.OnSceneActive(CurrentGameInstance);
            CurrentGameInstance = gameplayGameInstance;
        }
    }
}
