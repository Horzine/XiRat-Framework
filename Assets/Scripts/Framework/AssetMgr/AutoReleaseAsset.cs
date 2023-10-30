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
    }
}
