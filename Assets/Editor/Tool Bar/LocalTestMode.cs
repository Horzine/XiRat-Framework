using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Xi.EditorExtend
{
    [InitializeOnLoad]
    public static class LocalTestMode
    {
        private const string kLocalTestMode = "LocalTestMode";
        private static bool _inited;
        private static int _isLocalTestMode = -1;
        public static bool IsLocalTestMode
        {
            get
            {
                if (_isLocalTestMode == -1)
                {
                    _isLocalTestMode = EditorPrefs.GetBool(kLocalTestMode, false) ? 1 : 0;
                }

                return _isLocalTestMode != 0;
            }
            set
            {
                int newValue = value ? 1 : 0;
                if (newValue != _isLocalTestMode)
                {
                    _isLocalTestMode = newValue;
                    EditorPrefs.SetBool(kLocalTestMode, value);
                }
            }
        }
        static LocalTestMode() { }

        [PostProcessScene(1)]
        private static void PrepareSceneForLocalTest()
        {
            if (_inited)
            {
                return;
            }

            if (IsLocalTestMode && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;
                _inited = true;

                CreateDebugGameObjects().Forget();
            }
        }

        private static void HandleOnPlayModeChanged(PlayModeStateChange playMode)
        {
            if (playMode == PlayModeStateChange.ExitingPlayMode)
            {
                IsLocalTestMode = false;
                _inited = false;
            }
        }

        private static async UniTaskVoid CreateDebugGameObjects()
        {
            // Add GameObject Here
            var go = new GameObject($"{nameof(LocalTestMode)}");
            await UniTask.Yield();
        }
    }
}
