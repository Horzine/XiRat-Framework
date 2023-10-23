using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(MeshCollider))]
    internal class DebugMeshCollider : DebugComponent<MeshCollider>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            target.isTrigger = GUILayout.Toggle(target.isTrigger, " Is Trigger", Array.Empty<GUILayoutOption>());
            target.convex = GUILayout.Toggle(target.convex, " Convex", Array.Empty<GUILayoutOption>());
        }
    }
}