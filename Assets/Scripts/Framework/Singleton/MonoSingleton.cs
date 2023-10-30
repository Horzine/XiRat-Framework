using UnityEngine;

namespace Xi.Framework
{
    public interface ISingleton
    {
        void OnCreate();
    }

    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour, ISingleton
    {
        private static T _instance;
        private static readonly object _lock = new();
        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
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
                                singleton.name = typeof(T).Name;
                                _instance.OnCreate();
                                DontDestroyOnLoad(singleton);
                                Debug.Log($"[Singleton][{typeof(T)}]{singleton} was created with DontDestroyOnLoad.");
                            }
                            else
                            {
                                Debug.Log($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                            }
                        }
                    }
                }

                return _instance;
            }
        }

        private void OnDestroy() => applicationIsQuitting = true;
    }
}
