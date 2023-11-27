using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Extend.UnityExtend;
using Xi.Tools;

namespace Xi.Framework
{
    public interface IUiController
    {
        void ForceReleaseWindow();
        UiEnum UiEnum { get; }
        UiRootObject UiRootObject { set; }
    }
    public abstract class UiBaseController<TWindow> : IUiController where TWindow : UiBaseWindow
    {
        private enum WindowState
        {
            None,
            Opening,
            OnDisplay,
            Closing,
        }
        protected TWindow _windowObject;
        private WindowState _windowState = WindowState.None;
        private UiRootObject _uiRootObject;
        protected abstract UiEnum UiEnum { get; }
        protected abstract (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath { get; }
        protected abstract bool IsOverlayMode { get; }

        public abstract void BeforeClose();

        protected async UniTask OpenAsync()
        {
            if (!CanOpen)
            {
                return;
            }

            _windowState = WindowState.Opening;
            _windowObject = await AssetManager.Instance.InstantiateScriptAsync<TWindow>(UiNameConst_Extend.AddressableName(PrefabAssetPath),
                Vector3.zero,
                Quaternion.identity,
                IsOverlayMode ? _uiRootObject.OverlayModeCanvasTsf : _uiRootObject.CameraModeCanvasTsf);
            _windowObject.GetRectTransform().anchoredPosition3D = Vector3.zero;
            _windowObject.Init(UiEnum_Extend.GetSortingOrder(UiEnum));
            await _windowObject.OpenAsync();
            _windowState = WindowState.OnDisplay;
        }

        public async UniTask CloseAsync()
        {
            if (!CanClose)
            {
                return;
            }

            BeforeClose();
            _windowState = WindowState.Closing;
            await _windowObject.CloseAsync();
            DestroyWindow();
        }

        protected void DestroyWindow()
        {
            if (_windowObject)
            {
                _windowObject.DestroySelfGameObject();
                XiLogger.Log($"Destroy {UiEnum} Window");
            }

            _windowObject = null;
            _windowState = WindowState.None;
        }

        protected bool CanOpen => _windowState == WindowState.None;

        protected bool CanClose => _windowState == WindowState.OnDisplay;

        #region Interface IUiController
        UiEnum IUiController.UiEnum => UiEnum;
        UiRootObject IUiController.UiRootObject { set => _uiRootObject = value; }
        void IUiController.ForceReleaseWindow()
        {
            BeforeClose();
            DestroyWindow();
        }
        #endregion
    }
}
