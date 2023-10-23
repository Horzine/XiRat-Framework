using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(SphereCollider))]
    internal class DebugSphereCollider : DebugComponent<SphereCollider>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            target.isTrigger = GUILayout.Toggle(target.isTrigger, " Is Trigger", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Center: ", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.center = DebugSetting.Vector3Field(target.center);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Radius: ", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.radius = DebugSetting.FloatField(target.radius);
            GUILayout.EndHorizontal();
        }
    }
}