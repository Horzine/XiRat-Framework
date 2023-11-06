using UnityEngine;

namespace Xi.Extend.UnityExtend
{
    public static class TransformExtend
    {
        public static RectTransform GetRectTransform(this Component cp) => cp.transform as RectTransform;

        public static void SetIdentity(this Transform tsf)
        {
            tsf.localPosition = Vector3.zero;
            tsf.localRotation = Quaternion.identity;
            tsf.localScale = Vector3.one;
        }

        public static void DestroyAllChild(this Transform tsf)
        {
            if (tsf == null)
            {
                return;
            }

            for (int i = 0; i < tsf.childCount; ++i)
            {
                Object.Destroy(tsf.GetChild(i).gameObject);
            }
        }

        public static Transform[] GetAllDirectChild(this Transform tsf)
        {
            if (tsf == null || tsf.childCount == 0)
            {
                return null;
            }

            var children = new Transform[tsf.childCount];
            for (int i = 0; i < tsf.childCount; i++)
            {
                var gameObject = tsf.GetChild(i);
                children[i] = gameObject;
            }

            return children;
        }

        public static void AddChildAndSetIdentity(this Transform parent, Transform child, bool autoSetLayer = true)
        {
            child.SetParent(parent);
            child.SetIdentity();
            if (autoSetLayer)
            {
                child.gameObject.SetLayerRecursively(parent.gameObject.layer);
            }
        }
    }
}
