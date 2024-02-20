using UnityEngine;
using Xi.Tools;

namespace Xi.Framework
{
    public interface ISingleton
    {
        internal void OnCreate();
    }

    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour, ISingleton
    {
        private static T _instance;
        private static readonly object _lock = new();
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    XiLogger.LogWarning($"Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                    return null;
                }

                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = FindObjectOfType<T>();
                            if (_instance == null)
                            {
                                var singleton = new GameObject();
                                _instance = singleton.AddComponent<T>();
                                singleton.name = typeof(T).FullName;
                                _instance.OnCreate();
                                DontDestroyOnLoad(singleton);
                                XiLogger.Log($"[{singleton.name}] was created with DontDestroyOnLoad.");
                            }
                            else
                            {
                                XiLogger.Log($"Using instance already created: {_instance.gameObject.name}");
                            }
                        }
                    }
                }

                return _instance;
            }
        }

        private void OnDestroy() => _applicationIsQuitting = true;
    }
}
