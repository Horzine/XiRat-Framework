using System.Collections.Generic;
using Drawing;
using UnityEngine;
using Xi.Extension;

namespace Xi.TestCase
{
    public class Test_ALINE_DrawingUtilities : MonoBehaviour
    {
        private void Update()
        {
            {
                // Include Renderer !
                // Draw.WireBox(DrawingUtilities.BoundsFrom(transform), Color.black);

                // Collider Only !
                Draw.WireBox(BoundsUtils.BoundsFromAllCollider(transform, true), Color.cyan);

                // BoundsForm3Point();
            }
        }

        private void BoundsForm3Point()
        {
            var points = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 0, 0)/*, new Vector3(0, 1, 1) */};
            Draw.WireBox(DrawingUtilities.BoundsFrom(points), Color.green);
        }
    }
}
