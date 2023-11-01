using UnityEditor;
using UnityEngine;

namespace Xi.EditorExtend
{
    [InitializeOnLoad]
    public class LocalTestPlayButton : MonoBehaviour
    {
        static LocalTestPlayButton() => ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (!EditorApplication.isPlayingOrWillChangePlaymode && GUILayout.Button(new GUIContent("► Local Test", "Start in local test mode"), new GUILayoutOption[0]))
            {
                Debug.Log($"[{nameof(LocalTestPlayButton)}]<{nameof(OnToolbarGUI)}>: Playing local test mode");
                EditorApplication.isPlaying = true;
            }
        }
    }
}