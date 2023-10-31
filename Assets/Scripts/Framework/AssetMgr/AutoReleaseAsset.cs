#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Xi.Framework
{
    public class AutoReleaseAsset : MonoBehaviour
    {
        private AsyncOperationHandle _operationHandle;
        public void Init(AsyncOperationHandle operationHandle) => _operationHandle = operationHandle;
        public void OnDestroy()
        {
            if (AssetManager.Instance && _operationHandle.IsValid())
            {
                AssetManager.Instance.Release(_operationHandle);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            Debug.LogError($"{nameof(AutoReleaseAsset)} should not exist in the Prefab, please manually remove this component. Path = {AssetDatabase.GetAssetPath(transform)}");
        }
#endif

    }
}
