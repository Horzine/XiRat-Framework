using UnityEditor;
using UnityEngine;

namespace Xi.EditorExtend
{
    [InitializeOnLoad]
    public class LocalTestModePlayButton : MonoBehaviour
    {
        static LocalTestModePlayButton() => ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (!EditorApplication.isPlayingOrWillChangePlaymode && GUILayout.Button(new GUIContent("► Local Test", "Start in local test mode"), new GUILayoutOption[0]))
            {
                Debug.Log($"[{nameof(LocalTestModePlayButton)}]<{nameof(OnToolbarGUI)}>: Playing local test mode");
                LocalTestMode.IsLocalTestMode = true;
                EditorApplication.isPlaying = true;
            }
        }
    }
}