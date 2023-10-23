using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Xi.Tools
{
    [Debug(typeof(PlayableDirector))]
    internal class DebugPlayableDirector : DebugComponent<PlayableDirector>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("Play", Array.Empty<GUILayoutOption>()) && target.state == 0)
            {
                target.Play();
            }

            if (GUILayout.Button("Pause", Array.Empty<GUILayoutOption>()) && (int)target.state == 1)
            {
                target.Pause();
            }

            if (GUILayout.Button("Stop", Array.Empty<GUILayoutOption>()))
            {
                target.Stop();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label($"Duration: {target.duration}", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.extrapolationMode = (DirectorWrapMode)DebugSetting.EnumField(target.extrapolationMode);
            GUILayout.EndHorizontal();
        }
    }
}