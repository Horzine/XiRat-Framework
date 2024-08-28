using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Gameplay.Main;
using Xi.Metagame.Main;
using Xi.Tools;

namespace Xi.Framework
{
    public class Bootstrap : MonoBehaviour
    {
        private static IReadOnlyCollection<Type> _cachedTypes;
#pragma warning disable IDE0052 
        private static AdvancedLoggerTool _loggerTool;
#pragma warning restore IDE0052

#pragma warning disable IDE0051 
        private void Awake() => SelfInitAsync().Forget();
#pragma warning restore IDE0051 

        private static async UniTaskVoid SelfInitAsync()
        {
            await InitAllManagerAsync();
            await OnInitAllManagerAccomplish();
        }

        private static async UniTask OnInitAllManagerAccomplish()
        {
            XiLogger.CallMark();
            await UniTask.Yield();
            GameMain.Instance.ChangeSceneToMetagameScene().Forget();
        }

        public static IReadOnlyCollection<Type> GetTypesFromAssembly()
        {
            if (_cachedTypes == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                _cachedTypes = assembly.GetTypes();
            }

            return _cachedTypes;
        }

        public static async UniTask InitAllManagerAsync()
        {
            XiLogger.SetupMainThread(Thread.CurrentThread);
            _loggerTool ??= new AdvancedLoggerTool();

            await ConfigManager.Instance.InitAsync();
            await UserArchiveManager.Instance.InitAsync();
            await GameSceneManager.Instance.InitAsync();
            var gameSceneManager = GameSceneManager.Instance;
            await AssetManager.Instance.InitAsync(gameSceneManager);
            var assetManager = AssetManager.Instance;
            await GameMain.Instance.InitAsync(gameSceneManager,
                MetagameGameInstance_Extension.CreateMetagameGameInstance,
                GameplayGameInstance_Extension.CreateGameplayGameInstance);
            await GameObjectPoolManager.Instance.InitAsync(gameSceneManager);
            await UiManager.Instance.InitAsync(GetTypesFromAssembly(), assetManager);
            await EventCenter.Instance.InitAsync();
            await InputManager.Instance.InitAsync();
        }

        public static void InitAllManager()
        {
            XiLogger.SetupMainThread(Thread.CurrentThread);
            _loggerTool ??= new AdvancedLoggerTool();

            ConfigManager.Instance.Init();
            UserArchiveManager.Instance.Init();
            GameSceneManager.Instance.Init();
            var gameSceneManager = GameSceneManager.Instance;
            AssetManager.Instance.Init(gameSceneManager);
            var assetManager = AssetManager.Instance;
            GameMain.Instance.Init(gameSceneManager,
                 MetagameGameInstance_Extension.CreateMetagameGameInstance,
                 GameplayGameInstance_Extension.CreateGameplayGameInstance);
            GameObjectPoolManager.Instance.Init(gameSceneManager);
            UiManager.Instance.Init(GetTypesFromAssembly(), assetManager);
            EventCenter.Instance.Init();
            InputManager.Instance.Init();
        }
    }
}
