using Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Xi_
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private Ball _ballTemplate;
        [SerializeField] private TableBoard _boardForward;
        [SerializeField] private TableBoard _boardBackward;
        [SerializeField] private TableBoard _boardLeft;
        [SerializeField] private TableBoard _boardRight;

        private Plane _topSurfacePlane;
        private Bounds _localTopSurfaceLimitBounds;
        private Transform _selfTsf;
        private BallCollisionHandler _collisionHandler;

        private Vector3 TopSurfaceCenterPoint => _selfTsf.position + _boxCollider.center + (0.5f * _boxCollider.size.y * _selfTsf.localScale.y * _selfTsf.up);

        private void Awake()
        {
            _collisionHandler = FindObjectOfType<BallCollisionHandler>();

            _selfTsf = transform;
            _topSurfacePlane = new Plane(_selfTsf.up, TopSurfaceCenterPoint);
            var boundsCenter = _selfTsf.InverseTransformPoint(TopSurfaceCenterPoint);
            var boundsSize = new Vector3(_boxCollider.size.x - (2 * _ballTemplate.BallRadius / _selfTsf.localScale.x), 0, _boxCollider.size.z - (2 * _ballTemplate.BallRadius / _selfTsf.localScale.z));
            _localTopSurfaceLimitBounds = new Bounds(boundsCenter, boundsSize);

            foreach (var item in new TableBoard[] { _boardForward, _boardBackward, _boardLeft, _boardRight, })
            {
                var boardToTable = _selfTsf.position - item.transform.position;
                boardToTable.y = 0;
                item.Init(OnBoardTriggerEnterBall, OnBoardTriggerExitBall, boardToTable);
            }
        }

        private void Update()
        {
            using (Draw.WithColor(Color.black))
            {
                Draw.SphereOutline(TopSurfaceCenterPoint, 0.02f);
            }

            using (Draw.WithColor(Color.green))
            {
                var boundsCenter = _localTopSurfaceLimitBounds.center;
                var boundsSize = _localTopSurfaceLimitBounds.size;
                var halfExtentsForward = 0.5f * boundsSize.z * Vector3.forward;
                var halfExtentsLeft = 0.5f * boundsSize.x * Vector3.right;
                var corner1 = boundsCenter + halfExtentsForward + halfExtentsLeft;
                var corner2 = boundsCenter + halfExtentsForward - halfExtentsLeft;
                var corner3 = boundsCenter - halfExtentsForward + halfExtentsLeft;
                var corner4 = boundsCenter - halfExtentsForward - halfExtentsLeft;

                Draw.Line(_selfTsf.TransformPoint(corner1), _selfTsf.TransformPoint(corner3));
                Draw.Line(_selfTsf.TransformPoint(corner3), _selfTsf.TransformPoint(corner4));
                Draw.Line(_selfTsf.TransformPoint(corner4), _selfTsf.TransformPoint(corner2));
                Draw.Line(_selfTsf.TransformPoint(corner2), _selfTsf.TransformPoint(corner1));
            }

            using (Draw.WithColor(Color.red))
            {
                Draw.xz.Circle(ProjectPointOnOBB(_ballTemplate.transform.position), 0.02f);
            }

            using (Draw.WithColor(Color.yellow))
            {
                Draw.xz.Circle(GetPointOnTopSurface(_ballTemplate.transform.position), 0.03f);
            }
        }

        public Vector3 GetPointOnTopSurface(Vector3 worldPoint) => _topSurfacePlane.ClosestPointOnPlane(worldPoint);

        public Vector3 ProjectPointOnOBB(Vector3 point)
        {
            // 将点转换到OBB的局部坐标系
            var localPoint = _selfTsf.InverseTransformPoint(point);
            var min = _localTopSurfaceLimitBounds.min;
            var max = _localTopSurfaceLimitBounds.max;

            // 裁剪点到OBB的边界
            var clampedLocalPoint = new Vector3(
                Mathf.Clamp(localPoint.x, min.x, max.x),
                Mathf.Clamp(localPoint.y, min.y, max.y),
                Mathf.Clamp(localPoint.z, min.z, max.z)
            );

            // 如果点已在OBB内或其表面上，localPoint与clampedLocalPoint相同
            // 否则clampedLocalPoint是OBB上距离点最近的点

            // 将裁剪后的点转换回世界坐标系
            return _selfTsf.TransformPoint(clampedLocalPoint);
        }

        private void OnBoardTriggerEnterBall(BoardDirectionEnum boardDirection, Ball ballObj, Vector3 boardNormal)
        {
            //XiLogger.CallMark();
            _collisionHandler.OnTriggerEnterTableBoard(ballObj, boardNormal);
        }

        private void OnBoardTriggerExitBall(BoardDirectionEnum boardDirection, Ball ballObj)
        {
            //XiLogger.CallMark();
        }
    }
}
