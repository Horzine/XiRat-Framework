using System;
using UnityEngine;
using UnityEngine.Pool;
using Xi.Extension.UnityExtension;
using Object = UnityEngine.Object;

namespace Xi.Framework
{
    public interface IGameObjectPoolEntry
    {
        internal void ActionOnCreate();
        internal void ActionOnGet();
        internal void ActionOnRelease();
        internal void ActionOnDestroy();
    }
    public interface IGameObjectPool
    {
        internal void Release();
    }
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
}
