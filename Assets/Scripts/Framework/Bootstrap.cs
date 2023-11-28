using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Gameplay;
using Xi.Metagame;
using Xi.Tools;

namespace Xi.Framework
{
    public class Bootstrap : MonoBehaviour
    {
        private static IReadOnlyCollection<Type> _cachedTypes;
        private static AdvancedLoggerTool _loggerTool;

        public static IReadOnlyCollection<Type> GetTypesFromAssembly()
        {
            if (_cachedTypes == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                _cachedTypes = assembly.GetTypes();
            }

            return _cachedTypes;
        }

        private void Awake() => InitAllManager().Forget();

        private async UniTaskVoid InitAllManager()
        {
            _loggerTool ??= new AdvancedLoggerTool();

            await GameSceneManager.Instance.InitAsync();
            await GameMain.Instance.InitAsync(GameSceneManager.Instance, MetagameGameInstance_Extend.CreateMetagameGameInstance, GameplayGameInstance_Extend.CreateGameplayGameInstance);
            await AssetManager.Instance.InitAsync();
            await GameObjectPoolManager.Instance.InitAsync();
            await UiManager.Instance.InitAsync(GetTypesFromAssembly(), AssetManager.Instance);
            await EventCenter.Instance.InitAsync(GetTypesFromAssembly());

            OnInitAllManagerAccomplish().Forget();
        }

        private async UniTask OnInitAllManagerAccomplish()
        {
            await UniTask.Yield();

            GameMain.Instance.ChangeSceneToMetagameScene().Forget();

            await UniTask.Delay(10000);

            GameMain.Instance.ChangeSceneToGameplayScene(SceneNameConst.kMap_1).Forget();
        }
    }
}
