using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(Camera))]
    internal class DebugCamera : DebugComponent<Camera>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            GUILayout.Label("Clear Flags: ", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.clearFlags = (CameraClearFlags)DebugSetting.EnumField(target.clearFlags);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Field Of View: ", Array.Empty<GUILayoutOption>());
            target.fieldOfView = DebugSetting.FloatField(target.fieldOfView);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.fieldOfView = GUILayout.HorizontalSlider(target.fieldOfView, 1f, 179f, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Depth: ", Array.Empty<GUILayoutOption>());
            target.depth = DebugSetting.FloatField(target.depth);
            GUILayout.EndHorizontal();
        }
    }
}