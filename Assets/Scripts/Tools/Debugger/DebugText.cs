using System;
using UnityEngine;
using UnityEngine.UI;

namespace Xi.Tools
{
    [Debug(typeof(Text))]
    internal class DebugText : DebugComponent<Text>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            target.raycastTarget = GUILayout.Toggle(target.raycastTarget, " Raycast Target", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Text: ", Array.Empty<GUILayoutOption>());
            target.text = GUILayout.TextArea(target.text, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Font Style: ", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.fontStyle = (FontStyle)DebugSetting.EnumField(target.fontStyle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Font Size: ", Array.Empty<GUILayoutOption>());
            target.fontSize = DebugSetting.IntField(target.fontSize);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Line Spacing: ", Array.Empty<GUILayoutOption>());
            target.lineSpacing = DebugSetting.FloatField(target.lineSpacing);
            GUILayout.EndHorizontal();
            target.supportRichText = GUILayout.Toggle(target.supportRichText, " Rich Text", Array.Empty<GUILayoutOption>());
        }
    }
}