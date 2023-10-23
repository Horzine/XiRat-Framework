using System;
using UnityEngine;

namespace Xi.Tools
{

    [Debug(typeof(AudioSource))]
    internal class DebugAudioSource : DebugComponent<AudioSource>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Clip: ", Array.Empty<GUILayoutOption>());
            GUILayout.Label(target.clip ? target.clip.name : "None", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button(target.isPlaying ? "Pause" : "Play", Array.Empty<GUILayoutOption>()))
            {
                if (target.isPlaying)
                {
                    target.Pause();
                }
                else
                {
                    target.UnPause();
                }
            }

            if (GUILayout.Button("Replay", Array.Empty<GUILayoutOption>()))
            {
                target.Stop();
                target.Play();
            }

            if (GUILayout.Button("Stop", Array.Empty<GUILayoutOption>()))
            {
                target.Stop();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.mute = GUILayout.Toggle(target.mute, " Mute", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.loop = GUILayout.Toggle(target.loop, " Loop", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Volume: ", Array.Empty<GUILayoutOption>());
            target.volume = DebugSetting.FloatField(target.volume);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            target.volume = GUILayout.HorizontalSlider(target.volume, 0f, 1f, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
        }
    }
}