using System;
using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_GameObjectPool_Cube : MonoBehaviour, IGameObjectPoolEntry
    {
        private Action<Test_GameObjectPool_Cube> _delayTimeCallback;

        public void Init(Action<Test_GameObjectPool_Cube> delayTimeCallback) => _delayTimeCallback = delayTimeCallback;

        void IGameObjectPoolEntry.ActionOnCreate() { }
        void IGameObjectPoolEntry.ActionOnDestroy() { }
        void IGameObjectPoolEntry.ActionOnGet() => Invoke(nameof(OnDelayTime), 1);
        void IGameObjectPoolEntry.ActionOnRelease() { }

        private void OnDelayTime() => _delayTimeCallback?.Invoke(this);
    }
}
