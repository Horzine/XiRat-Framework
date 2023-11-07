using Drawing;
using UnityEngine;

namespace Xi.TestCase
{
    public class Test_ALINE_DrawIn2D : MonoBehaviour
    {
        private void Update()
        {
            var p1 = new Vector2(0, 1);
            var p2 = new Vector2(5, 7);

            // // Draw it in the XY plane
            // Draw.xy.Line(p1, p2);
            // 
            // // Draw it in the XZ plane
            // Draw.xz.Line(p1, p2);
            // 
            // Draw.xy.Circle(new Vector2(2, 2), 0.9f, Palette.Pure.Red);
            // Draw.xy.Label2D(new Vector2(2, 2), "I'm drawn in the XY plane", 20, LabelAlignment.Center, Color.black);
            // 
            // Draw.xz.Circle(new Vector2(2, 2), 0.9f, Palette.Pure.Red);
            // Draw.xz.Label2D(new Vector2(2, 2), "and I'm in the XZ plane", 20, LabelAlignment.Center, Color.black);

            var inGame = Draw.ingame;
            inGame.WireBox(Vector3.zero, 1, Color.red);//Visible in Real Game
            var inEditor = Draw.editor;
            inEditor.SphereOutline(Vector3.zero, 1, Color.blue);

            // Draw it in the XY plane
            inGame.xy.Line(p1, p2);

            // Draw it in the XZ plane
            inEditor.xz.Line(p1, p2);
        }
    }
}
