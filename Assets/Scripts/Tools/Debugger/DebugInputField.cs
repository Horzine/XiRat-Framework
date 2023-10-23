using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.InputField;

namespace Xi.Tools
{
    [Debug(typeof(InputField))]
    internal class DebugInputField : DebugComponent<InputField>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            target.interactable = GUILayout.Toggle(target.interactable, " Interactable", Array.Empty<GUILayoutOption>());
            target.readOnly = GUILayout.Toggle(target.readOnly, " Read Only", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Text: ", Array.Empty<GUILayoutOption>());
            target.text = GUILayout.TextArea(target.text, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Character Limit: ", Array.Empty<GUILayoutOption>());
            target.characterLimit = DebugSetting.IntField(target.characterLimit);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Content Type: ", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            target.contentType = (ContentType)DebugSetting.EnumField(target.contentType);
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Line Type: ", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            target.lineType = (LineType)DebugSetting.EnumField(target.lineType);
        }
    }
}