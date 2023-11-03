using UnityEngine;

namespace Xi.Extend.UnityExtend
{
    public static class TransformExtend
    {
        public static RectTransform GetRectTransform(this Component cp) => cp.transform as RectTransform;

        public static void SetIdentity(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var comp))
            {
                comp = gameObject.AddComponent<T>();
            }

            return comp;
        }

        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
            => transform.gameObject.GetOrAddComponent<T>();

        public static void RemoveAllChild(this Transform transform)
        {
            if (transform == null)
            {

                return;
            }

            for (int i = 0; i < transform.childCount; ++i)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
