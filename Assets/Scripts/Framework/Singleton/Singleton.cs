using System;

namespace Xi.Framework
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T instance;
        private static readonly object lockObject = new();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        instance ??= new T();
                    }
                }

                return instance;
            }
        }

        public static void Dispose() => instance = null;
    }

    public abstract class AppSingleton<T> : IDisposable where T : class, new()
    {
        private static T instance;
        private static readonly object lockObject = new();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                            (instance as AppSingleton<T>)?.OnCreate();
                        }
                    }
                }

                return instance;
            }
        }

        public void Dispose()
        {
            (instance as AppSingleton<T>)?.OnDispose();
            instance = null;
        }

        protected virtual void OnCreate() { }

        protected virtual void OnDispose() { }
    }
}
