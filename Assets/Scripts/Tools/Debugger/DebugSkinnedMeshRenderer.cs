using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Xi.Tools
{
    [Debug(typeof(SkinnedMeshRenderer))]
    internal class DebugSkinnedMeshRenderer : DebugComponent<SkinnedMeshRenderer>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            target.receiveShadows = GUILayout.Toggle(target.receiveShadows, " Receive Shadows", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Cast Shadows: ", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.shadowCastingMode = (ShadowCastingMode)DebugSetting.EnumField(target.shadowCastingMode);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label($"Bones: {target.bones.Length}", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
        }
    }
}