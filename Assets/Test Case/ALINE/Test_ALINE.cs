using Drawing;
using UnityEngine;

namespace Xi.TestCase
{
    public class Test_ALINE : MonoBehaviour
    {
        private void Start() => DrawWithDuration();

        private void Update()
        {
            {
                //DrawWireCylinder();

                //Draw3WireBox();

                //DrayInLocalSpace();

                //DrawWithDuration();

                //DrawInSceenSpace();

            }
        }

        private void Draw3WireBox()
        {
            // Draw three red cubes
            using (Draw.WithColor(Color.red))
            {
                Draw.WireBox(transform.position, Vector3.one);
                Draw.WireBox(transform.position + Vector3.right, Vector3.one);
                Draw.WireBox(transform.position - Vector3.right, Vector3.one);
            }
        }

        private void DrawWireCylinder() =>// Draw a cylinder at the object's position with a height of 2 and a radius of 0.5
                    Draw.WireCylinder(transform.position, Vector3.up, 2f, 0.5f, Color.red);

        private void DrayInLocalSpace()
        {
            using (Draw.InLocalSpace(transform))
            {
                // Draw a box at (0,0,0) relative to the current object
                // This means it will show up at the object's position
                Draw.WireBox(Vector3.zero, Vector3.one);
            }

            // Equivalent code using the lower level WithMatrix scope
            using (Draw.WithMatrix(transform.localToWorldMatrix))
            {
                Draw.WireBox(Vector3.zero, Vector3.one);
            }
        }

        private void DrawWithDuration()
        {
            // This box will be drawn for 2 seconds
            using (Draw.WithDuration(2))
            {
                Draw.WireBox(Vector3.zero, Vector3.one * 2);
            }

            // Scopes can be nested
            using (Draw.WithColor(Color.red))
            {
                using (Draw.WithDuration(2))
                {
                    Draw.WireBox(Vector3.zero, Vector3.one);
                }
            }
        }

        private void DrawInSceenSpace()
        {

        }
    }
}
