using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Xi.Framework
{
    public class UiManager : MonoSingleton<UiManager>, ISingleton
    {
        private const string kUiManagerPrefabName = "UiRootObject";
        private UiRootObject _uiRootObject;
        private readonly Dictionary<int, IUiController> _allUiControllers = new();

        void ISingleton.OnCreate()
        {

        }

        public async UniTask InitAsync()
        {
            _uiRootObject = await AssetManager.Instance.InstantiateScriptAsync<UiRootObject>($"{AssetGroupNameConst.kAddressableGroupName_Manager}/{kUiManagerPrefabName}", Vector3.zero, Quaternion.identity, transform);

            CreateAllUiControllerInstance();
        }

        private void CreateAllUiControllerInstance()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (typeof(IUiController).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var uiController = (IUiController)Activator.CreateInstance(type);
                    uiController.UiRootObject = _uiRootObject;
                    var uiEnum = uiController.UiEnum;
                    if (!_allUiControllers.TryAdd((int)uiEnum, uiController))
                    {
                        Debug.LogError($"[{nameof(UiManager)}]<{nameof(CreateAllUiControllerInstance)}>: UiEnum {uiEnum} already has instance!");
                    }
                }
            }
        }

        public T GetController<T>(UiEnum uiEnum) where T : IUiController
            => _allUiControllers.TryGetValue((int)uiEnum, out var uiController) ? uiController is T t ? t : default : default;
    }
}
