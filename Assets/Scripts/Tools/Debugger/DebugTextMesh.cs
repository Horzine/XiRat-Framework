using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(TextMesh))]
    internal class DebugTextMesh : DebugComponent<TextMesh>
    {
        protected override void OnSceneWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Text: ", Array.Empty<GUILayoutOption>());
            target.text = GUILayout.TextArea(target.text, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Offset Z: ", Array.Empty<GUILayoutOption>());
            target.offsetZ = DebugSetting.FloatField(target.offsetZ);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Character Size: ", Array.Empty<GUILayoutOption>());
            target.characterSize = DebugSetting.FloatField(target.characterSize);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Line Spacing: ", Array.Empty<GUILayoutOption>());
            target.lineSpacing = DebugSetting.FloatField(target.lineSpacing);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Tab Size: ", Array.Empty<GUILayoutOption>());
            target.tabSize = DebugSetting.FloatField(target.tabSize);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Font Size: ", Array.Empty<GUILayoutOption>());
            target.fontSize = DebugSetting.IntField(target.fontSize);
            GUILayout.EndHorizontal();
        }
    }
}