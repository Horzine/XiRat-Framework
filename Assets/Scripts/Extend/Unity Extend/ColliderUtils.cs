using UnityEngine;

namespace Xi.Extend
{
    public static class ColliderUtils
    {
        /// <summary>
        /// 检查射线是否与指定的 Collider 相交，使用默认的最大距离。
        /// </summary>
        /// <param name="collider">要检查的 Collider。</param>
        /// <param name="ray">射线。</param>
        /// <param name="hitInfo">射线击中信息。</param>
        /// <returns>如果射线击中了 Collider，则返回 true；否则返回 false。</returns>
        public static bool RaycastWithBoundsCheck(this Collider collider, Ray ray, out RaycastHit hitInfo)
            => RaycastWithBoundsCheck(collider, ray, out hitInfo, Mathf.Infinity);

        /// <summary>
        /// 检查射线是否与指定的 Collider 相交，使用指定的最大距离。
        /// </summary>
        /// <param name="collider">要检查的 Collider。</param>
        /// <param name="ray">射线。</param>
        /// <param name="hitInfo">射线击中信息。</param>
        /// <param name="maxDistance">最大距离。</param>
        /// <returns>如果射线击中了 Collider，则返回 true；否则返回 false。</returns>
        public static bool RaycastWithBoundsCheck(this Collider collider, Ray ray, out RaycastHit hitInfo, float maxDistance)
        {
            // 检查 Collider 是否启用
            if (!collider.enabled)
            {
                hitInfo = default;
                return false;
            }

            // 获取 Collider 的边界框
            var bounds = collider.bounds;

            // 判断射线是否与边界框相交
            if (bounds.IntersectRay(ray))
            {
                // 进行实际的射线检测
                return collider.Raycast(ray, out hitInfo, maxDistance);
            }

            // 射线与边界框不相交
            hitInfo = default;
            return false;
        }

        /// <summary>
        /// 检查射线是否与指定的 Collider 相交，使用指定的最大距离和层掩码。
        /// </summary>
        /// <param name="collider">要检查的 Collider。</param>
        /// <param name="ray">射线。</param>
        /// <param name="hitInfo">射线击中信息。</param>
        /// <param name="maxDistance">最大距离。</param>
        /// <param name="layerMask">层掩码，用于过滤 Collider。</param>
        /// <returns>如果射线击中了 Collider，则返回 true；否则返回 false。</returns>
        public static bool RaycastWithBoundsCheck(this Collider collider, Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
        {
            // 检查 Collider 是否启用
            if (!collider.enabled)
            {
                hitInfo = default;
                return false;
            }

            // 获取 Collider 的边界框
            var bounds = collider.bounds;

            // 判断射线是否与边界框相交 && 考虑层掩码
            if (bounds.IntersectRay(ray) && (layerMask & (1 << collider.gameObject.layer)) != 0)
            {
                return collider.Raycast(ray, out hitInfo, maxDistance);
            }

            // 射线与边界框不相交
            hitInfo = default;
            return false;
        }
    }
}
