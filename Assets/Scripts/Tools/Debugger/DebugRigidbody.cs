using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(Rigidbody))]
    internal class DebugRigidbody : DebugComponent<Rigidbody>
    {
        protected override void OnSceneWindow()
        {
            target.useGravity = GUILayout.Toggle(target.useGravity, " Use Gravity", Array.Empty<GUILayoutOption>());
            target.isKinematic = GUILayout.Toggle(target.isKinematic, " Is Kinematic", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Velocity:", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.velocity = DebugSetting.Vector3Field(target.velocity);
            GUILayout.EndHorizontal();
        }
    }
}