using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Xi.Extend.UnityExtend;
using Object = UnityEngine.Object;

namespace Xi.Framework
{
    public interface IGameObjectPoolEntry
    {
        void ActionOnCreate();
        void ActionOnGet();
        void ActionOnRelease();
        void ActionOnDestroy();
    }
    public interface IGameObjectPool { void Release(); }
    public class GameObjectPool<T> : IDisposable, IGameObjectPool where T : MonoBehaviour, IGameObjectPoolEntry
    {
        private readonly ObjectPool<T> _pool;
        private readonly T _template;
        private readonly Transform _parentTsf;
        private readonly Action<T> _actionAfterCreate;

        public GameObjectPool(T template,
            Transform rootTsf,
            Action<T> actionAfterCreate,
            Func<T> createFunc = null,
            Action<T> actionOnGet = null,
            Action<T> actionOnRelease = null,
            Action<T> actionOnDestroy = null)
        {
            _template = template;
            _actionAfterCreate = actionAfterCreate;
            _template.SetSelfActive(false);
            _parentTsf = new GameObject($"{template.name} Pool").transform;
            _parentTsf.SetParent(rootTsf);
            _pool = new ObjectPool<T>(createFunc ?? CreateEntryFunc, actionOnGet ?? ActionOnGet, actionOnRelease ?? ActionOnRelease, actionOnDestroy ?? ActionOnDestroy);
        }
        private T CreateEntryFunc()
        {
            var entry = Object.Instantiate(_template, _parentTsf);
            _actionAfterCreate?.Invoke(entry);
            entry.ActionOnCreate();
            return entry;
        }
        private void ActionOnGet(T entry)
        {
            entry.SetSelfActive(true);
            entry.ActionOnGet();
        }
        private void ActionOnRelease(T entry)
        {
            entry.SetSelfActive(false);
            entry.ActionOnRelease();
        }
        private void ActionOnDestroy(T entry)
        {
            entry.ActionOnDestroy();
            entry.DestroySelfGameObject();
        }
        private void Clear()
        {
            _template.DestroySelfGameObject();
            _parentTsf.DestroySelfGameObject();
            _pool.Clear();
        }
        void IDisposable.Dispose() => Clear();
        void IGameObjectPool.Release() => Clear();
        public T GetEntry() => _pool.Get();
        public void ReleaseEntry(T entry) => _pool.Release(entry);
    }
    public class GameObjectPoolManager : MonoSingleton<GameObjectPoolManager>, ISingleton
    {
        private readonly HashSet<IGameObjectPool> _gameObjectPools = new();
        public Transform RootTsf { get; private set; }

        void ISingleton.OnCreate() { }

        public async UniTask InitAsync() => await UniTask.Yield();

        // ------TEMP------
        private void OnSceneEnter(string sceneName)
        {
            if (!RootTsf)
            {
                RootTsf = new GameObject(string.Concat(nameof(GameObjectPoolManager), "Root Transform")).transform;
            }
        }
        private void OnSceneExit(string sceneName) => RootTsf = null;
        // ------TEMP------

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
