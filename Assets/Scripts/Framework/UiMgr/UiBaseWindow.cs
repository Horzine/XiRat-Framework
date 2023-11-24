using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Extend.UnityExtend;

namespace Xi.Framework
{
    public abstract class UiBaseWindow : MonoBehaviour
    {
        public async UniTask OpenAsync()
        {
            await UniTask.Delay(1000);
            Debug.Log($"[{GetType().Name}]<{nameof(OpenAsync)}>:");
        }

        public async UniTask CloseAsync()
        {
            await UniTask.Delay(1000);
            Debug.Log($"[{GetType().Name}]<{nameof(CloseAsync)}>:");
        }

        public void SetCanvasSortOrder(int sortOrder)
        {
            if (sortOrder == UiEnum_Extend.kDefaultOrder)
            {
                return;
            }

            var canvas = this.GetOrAddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortOrder;
        }
    }
}
