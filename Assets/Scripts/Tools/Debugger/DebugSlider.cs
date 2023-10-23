using System;
using UnityEngine;
using UnityEngine.UI;

namespace Xi.Tools
{
    [Debug(typeof(Slider))]
    internal class DebugSlider : DebugComponent<Slider>
    {
        protected override void OnSceneWindow()
        {
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, " Enabled", Array.Empty<GUILayoutOption>());
            target.interactable = GUILayout.Toggle(target.interactable, " Interactable", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Value: ", Array.Empty<GUILayoutOption>());
            target.value = GUILayout.HorizontalSlider(target.value, target.minValue, target.maxValue, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
        }
    }
}