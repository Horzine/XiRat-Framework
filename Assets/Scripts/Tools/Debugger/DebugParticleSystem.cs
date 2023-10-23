using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(ParticleSystem))]
    internal class DebugParticleSystem : DebugComponent<ParticleSystem>
    {
        protected override void OnSceneWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button(target.isPlaying ? "Pause" : "Play", Array.Empty<GUILayoutOption>()))
            {
                if (target.isPlaying)
                {
                    target.Pause();
                }
                else
                {
                    target.Play();
                }
            }

            if (GUILayout.Button("Restart", Array.Empty<GUILayoutOption>()))
            {
                target.Stop();
                target.Play();
            }

            if (GUILayout.Button("Stop", Array.Empty<GUILayoutOption>()))
            {
                target.Stop();
            }

            GUILayout.EndHorizontal();
            GUILayout.Label($"Particles: {target.particleCount}", Array.Empty<GUILayoutOption>());
            var main = target.main;
            main.loop = GUILayout.Toggle(main.loop, " Loop", Array.Empty<GUILayoutOption>());
        }
    }
}