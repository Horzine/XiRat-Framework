using UnityEditor;
using UnityEngine;
using Xi.Tools;

namespace Xi.EditorExtend
{
    [InitializeOnLoad]
    public static class LocalTestModePlayButton
    {
        static LocalTestModePlayButton() => ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (!EditorApplication.isPlayingOrWillChangePlaymode && GUILayout.Button(new GUIContent("► Local Test", "Start in local test mode"), new GUILayoutOption[0]))
            {
                XiLogger.Log($"Playing local test mode");
                LocalTestMode.IsLocalTestMode = true;
                EditorApplication.isPlaying = true;
            }
        }
    }
}