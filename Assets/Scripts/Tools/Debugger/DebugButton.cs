using System;
using UnityEngine;
using UnityEngine.UI;

namespace Xi.Tools
{
    [Debug(typeof(Button))]
    internal class DebugButton : DebugComponent<Button>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            target.interactable = GUILayout.Toggle(target.interactable, " Interactable", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            int persistentEventCount = target.onClick.GetPersistentEventCount();
            GUILayout.Label($"Event Count: {persistentEventCount}", Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("OnClick", Array.Empty<GUILayoutOption>()))
            {
                target.onClick.Invoke();
            }

            GUILayout.EndHorizontal();
            for (int i = 0; i < persistentEventCount; i++)
            {
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayout.Label($"{i + 1}: {target.onClick.GetPersistentTarget(i).name} / {target.onClick.GetPersistentMethodName(i)}()", Array.Empty<GUILayoutOption>());
                GUILayout.EndHorizontal();
            }
        }
    }
}