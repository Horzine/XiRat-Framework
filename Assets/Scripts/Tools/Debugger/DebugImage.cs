using System;
using UnityEngine;
using UnityEngine.UI;

namespace Xi.Tools
{
    [Debug(typeof(Image))]
    internal class DebugImage : DebugComponent<Image>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            target.raycastTarget = GUILayout.Toggle(target.raycastTarget, " Raycast Target", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Fill Amount: ", Array.Empty<GUILayoutOption>());
            target.fillAmount = DebugSetting.FloatField(target.fillAmount);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.fillCenter = GUILayout.Toggle(target.fillCenter, " Fill Center", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
        }
    }
}