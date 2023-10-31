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
    internal static class SceneAutoLoader
    {
        // Static constructor binds a playmode-changed callback.
        // [InitializeOnLoad] above makes sure this gets executed.
        static SceneAutoLoader()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            MasterScene = "Assets/Recreate/Scenes/Gui/MenuScene/Menu.unity";
        }

        private static void OnToolbarGUI()
        {
            if (GUILayout.Button(new GUIContent("► Menu", "Play from Menu"), new GUILayoutOption[0]))
            {
                // AAEditorHelper.PlayModeScriptReminder();

                LoadMasterOnPlay = true;

                EditorApplication.isPlaying = true;
            }

            GUILayout.FlexibleSpace();
        }

        //   [MenuItem("SourceTool/Play From Menu")]
        //private static void SetLoadMasterOnPlay()
        //{
        //	LoadMasterOnPlay = !LoadMasterOnPlay;
        //	Menu.SetChecked("SourceTool/Play From Menu", LoadMasterOnPlay);

        //	if (LoadMasterOnPlay)
        //	{
        //		MasterScene = "Assets/Recreate/Scenes/Gui/MenuScene/Menu.unity";
        //	}
        //}

        //[MenuItem("SourceTool/Play From Menu", true)]
        //private static bool SetLoadMasterOnPlayValidate()
        //{
        //	Menu.SetChecked("SourceTool/Play From Menu", LoadMasterOnPlay);
        //	return true;
        //}

        // Play mode change callback handles the scene load/reload.
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
        private const string cEditorPrefLoadMasterOnPlay = "SceneAutoLoader.LoadMasterOnPlay";
        private const string cEditorPrefMasterScene = "SceneAutoLoader.MasterScene";
        private const string cEditorPrefPreviousScene = "SceneAutoLoader.PreviousScene";

        private static bool LoadMasterOnPlay
        {
            get => EditorPrefs.GetBool(cEditorPrefLoadMasterOnPlay, false);
            set => EditorPrefs.SetBool(cEditorPrefLoadMasterOnPlay, value);
        }

        private static string MasterScene
        {
            get => EditorPrefs.GetString(cEditorPrefMasterScene, "Master.unity");
            set => EditorPrefs.SetString(cEditorPrefMasterScene, value);
        }

        private static string PreviousScene
        {
            get => EditorPrefs.GetString(cEditorPrefPreviousScene, EditorSceneManager.GetActiveScene().path);
            set => EditorPrefs.SetString(cEditorPrefPreviousScene, value);
        }

        private static bool PathMatch(string path1, string path2)
            => !string.IsNullOrEmpty(path1) && !string.IsNullOrEmpty(path2) && path1.Replace('/', ' ').Replace('\\', ' ') == path2.Replace('/', ' ').Replace('\\', ' ');
    }
}