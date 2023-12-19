using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Xi.Extend.UnityExtend;
using Xi.Tools;

namespace Xi.Framework
{
    public abstract class UiBaseWindow : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;

        public async UniTask OpenAsync()
        {
            XiLogger.Log($"[{GetType().Name}]: Begin Open");
            SetCanvasGroupInteractable(false);
            await UniTask.Yield();
            SetCanvasGroupInteractable(true);
            XiLogger.Log($"[{GetType().Name}]: End Open");
        }

        public async UniTask CloseAsync()
        {
            XiLogger.Log($"[{GetType().Name}]: Begin Close");
            SetCanvasGroupInteractable(false);
            await UniTask.Yield();
            SetCanvasGroupInteractable(false);
            XiLogger.Log($"[{GetType().Name}]: End Close");
        }

        public void BaseWindowInit(int sortOrder)
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
        {
            if (_canvasGroup)
            {
                _canvasGroup.interactable = interactable;
            }
        }
    }
}
