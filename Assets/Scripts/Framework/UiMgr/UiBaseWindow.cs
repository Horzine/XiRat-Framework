using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Xi.Framework
{
    public abstract class UiBaseWindow : MonoBehaviour
    {
        public async UniTask CloseAsync() => await UniTask.Yield();

        public async UniTask OpenAsync() => await UniTask.Yield();
    }
}
