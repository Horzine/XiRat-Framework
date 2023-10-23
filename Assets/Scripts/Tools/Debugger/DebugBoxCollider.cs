using System;
using UnityEngine;

namespace Xi.Tools
{

    [Debug(typeof(BoxCollider))]
    internal class DebugBoxCollider : DebugComponent<BoxCollider>
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
            GUILayout.Label("Size: ", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.size = DebugSetting.Vector3Field(target.size);
            GUILayout.EndHorizontal();
        }
    }
}