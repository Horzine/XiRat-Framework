using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Scene auto loader.
/// </summary>
/// <description>
/// This class adds a File > Scene Autoload menu containing options to select
/// a "master scene" enable it to be auto-loaded when the user presses play
/// in the editor. When enabled, the selected scene will be loaded on play,
/// then the original scene will be reloaded on stop.
///
/// Based on an idea on this thread:
/// http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor
/// </description>
namespace Xi.EditorExtend
{
    [InitializeOnLoad]
    internal static class SceneAutoLoaderButton
    {
        private static readonly string kMasterSceneName = $"Assets/Scenes/Build/{Framework.SceneNameConst.kBoost}.unity";

        // Static constructor binds a playmode-changed callback.
        // [InitializeOnLoad] above makes sure this gets executed.
        static SceneAutoLoaderButton()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnToolbarGUI()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode && GUILayout.Button(new GUIContent("► Boost", "Play from Boost"), new GUILayoutOption[0]))
            {
                LoadMasterOnPlay = true;
                EditorApplication.isPlaying = true;
            }

            GUILayout.FlexibleSpace();
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!LoadMasterOnPlay)
            {
                return;
            }

            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // User pressed play -- autoload master scene.
                PreviousScene = EditorSceneManager.GetActiveScene().path;

                if (PathMatch(MasterScene, PreviousScene))
                {
                    return;
                }

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    try
                    {
                        EditorSceneManager.OpenScene(MasterScene);
                    }
                    catch
                    {
                        Debug.LogError(string.Format("error: scene not found: {0}", MasterScene));
                        EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    // User canceled the save operation -- cancel play as well.
                    EditorApplication.isPlaying = false;
                }
            }

            // isPlaying check required because cannot OpenScene while playing
            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                LoadMasterOnPlay = false;

                if (PathMatch(MasterScene, PreviousScene))
                {
                    return;
                }

                // User pressed stop -- reload previous scene.
                try
                {
                    EditorSceneManager.OpenScene(PreviousScene);
                }
                catch
                {
                    Debug.LogError(string.Format("error: scene not found: {0}", PreviousScene));
                }
            }
        }

        // Properties are remembered as editor preferences.
        private const string kEditorPrefLoadMasterOnPlay = "SceneAutoLoaderButton.LoadMasterOnPlay";
        private const string kEditorPrefMasterScene = "SceneAutoLoaderButton.MasterScene";
        private const string kEditorPrefPreviousScene = "SceneAutoLoaderButton.PreviousScene";

        private static bool LoadMasterOnPlay
        {
            get => EditorPrefs.GetBool(kEditorPrefLoadMasterOnPlay, false);
            set => EditorPrefs.SetBool(kEditorPrefLoadMasterOnPlay, value);
        }

        private static string MasterScene
        {
            get => EditorPrefs.GetString(kEditorPrefMasterScene, kMasterSceneName);
            set => EditorPrefs.SetString(kEditorPrefMasterScene, value);
        }

        private static string PreviousScene
        {
            get => EditorPrefs.GetString(kEditorPrefPreviousScene, EditorSceneManager.GetActiveScene().path);
            set => EditorPrefs.SetString(kEditorPrefPreviousScene, value);
        }

        private static bool PathMatch(string path1, string path2)
            => !string.IsNullOrEmpty(path1) && !string.IsNullOrEmpty(path2) && path1.Replace('/', ' ').Replace('\\', ' ') == path2.Replace('/', ' ').Replace('\\', ' ');
    }
}