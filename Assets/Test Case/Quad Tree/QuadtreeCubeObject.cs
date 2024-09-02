using UnityEngine;
using Xi.Extension.Collection;

namespace Xi.TestCase
{
    [SelectionBase]
    public class QuadtreeCubeObject : MonoBehaviour, IQuadtreeObject<QuadtreeCubeObject>
    {
        private Vector2 _selfSize;
        private Transform _selfTsf;
        private Vector3 _lastPos;
        [field: SerializeField] public bool IsDirty { get; set; }
        Quadtree<QuadtreeCubeObject> IQuadtreeObject<QuadtreeCubeObject>.CurrentQuadtree { get; set; }
        private QuadtreeTester Tester { get; set; }
        Rect IQuadtreeObject<QuadtreeCubeObject>.LastInsertCachedBound { get; set; }

        public string numStr = string.Empty;

        private void Awake()
        {
            _selfTsf = transform;
            _selfSize = new Vector2(_selfTsf.localScale.x, _selfTsf.localScale.z);
            _lastPos = _selfTsf.position;
        }

        public void Init(QuadtreeTester tester, int id)
        {
            Tester = tester;
            numStr = id.ToString();
        }

        private void Update()
        {
            if (IsDirty)
            {
                Tester.GetQuadtree().UpdateObject(this);
                IsDirty = false;
            }
        }

        Rect IQuadtreeObject<QuadtreeCubeObject>.GetBounds()
        {
            var position = new Vector2(_selfTsf.position.x, _selfTsf.position.z);
            return new Rect(position - (_selfSize / 2), _selfSize);
        }

        [ContextMenu("Remove From Quadtree")]
        public void RemoveFromQuadtree()
        {
            var CurrentQuadtree = ((IQuadtreeObject<QuadtreeCubeObject>)this).CurrentQuadtree;
            if (CurrentQuadtree != null)
            {
                CurrentQuadtree.Remove(this);
                Tester.GetQuadtree().Insert(this);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var CurrentQuadtree = ((IQuadtreeObject<QuadtreeCubeObject>)this).CurrentQuadtree;
            if (CurrentQuadtree != null)
            {
                Gizmos.color = Color.blue;
                // 在水平面上绘制边界
                Gizmos.DrawWireCube(new Vector3(CurrentQuadtree.Bounds.center.x, 0, CurrentQuadtree.Bounds.center.y), new Vector3(CurrentQuadtree.Bounds.width, 0.1f, CurrentQuadtree.Bounds.height) * 0.97f);
            }
        }

        private void OnDrawGizmos()
        {
            // 获取 cube 的位置
            Vector3 cubePosition = _selfTsf != null ? _selfTsf.position : transform.position;

            // 在指定高度 (y轴) 上获取要显示数字的世界坐标
            Vector3 drawPosition = new Vector3(cubePosition.x, cubePosition.y + 1f, cubePosition.z);

            // 画一个 cube 的 gizmo
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cubePosition, new Vector3(1, 1, 1));  // 在物体位置画个绿色立方体

            // 使用 Handles 来绘制数字文本
            UnityEditor.Handles.color = Color.cyan;
            // 显示数字在 y 轴高度的地方，位置为 drawPosition
            UnityEditor.Handles.Label(drawPosition, numStr, new GUIStyle()
            {
                fontSize = 18,  // 字体大小
                normal = new GUIStyleState() { textColor = Color.cyan }
            });
        }
#endif

    }
}
