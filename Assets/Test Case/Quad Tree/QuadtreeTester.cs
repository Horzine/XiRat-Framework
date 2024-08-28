using System.Collections.Generic;
using UnityEngine;
using Xi.Extension.Collection;

namespace Xi.TestCase
{
    public class QuadtreeTester : MonoBehaviour
    {
        public int maxObjectsPerNode = 4;
        public int maxLevels = 5;
        public float areaSize = 100f;
        public GameObject cubePrefab; // 在 Inspector 中设置立方体预制件

        private Quadtree<QuadtreeCubeObject> _quadtree;
        private readonly List<QuadtreeCubeObject> _cubes = new();

        private void Awake()
        {
            // 初始化四叉树
            var bounds = new Rect(-areaSize / 2, -areaSize / 2, areaSize, areaSize);
            _quadtree = Quadtree<QuadtreeCubeObject>.Create(bounds, maxObjectsPerNode, maxLevels);

            // 随机生成立方体对象
            for (int i = 0; i < 50; i++)
            {
                var randomPosition = new Vector3(Random.Range(-areaSize / 2, areaSize / 2), 0, Random.Range(-areaSize / 2, areaSize / 2));
                var cube = Instantiate(cubePrefab, randomPosition, Quaternion.identity, transform).GetComponent<QuadtreeCubeObject>();
                _quadtree.Insert(cube);
                _cubes.Add(cube);
            }

            cubePrefab.SetActive(false);
        }

        private void Update()
        {
            // 每帧更新四叉树并重新插入立方体
            _quadtree.Clear();
            foreach (var cube in _cubes)
            {
                _quadtree.Insert(cube);
            }
        }

        public Quadtree<QuadtreeCubeObject> GetQuadtree() => _quadtree;

        // 可视化四叉树
        private void OnDrawGizmos() => DrawQuadtree(_quadtree);

        private void DrawQuadtree(Quadtree<QuadtreeCubeObject> tree)
        {
            if (tree == null)
            {
                return;
            }

            Gizmos.color = Color.green;
            // 在水平面上绘制边界
            Gizmos.DrawWireCube(new Vector3(tree.Bounds.center.x, 0, tree.Bounds.center.y), new Vector3(tree.Bounds.width, 0.1f, tree.Bounds.height));

            if (tree.Nodes != null)
            {
                foreach (var node in tree.Nodes)
                {
                    DrawQuadtree(node);
                }
            }
        }
    }
}
