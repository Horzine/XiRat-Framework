using UnityEditor;
using UnityEngine;

namespace Xi.EditorExtend
{
    [InitializeOnLoad]
    [AddComponentMenu("Recreate/Editor/Local Test Play Button")]
    public class LocalTestPlayButton : MonoBehaviour
    {
        static LocalTestPlayButton() => ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("► Local Test", "Start in local test mode"), new GUILayoutOption[0]))
            {
                // AAEditorHelper.PlayModeScriptReminder();

                // DebugModeManager.isLocalTestMode = true;

                EditorApplication.isPlaying = true;
            }
        }

        [MenuItem("SourceTool/LocalTest")]
        private static void StartPlay() =>
            // AAEditorHelper.PlayModeScriptReminder();
            // DebugModeManager.isLocalTestMode = true;
            EditorApplication.isPlaying = true;
    }
}