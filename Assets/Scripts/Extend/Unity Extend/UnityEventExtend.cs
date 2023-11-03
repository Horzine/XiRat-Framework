using UnityEngine.Events;

namespace Xi.Extend.UnityExtend
{
    public static class UnityEventExtend
    {
        public static void SafeInvoke(this UnityEvent unityEvent) => unityEvent?.Invoke();
    }
}
