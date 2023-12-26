using Drawing;
using Unity.Mathematics;
using UnityEngine;

namespace Xi.TestCase
{
    public class Test_ALINE_Styling_AdvancedUsage : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Update()
        {
            {
                // TestColor();

                // TestWidth();

                // RenderingFromDifferentThreads(); !!!! Not working !!!! 

                RenderingToRenderTextures();

                RenderingToSpecificCameras();
            }
        }

        private void TestColor()
        {
            // Draw a circle
            Draw.Circle(Vector3.zero, Vector3.up, 2);

            // Draw a red circle by adding a color parameter
            Draw.Circle(Vector3.zero, Vector3.up, 2, Color.red);

            // Draw three red cubes using a scope
            using (Draw.WithColor(Color.red))
            {
                Draw.WireBox(transform.position, Vector3.one);
                Draw.WireBox(transform.position + Vector3.right, Vector3.one);
                Draw.WireBox(transform.position - Vector3.right, Vector3.one);
            }
        }

        private void TestWidth()
        {
            // Draw a red circle with a line width of 2
            using (Draw.WithLineWidth(2))
            {
                Draw.Circle(Vector3.zero, Vector3.up, 2, Color.red);
            }
        }

        // !!!! Not working !!!!
        private void RenderingFromDifferentThreads()
        {
            var draw = DrawingManager.GetBuilder(true);
            var thread = new System.Threading.Thread(() =>
            {
                // Draw a big grid
                using (draw.WithDuration(10))
                {
                    draw.WireGrid(float3.zero, Quaternion.identity, new int2(100, 100), new float2(10, 10), Color.black);
                }
            });

            thread.Start();
            thread.Join();
            draw.Dispose();
        }

        private void RenderingToRenderTextures() => DrawingManager.allowRenderToRenderTextures = true;

        private void RenderingToSpecificCameras()
        {
            var draw = DrawingManager.GetBuilder(true);

            draw.cameraTargets = new Camera[] { Camera.main };
            // This sphere will only be rendered to myCamera
            draw.WireSphere(Vector3.zero, 0.5f, Color.black);
            draw.Dispose();
        }
    }
}
