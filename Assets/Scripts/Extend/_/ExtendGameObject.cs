using System;
using System.Collections.Generic;
using UnityEngine;
using Xi.Extend.UnityExtend;

public static class ExtendGameObject
{
    #region Collider
    public static float GetColliderHeight(this GameObject gameobject)
    {
        float height = 0f;
        if (gameobject)
        {
            var collider = gameobject.GetComponent<Collider>();
            if (collider != null)
            {
                if (collider is BoxCollider)
                {
                    height = ((BoxCollider)collider).size.y;
                }

                if (collider is CapsuleCollider)
                {
                    height = ((CapsuleCollider)collider).height;
                }
            }
        }

        return height;
    }

    public static Vector3 GetColliderCenter(this GameObject gameobject)
    {
        var center = Vector3.zero;

        var collider = gameobject.GetComponent<Collider>();
        if (collider != null)
        {
            if (collider is BoxCollider)
            {
                center = ((BoxCollider)collider).center;
            }

            if (collider is CapsuleCollider)
            {
                center = ((CapsuleCollider)collider).center;
            }
        }

        return center;
    }
    #endregion

    #region 事件机制 高效!!
    private class GameObjectEventHolder : MonoBehaviour
    {
        private void OnDestroy() => gameObject._UnSubscribeAll();
    }

    private static readonly Dictionary<GameObject, ExtendEvents.Publisher<string>> sGameObjectEventMap = new();

    private static ExtendEvents.Publisher<string> _GetOrAddPublisher(this GameObject obj)
    {
        if (!sGameObjectEventMap.TryGetValue(obj, out var publisher))
        {
            publisher = new ExtendEvents.Publisher<string>();
            obj.GetOrAddComponent<GameObjectEventHolder>();
            sGameObjectEventMap.Add(obj, publisher);

        }

        return publisher;
    }

    internal static void _UnSubscribeAll(this GameObject obj) => sGameObjectEventMap.Remove(obj);

    public static void UnSubscribeAll() => sGameObjectEventMap.Clear();
    public static void Subscribe<T1, T2, T3, T4, T5, T6>(this GameObject obj, string name, Action<T1, T2, T3, T4, T5, T6> cb) => obj._GetOrAddPublisher().Subscribe(name, cb);
    public static void UnSubscribe<T1, T2, T3, T4, T5, T6>(this GameObject obj, string name, Action<T1, T2, T3, T4, T5, T6> cb) => obj._GetOrAddPublisher().UnSubscribe(name, cb);

    public static void Notify<T1, T2, T3, T4, T5, T6>(this GameObject obj, string name, T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6) => obj._GetOrAddPublisher().Notify(name, a1, a2, a3, a4, a5, a6);
    public static void Subscribe<T1, T2, T3, T4, T5>(this GameObject obj, string name, Action<T1, T2, T3, T4, T5> cb) => obj._GetOrAddPublisher().Subscribe(name, cb);
    public static void UnSubscribe<T1, T2, T3, T4, T5>(this GameObject obj, string name, Action<T1, T2, T3, T4, T5> cb) => obj._GetOrAddPublisher().UnSubscribe(name, cb);

    public static void Notify<T1, T2, T3, T4, T5>(this GameObject obj, string name, T1 a1, T2 a2, T3 a3, T4 a4, T5 a5) => obj._GetOrAddPublisher().Notify(name, a1, a2, a3, a4, a5);
    public static void Subscribe<T1, T2, T3, T4>(this GameObject obj, string name, Action<T1, T2, T3, T4> cb) => obj._GetOrAddPublisher().Subscribe(name, cb);
    public static void UnSubscribe<T1, T2, T3, T4>(this GameObject obj, string name, Action<T1, T2, T3, T4> cb) => obj._GetOrAddPublisher().UnSubscribe(name, cb);

    public static void Notify<T1, T2, T3, T4>(this GameObject obj, string name, T1 a1, T2 a2, T3 a3, T4 a4) => obj._GetOrAddPublisher().Notify(name, a1, a2, a3, a4);
    public static void Subscribe<T1, T2, T3>(this GameObject obj, string name, Action<T1, T2, T3> cb) => obj._GetOrAddPublisher().Subscribe(name, cb);
    public static void UnSubscribe<T1, T2, T3>(this GameObject obj, string name, Action<T1, T2, T3> cb) => obj._GetOrAddPublisher().UnSubscribe(name, cb);

    public static void Notify<T1, T2, T3>(this GameObject obj, string name, T1 a1, T2 a2, T3 a3) => obj._GetOrAddPublisher().Notify(name, a1, a2, a3);

    public static void Subscribe<T1, T2>(this GameObject obj, string name, Action<T1, T2> cb) => obj._GetOrAddPublisher().Subscribe(name, cb);
    public static void UnSubscribe<T1, T2>(this GameObject obj, string name, Action<T1, T2> cb) => obj._GetOrAddPublisher().UnSubscribe(name, cb);

    public static void Notify<T1, T2>(this GameObject obj, string name, T1 a1, T2 a2) => obj._GetOrAddPublisher().Notify(name, a1, a2);
    public static void Subscribe<T1>(this GameObject obj, string name, Action<T1> cb) => obj._GetOrAddPublisher().Subscribe(name, cb);
    public static void UnSubscribe<T1>(this GameObject obj, string name, Action<T1> cb) => obj._GetOrAddPublisher().UnSubscribe(name, cb);

    public static void Notify<T1>(this GameObject obj, string name, T1 a1) => obj._GetOrAddPublisher().Notify(name, a1);
    public static void Subscribe(this GameObject obj, string name, Action cb) => obj._GetOrAddPublisher().Subscribe(name, cb);
    public static void UnSubscribe(this GameObject obj, string name, Action cb) => obj._GetOrAddPublisher().UnSubscribe(name, cb);

    public static void Notify(this GameObject obj, string name) => obj._GetOrAddPublisher().Notify(name);
    #endregion //事件机制 高效！！
}
