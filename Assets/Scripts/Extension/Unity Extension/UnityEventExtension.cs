using UnityEngine.Events;

namespace Xi.Extension.UnityExtension
{
    public static class UnityEventExtension
    {
        public static void SafeInvoke(this UnityEvent unityEvent) => unityEvent?.Invoke();
    }
}
