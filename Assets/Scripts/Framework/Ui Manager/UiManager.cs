using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Extend.Collection;
using Xi.Tools;

namespace Xi.Framework
{
    public class UiManager : MonoSingleton<UiManager>, ISingleton
    {
        private const string kUiManagerPrefabName = "UiRootObject";

        private UiRootObject _uiRootObject;
        private readonly Dictionary<int, IUiController> _allUiController = new();

        void ISingleton.OnCreate()
        {

        }

        public async UniTask InitAsync(IReadOnlyCollection<Type> allTypeInAssembly, AssetManager assetManager)
        {
            _uiRootObject = await assetManager.InstantiateScriptAsync<UiRootObject>($"{AssetGroupNameConst.kAddressableGroupName_Manager}/{kUiManagerPrefabName}",
                Vector3.zero,
                Quaternion.identity,
                transform,
                this.GetCancellationTokenOnDestroy(),
                false);
            CreateAllUiControllerInstance(allTypeInAssembly);
        }

        private void CreateAllUiControllerInstance(IReadOnlyCollection<Type> allTypeInAssembly)
        {
            foreach (var type in allTypeInAssembly)
            {
                if (typeof(IUiController).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var uiController = (IUiController)Activator.CreateInstance(type);
                    uiController.UiRootObject = _uiRootObject;
                    var uiEnum = uiController.UiEnum;
                    if (!_allUiController.TryAdd((int)uiEnum, uiController))
                    {
                        XiLogger.LogError($"UiEnum {uiEnum} already has instance!");
                    }
                }
            }
        }

        public void ForceReleaseAllWindow() => _allUiController.ForeachValue((item) => item.ForceReleaseWindow());

        public T GetController<T>(UiEnum uiEnum) where T : IUiController
            => _allUiController.TryGetValue((int)uiEnum, out var uiController) ? uiController is T t ? t : default : default;
    }
}
