using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(MeshFilter))]
    internal class DebugMeshFilter : DebugComponent<MeshFilter>
    {
        protected override void OnSceneWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label($"Mesh: {(target.mesh ? target.mesh.name : "None")}", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label($"Vertex Count: {(target.mesh ? target.mesh.vertexCount.ToString() : "0")}", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
        }
    }
}