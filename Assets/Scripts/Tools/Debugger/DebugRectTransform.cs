using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(RectTransform))]
    internal class DebugRectTransform : DebugComponent<RectTransform>
    {
        protected override void OnSceneWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label($"ChildCount: {target.childCount}", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Position:", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.anchoredPosition = DebugSetting.Vector2Field(target.anchoredPosition);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Rotation:", new GUILayoutOption[1] { GUILayout.Width(60f) });
            var obj = target;
            var localRotation = target.localRotation;
            obj.localRotation = Quaternion.Euler(DebugSetting.Vector3Field(localRotation.eulerAngles));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Scale:", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.localScale = DebugSetting.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Size:", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.sizeDelta = DebugSetting.Vector2Field(target.sizeDelta);
            GUILayout.EndHorizontal();
        }
    }
}