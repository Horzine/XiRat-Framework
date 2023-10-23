using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(Light))]
    internal class DebugLight : DebugComponent<Light>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            var type = target.type;
            GUILayout.Label("Type: " + type.ToString(), Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Intensity: ", Array.Empty<GUILayoutOption>());
            target.intensity = DebugSetting.FloatField(target.intensity);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Range: ", Array.Empty<GUILayoutOption>());
            target.range = DebugSetting.FloatField(target.range);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Spot Angle: ", Array.Empty<GUILayoutOption>());
            target.spotAngle = DebugSetting.FloatField(target.spotAngle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Shadow: ", Array.Empty<GUILayoutOption>());
            target.shadows = (LightShadows)DebugSetting.EnumField(target.shadows);
            GUILayout.EndHorizontal();
        }
    }
}