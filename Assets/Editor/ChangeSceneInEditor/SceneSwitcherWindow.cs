using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Xi.Tools;

namespace Xi.EditorExtension
{
    public class SceneSwitcherWindow : EditorWindow
    {
        private const string kAssetsFolder = "Assets";
        private const string kDefaultGameScenesFolder = "Assets/Scenes";
        private string _scenesFolder = kDefaultGameScenesFolder;
        private bool _showFullPath = false;
        private Vector2 _scrollPosition = Vector2.zero;

        [MenuItem("Xi-Windows/Scene Switcher Window")]
        private static void OpenWindow() => GetWindow<SceneSwitcherWindow>(false, "Scene Switcher");

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            _scenesFolder = EditorGUILayout.TextField("Scenes Folder:", _scenesFolder);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            _showFullPath = EditorGUILayout.Toggle("Show Full Path", _showFullPath);
            GUILayout.EndHorizontal();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            if (string.IsNullOrEmpty(_scenesFolder))
            {
                _scenesFolder = kAssetsFolder;
            }

            if (!AssetDatabase.IsValidFolder(_scenesFolder))
            {
                EditorGUILayout.EndScrollView();
                return;
            }

            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { _scenesFolder });

            foreach (string sceneGuid in sceneGuids)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                string displayText = _showFullPath ? scenePath : sceneName;

                if (GUILayout.Button($"{displayText}"))
                {
                    SwitchToScene(scenePath);
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private static void SwitchToScene(string scenePath)
        {
            if (EditorApplication.isPlaying
                || EditorApplication.isPlayingOrWillChangePlaymode
                || EditorApplication.isPaused
                || EditorApplication.isCompiling)
            {
                XiLogger.LogWarning("Cannot switch scenes during Play mode or scene transition.");
                return;
            }

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}
