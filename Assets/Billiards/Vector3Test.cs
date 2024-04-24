using Drawing;
using UnityEngine;

namespace Xi_
{
    public class Vector3Test : MonoBehaviour
    {
        public Transform cube;

        public void Update()
        {
            using (Draw.WithColor(Color.black))
            {
                Draw.SphereOutline(transform.position, 0.02f);
            }

            using (Draw.WithColor(Color.red))
            {
                var result = FuncProjectByLocal(cube.position);

                Draw.SphereOutline(result, 0.02f);

                var result2 = FuncProject(cube.position);

                Draw.SphereOutline(result2, 0.03f);
            }

            using (Draw.WithColor(Color.green))
            {
                var result = FuncProjectOnByPlaneLocal(cube.position);

                Draw.SphereOutline(result, 0.02f);

                var result2 = FuncProjectOnPlane(cube.position);

                Draw.SphereOutline(result, 0.03f);
            }
        }

        public Vector3 FuncProjectByLocal(Vector3 point)
        {
            var localPoint = transform.InverseTransformPoint(point);
            var result = Vector3.Project(localPoint, Vector3.forward);
            return transform.TransformPoint(result);
        }

        public Vector3 FuncProject(Vector3 point)
        {
            var result = Vector3.Project(point - transform.position, transform.forward);
            return result + transform.position;
        }

        public Vector3 FuncProjectOnByPlaneLocal(Vector3 point)
        {
            var localPoint = transform.InverseTransformPoint(point);
            var result = Vector3.ProjectOnPlane(localPoint, Vector3.up);
            return transform.TransformPoint(result);
        }

        public Vector3 FuncProjectOnPlane(Vector3 point)
        {
            var result = Vector3.ProjectOnPlane(point - transform.position, transform.up);
            return result + transform.position;
        }
    }
}
