using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Extend.UnityExtend;

namespace Xi.Framework
{
    public abstract class UiBaseWindow : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;

        public async UniTask OpenAsync()
        {
            Debug.Log($"[{GetType().Name}]<{nameof(OpenAsync)}>: Begin Open");
            SetCanvasGroupInteractable(false);
            await UniTask.Yield();
            SetCanvasGroupInteractable(true);
            Debug.Log($"[{GetType().Name}]<{nameof(OpenAsync)}>: End Open");
        }

        public async UniTask CloseAsync()
        {
            Debug.Log($"[{GetType().Name}]<{nameof(CloseAsync)}>: Begin Close");
            SetCanvasGroupInteractable(false);
            await UniTask.Yield();
            SetCanvasGroupInteractable(false);
            Debug.Log($"[{GetType().Name}]<{nameof(CloseAsync)}>: End Close");
        }

        public void Init(int sortOrder)
        {
            SetCanvasSortOrder(sortOrder);
            _canvasGroup = this.GetOrAddComponent<CanvasGroup>();
            SetCanvasGroupInteractable(false);
        }

        private void SetCanvasSortOrder(int sortOrder)
        {
            if (sortOrder == UiEnum_Extend.kDefaultOrder)
            {
                return;
            }

            var canvas = this.GetOrAddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortOrder;
        }

        protected void SetCanvasGroupAlpha(float alpha)
        {
            alpha = Mathf.Clamp01(alpha);
            _canvasGroup.alpha = alpha;
        }

        protected void SetCanvasGroupInteractable(bool interactable)
            => _canvasGroup.interactable = interactable;
    }
}
