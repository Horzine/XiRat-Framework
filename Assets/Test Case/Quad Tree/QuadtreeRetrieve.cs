using System.Collections.Generic;
using UnityEngine;
using Xi.Extension.Collection;

namespace Xi.TestCase
{
    public class QuadtreeRetrieve : MonoBehaviour
    {
        private Rect _retrieveRect; // 动态计算的查询矩形

        private Quadtree<QuadtreeCubeObject> _quadtree;

        private readonly List<QuadtreeCubeObject> _retrievedObjects = new();

        private void Start() => _quadtree = FindObjectOfType<QuadtreeTester>().GetQuadtree();

        private void Update()
        {
            if (_quadtree != null)
            {
                // 更新查询矩形的边界
                UpdateRetrieveRect();

                foreach (var item in _retrievedObjects)
                {
                    item.GetComponent<Renderer>().material.color = Color.white;
                }

                // 查询在 retrieveRect 中的对象
                _retrievedObjects.Clear();

                //_retrievedObjects = _quadtree.Retrieve(_retrieveRect);

                _quadtree.RetrieveNonAlloc(_retrievedObjects, _retrieveRect);

                // 将查询到的对象变成红色
                foreach (var cube in _retrievedObjects)
                {
                    cube.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }

        private void UpdateRetrieveRect()
        {
            var position = new Vector2(transform.position.x, transform.position.z);
            var size = new Vector2(transform.localScale.x, transform.localScale.z) * 10;
            _retrieveRect = new Rect(position - (size / 2), size); // 计算新的查询矩形
        }

        private void OnDrawGizmos()
        {
            // 可视化查询矩形
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(_retrieveRect.center.x, 0, _retrieveRect.center.y), new Vector3(_retrieveRect.width, 0.1f, _retrieveRect.height));
        }
    }
}
