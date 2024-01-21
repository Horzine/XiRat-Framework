using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Xi.Extend.Collection;
using Xi.Tools;

namespace Xi.TestCase
{

    public interface ICallbackEntry_My : CallbackContainer.ICallbackEntry
    {
        internal void Test();
    }

    public class Test_CallbackContainer_Entry : ICallbackEntry_My
    {
        ~Test_CallbackContainer_Entry()
        {
            XiLogger.Log("~~~");
        }
        void ICallbackEntry_My.Test() => XiLogger.Log("");

        public void TEstTT()
        {
            XiLogger.Log("");
        }
    }

    public class Test_CallbackContainer : MonoBehaviour
    {

        private CallbackContainer<ICallbackEntry_My> _callbackContainer = new();


        private HashSet<WeakReference<Test_CallbackContainer_Entry>> _set = new();
        private Test_CallbackContainer_Entry _entry;

        private async void Start()
        {
            //for (int i = 0; i < 10000; i++)
            //{
            //    _entry = new();


            //    _callbackContainer.AddCallback(_entry);



            //    _callbackContainer.InvokeCallback((e) => e.Test());



            //    // await UniTask.Delay(3000);


            //    _callbackContainer.RemoveCallback(_entry);
            //    _entry = null;
            //    print(Time.frameCount);


            //    _callbackContainer.InvokeCallback((entry) => entry.Test());
            //    // await UniTask.Delay(3000);

            //    _callbackContainer.InvokeCallback((entry) => entry.Test());
            //}

            for (int i = 0; i < 100; i++)
            {

            }

            _entry = new();
            var wk = new WeakReference<Test_CallbackContainer_Entry>(_entry);
            _set.Add(wk);
            _entry = null;

            // 强制垃圾回收
            // GC.Collect();
            // 

            await UniTask.Delay(0);// 这里如果不等待，上一个 GC 是无效的
            // 强制垃圾回收
            XiLogger.Log("Run GC");
            // GC.Collect();


            if (wk.TryGetTarget(out _entry))
            {// wk 不影响 _entry 的回收
                _entry.TEstTT();
            }
            else
            {
                XiLogger.Log("housho");
            }

        }

    }
}
