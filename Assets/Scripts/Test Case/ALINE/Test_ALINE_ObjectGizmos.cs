using Drawing;
using UnityEngine;
using Draw = Drawing.Draw;

namespace Xi.TestCase
{
    public class Test_ALINE_ObjectGizmos : MonoBehaviourGizmos
    {
        public override void DrawGizmos()
        {
            {
                // DrawOnSelected();
            }
        }

        private void Update()
        {
            DrawInGame();

            DrawInEditor();
        }

        private void DrawOnSelected()
        {
            using (Draw.InLocalSpace(transform))
            {
                if (GizmoContext.InSelection(this))
                {
                    // Draw a yellow cylinder
                    Draw.WireCylinder(Vector3.zero, Vector3.up, 2f, 0.5f, Color.yellow);
                }
                else
                {
                    // Draw a yellow circle with some transparency
                    Draw.xz.Circle(Vector3.zero, 0.5f, Color.yellow * new Color(1, 1, 1, 0.5f));
                }
            }
        }

        private void DrawInGame()
        {
            // Draws items in the editor and in standalone games, even if gizmos are disabled.
            var gameDraw = Draw.ingame; // Cache 
            using (gameDraw.InLocalSpace(transform))
            {
                gameDraw.WireBox(Vector3.zero, 0.5f);
            }
        }

        private void DrawInEditor()
        {
            // Draws items in the editor if gizmos are enabled.
            var editorDraw = Draw.editor; // Cache
            editorDraw.WireBox(Vector3.zero, Vector3.one * 1);
            editorDraw.WireBox(Vector3.zero, Vector3.one * 2);
            editorDraw.WireBox(Vector3.zero, Vector3.one * 3);
        }
    }
}
