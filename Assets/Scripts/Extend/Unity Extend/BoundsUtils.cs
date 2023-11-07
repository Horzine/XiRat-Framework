using UnityEngine;

namespace Xi.Extend
{
    public static class BoundsUtils
    {
        public static Bounds BoundsFromAllCollider(GameObject gameObject, bool includeChild = true) => BoundsFromAllCollider(gameObject.transform, includeChild);

        public static Bounds BoundsFromAllCollider(Transform transform, bool includeChild = true)
        {
            var colliders = includeChild ? transform.GetComponentsInChildren<Collider>() : transform.GetComponents<Collider>();
            var colliders2D = includeChild ? transform.GetComponentsInChildren<Collider2D>() : transform.GetComponents<Collider2D>();

            bool hasValidBound = false;
            Bounds bounds = default;

            foreach (var coll in colliders)
            {
                if (coll.enabled)
                {
                    if (!hasValidBound)
                    {
                        bounds = coll.bounds;
                        hasValidBound = true;
                    }
                    else
                    {
                        bounds.Encapsulate(coll.bounds);
                    }
                }
            }

            foreach (var coll2D in colliders2D)
            {
                if (coll2D.enabled)
                {
                    if (!hasValidBound)
                    {
                        bounds = coll2D.bounds;
                        hasValidBound = true;
                    }
                    else
                    {
                        bounds.Encapsulate(coll2D.bounds);
                    }
                }
            }

            return bounds;
        }
    }
}
