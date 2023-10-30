using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_Singleton : MonoSingleton<Test_Singleton>, ISingleton
    {
        void ISingleton.OnCreate()
        {
            Debug.Log("OnCreate");
            var s = SimpleSingleton.Instance;
            var ss = SimpleAppSingleton.Instance;
        }

        private void OnDestroy()
        {
            SimpleSingleton.Dispose();
            SimpleAppSingleton.Instance.Dispose();
        }
    }

    public class SimpleSingleton : Singleton<SimpleSingleton>
    {
        
    }

    public class SimpleAppSingleton : AppSingleton<SimpleAppSingleton>
    {
        protected override void OnCreate() => Debug.Log("AppSingleton OnCreate");
        protected override void OnDispose() => Debug.Log("AppSingleton OnDispose");
    }
}
