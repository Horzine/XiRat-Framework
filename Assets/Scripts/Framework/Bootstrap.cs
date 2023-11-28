﻿using System;
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

        private void Awake() => SelfInit().Forget();

        private static async UniTaskVoid SelfInit()
        {
            await InitAllManager();
            await OnInitAllManagerAccomplish();
        }

        private static async UniTask OnInitAllManagerAccomplish()
        {
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

        public static async UniTask InitAllManager()
        {
            _loggerTool ??= new AdvancedLoggerTool();

            await GameSceneManager.Instance.InitAsync();
            var gameSceneManager = GameSceneManager.Instance;
            await AssetManager.Instance.InitAsync();
            var assetManager = AssetManager.Instance;
            await GameMain.Instance.InitAsync(gameSceneManager,
                MetagameGameInstance_Extend.CreateMetagameGameInstance,
                GameplayGameInstance_Extend.CreateGameplayGameInstance);
            await GameObjectPoolManager.Instance.InitAsync(gameSceneManager);
            await UiManager.Instance.InitAsync(GetTypesFromAssembly(), assetManager);
            await EventCenter.Instance.InitAsync(GetTypesFromAssembly());

        }
    }
}
