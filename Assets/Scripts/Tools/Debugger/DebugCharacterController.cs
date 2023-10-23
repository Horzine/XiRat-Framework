using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(CharacterController))]
    internal class DebugCharacterController : DebugComponent<CharacterController>
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
            GUILayout.Label("Height: ", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.height = DebugSetting.FloatField(target.height);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Radius: ", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.radius = DebugSetting.FloatField(target.radius);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label($"Is Grounded: {target.isGrounded}", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            var velocity = target.velocity;
            GUILayout.Label($"Velocity: {velocity}", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
        }
    }
}