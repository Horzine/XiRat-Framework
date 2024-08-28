using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Xi.Tools;

namespace Xi.TestCase
{
    public class Test_Logger : MonoBehaviour
    {
        //private void Update() => XiLogger.LogError(Time.time.ToString(), this);

        private async void Start()
        {
            //Test_Exception();

            Test_Exception_OnThread();

            await Task.Delay(1000);

            try
            {
                throw new Exception("的异常！");
            }
            catch (Exception ex)
            {
                //HandleException(ex);
                Debug.Log($"的异常！ {ex}");
            }

            throw new Exception("的异常2！");
        }

        private void Test_Exception()
        {
            try
            {
                var ri = GetComponent<Rigidbody>();
                ri = null;
                ri.useGravity = true;
            }
            catch (System.Exception e)
            {
                XiLogger.LogException(e);
            }

            ICallbackEntry_My my = new Test_CallbackContainer_Entry();
            my.Test();
        }

        private void Test_Exception_OnThread()
        {
            // 模拟异步方法，抛出异常
            Task.Run(async () =>
            {
                await Task.Delay(100); // 模拟异步操作
                throw new Exception("异步方法中的异常！");
            }).ContinueWith(task =>
            {
                if (task.IsFaulted && task.Exception != null)
                {
                    //HandleException(task.Exception);
                }
            });

            // 模拟线程池中的异常
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    throw new Exception("线程池中的异常！");
                }
                catch (Exception ex)
                {
                    //HandleException(ex);
                    Debug.Log($"线程池中的异常！{ex}");
                }

                throw new Exception("线程池中的异常！ 2");
            });
        }
    }
}
