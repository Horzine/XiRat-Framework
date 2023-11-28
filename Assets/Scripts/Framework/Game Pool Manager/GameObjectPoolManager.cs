using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Xi.Framework
{
    public class GameObjectPoolManager : MonoSingleton<GameObjectPoolManager>, ISingleton
    {
        private readonly HashSet<IGameObjectPool> _gameObjectPools = new();
        public Transform RootTsf { get; private set; }

        void ISingleton.OnCreate() { }

        public async UniTask InitAsync(GameSceneManager gameSceneManager)
        {
            gameSceneManager.AddOnActiveSceneChangedActionListener(OnActiveSceneChangedCallback);
            await UniTask.Yield();
        }

        private void OnActiveSceneChangedCallback(Scene scene1, Scene scene2)
        {
            RootTsf = null;
            RootTsf = new GameObject($"{nameof(GameObjectPoolManager)} Root Transform").transform;
        }

        public GameObjectPool<T> CreateGameObjectPool<T>(T template,
            Action<T> actionAfterCreate,
            Func<T> createFunc = null,
            Action<T> actionOnGet = null,
            Action<T> actionOnRelease = null,
            Action<T> actionOnDestroy = null) where T : MonoBehaviour, IGameObjectPoolEntry
        {
            var pool = new GameObjectPool<T>(template, RootTsf, actionAfterCreate, createFunc, actionOnGet, actionOnRelease, actionOnDestroy);
            _gameObjectPools.Add(pool);
            return pool;
        }

        public void ReleaseGameObjectPool(IGameObjectPool objectPool)
        {
            _gameObjectPools.Remove(objectPool);
            objectPool.Release();
        }
    }
}
