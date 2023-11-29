using System;
using System.Threading;
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
        public enum WindowState
        {
            None,
            Opening,
            OnDisplay,
            Closing,
        }

        protected TWindow _windowObject;
        private UiRootObject _uiRootObject;
        private Action _willRelaseAfterOpening = null;
        public WindowState CurrentWindowState { get; protected set; } = WindowState.None;
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

            CurrentWindowState = WindowState.Opening;
            await DoOpenAsync();
        }

        protected async UniTask DoOpenAsync()
        {
            _windowObject = await AssetManager.Instance.InstantiateScriptAsync<TWindow>(UiNameConst_Extend.AddressableName(PrefabAssetPath),
                Vector3.zero,
                Quaternion.identity,
                IsOverlayMode ? _uiRootObject.OverlayModeCanvasTsf : _uiRootObject.CameraModeCanvasTsf,
                CancellationToken.None);
            _windowObject.GetRectTransform().anchoredPosition3D = Vector3.zero;
            _windowObject.Init(UiEnum_Extend.GetSortingOrder(UiEnum));
            await _windowObject.OpenAsync();
            if (_willRelaseAfterOpening != null)
            {
                _willRelaseAfterOpening.Invoke();
                _willRelaseAfterOpening = null;
                return;
            }

            CurrentWindowState = WindowState.OnDisplay;
        }

        public virtual async UniTask CloseAsync()
        {
            if (!CanClose)
            {
                return;
            }

            BeforeClose();
            CurrentWindowState = WindowState.Closing;
            await DoCloseAsync();
        }

        protected async UniTask DoCloseAsync()
        {
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
            CurrentWindowState = WindowState.None;
        }

        protected bool CanOpen => CurrentWindowState == WindowState.None;

        protected bool CanClose => CurrentWindowState == WindowState.OnDisplay;

        #region Interface IUiController
        UiEnum IUiController.UiEnum => UiEnum;
        UiRootObject IUiController.UiRootObject { set => _uiRootObject = value; }
        void IUiController.ForceReleaseWindow()
        {
            switch (CurrentWindowState)
            {
                case WindowState.None:
                case WindowState.Closing:
                    return;
                case WindowState.Opening:
                    _willRelaseAfterOpening = doDestroyWindow;
                    return;
                case WindowState.OnDisplay:
                    doDestroyWindow();
                    return;
                default:
                    return;
            }

            void doDestroyWindow()
            {
                BeforeClose();
                DestroyWindow();
            }
        }
        #endregion
    }
}
