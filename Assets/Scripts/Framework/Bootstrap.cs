using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Tools;

namespace Xi.Framework
{
    public class Bootstrap : MonoBehaviour
    {
        private static IReadOnlyCollection<Type> _cachedTypes;
        private static AdvancedLoggerTool _logger;

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
            _logger ??= new AdvancedLoggerTool();

            await AssetManager.Instance.InitAsync();
            await GameObjectPoolManager.Instance.InitAsync();
            await GameSceneManager.Instance.InitAsync();
            await UiManager.Instance.InitAsync(GetTypesFromAssembly(), AssetManager.Instance);
            await EventCenter.Instance.InitAsync(GetTypesFromAssembly());

            OnInitAllManagerAccomplish();
        }

        private void OnInitAllManagerAccomplish() => GameSceneManager.Instance.LoadActiveSceneAsync(SceneNameConst.kMainScene, true).Forget();
    }
}
