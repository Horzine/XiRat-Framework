using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(Animator))]
    internal class DebugAnimator : DebugComponent<Animator>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Speed: ", Array.Empty<GUILayoutOption>());
            target.speed = DebugSetting.FloatField(target.speed);
            GUILayout.EndHorizontal();
        }
    }
}
