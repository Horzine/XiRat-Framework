using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(AudioListener))]
    internal class DebugAudioListener : DebugComponent<AudioListener>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
        }
    }
}
