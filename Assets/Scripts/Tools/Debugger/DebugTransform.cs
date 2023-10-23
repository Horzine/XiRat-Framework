using System;
using UnityEngine;

namespace Xi.Tools
{
    [Debug(typeof(Transform))]
    internal class DebugTransform : DebugComponent<Transform>
    {
        protected override void OnSceneWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label($"ChildCount: {target.childCount}", Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.RepeatButton("x-", Array.Empty<GUILayoutOption>()))
            {
                var obj = target;
                obj.localPosition -= new Vector3(0.1f, 0f, 0f);
            }

            if (GUILayout.RepeatButton("x+", Array.Empty<GUILayoutOption>()))
            {
                var obj2 = target;
                obj2.localPosition += new Vector3(0.1f, 0f, 0f);
            }

            if (GUILayout.RepeatButton("y-", Array.Empty<GUILayoutOption>()))
            {
                var obj3 = target;
                obj3.localPosition -= new Vector3(0f, 0.1f, 0f);
            }

            if (GUILayout.RepeatButton("y+", Array.Empty<GUILayoutOption>()))
            {
                var obj4 = target;
                obj4.localPosition += new Vector3(0f, 0.1f, 0f);
            }

            if (GUILayout.RepeatButton("z-", Array.Empty<GUILayoutOption>()))
            {
                var obj5 = target;
                obj5.localPosition -= new Vector3(0f, 0f, 0.1f);
            }

            if (GUILayout.RepeatButton("z+", Array.Empty<GUILayoutOption>()))
            {
                var obj6 = target;
                obj6.localPosition += new Vector3(0f, 0f, 0.1f);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Position:", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.localPosition = DebugSetting.Vector3Field(target.localPosition);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Rotation:", new GUILayoutOption[1] { GUILayout.Width(60f) });
            var obj7 = target;
            var localRotation = target.localRotation;
            obj7.localRotation = Quaternion.Euler(DebugSetting.Vector3Field(localRotation.eulerAngles));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Scale:", new GUILayoutOption[1] { GUILayout.Width(60f) });
            target.localScale = DebugSetting.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();
        }
    }
}