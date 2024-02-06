using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Extend.UnityExtend;
using Xi.Tools;

namespace Xi.Framework
{
    public interface IUiInitParams { }
    public interface IUiController
    {
        void ForceReleaseWindow();
        UiEnum UiEnumValue { get; }
        UiRootObject UiRootObj { set; }
    }
    public abstract class UiBaseController<TWindow, TInitParams> : IUiController
        where TWindow : UiBaseWindow
        where TInitParams : struct, IUiInitParams
    {
        public enum WindowState
        {
            None,
            Opening,
            OnDisplay,
            Closing,
        }
        protected TWindow WindowObj { get; private set; }
        protected TInitParams CachedInitParams { get; private set; }
        private UiRootObject _uiRootObj;
        private Action _willRelaseAfterOpening = null;
        public WindowState CurrentWindowState { get; protected set; } = WindowState.None;
        protected abstract UiEnum UiEnumValue { get; }
        protected abstract (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath { get; }
        protected abstract bool IsOverlayMode { get; }
        protected abstract void OnOpenAccomplishCallback();
        protected abstract void OnCloseAccomplishCallback();
        protected abstract void OnWindowInstantiateCallback();
        protected abstract void OnWindowDestoryCallback();

        public virtual async UniTask OpenAsync(TInitParams initParams)
        {
            if (!CanOpen)
            {
                return;
            }

            CachedInitParams = initParams;

            CurrentWindowState = WindowState.Opening;
            await DoOpenAsync();
        }

        public void OpenAsyncAndForget(TInitParams initParams) => OpenAsync(initParams).Forget();

        protected async UniTask DoOpenAsync()
        {
            WindowObj = await AssetManager.Instance.InstantiateScriptAsync<TWindow>(UiNameConst_Extend.AddressableName(PrefabAssetPath),
                Vector3.zero,
                Quaternion.identity,
                IsOverlayMode ? _uiRootObj.OverlayModeCanvasTsf : _uiRootObj.CameraModeCanvasTsf,
                CancellationToken.None,
                false);
            WindowObj.GetRectTransform().anchoredPosition3D = Vector3.zero;
            WindowObj.BaseWindowInit(UiEnum_Extend.GetSortingOrder(UiEnumValue));
            OnWindowInstantiateCallback();
            await WindowObj.OpenAsync();
            if (_willRelaseAfterOpening != null)
            {
                _willRelaseAfterOpening.Invoke();
                _willRelaseAfterOpening = null;
                return;
            }

            CurrentWindowState = WindowState.OnDisplay;
            OnOpenAccomplishCallback();
        }

        public virtual async UniTask CloseAsync()
        {
            if (!CanClose)
            {
                return;
            }

            CurrentWindowState = WindowState.Closing;
            await DoCloseAsync();
        }

        public void CloseAsyncAndForget() => CloseAsync().Forget();

        protected async UniTask DoCloseAsync()
        {
            await WindowObj.CloseAsync();
            OnCloseAccomplishCallback();
            DoDestroyWindow();
        }

        protected void DestroyWindow()
        {
            if (WindowObj)
            {
                WindowObj.DestroySelfGameObject();
                XiLogger.Log($"Destroy {UiEnumValue} Window");
            }

            WindowObj = null;
            CurrentWindowState = WindowState.None;
        }

        protected void CleanUiInitParams() => CachedInitParams = default;

        protected bool CanOpen => CurrentWindowState == WindowState.None;

        protected bool CanClose => CurrentWindowState == WindowState.OnDisplay;

        private void DoDestroyWindow()
        {
            DestroyWindow();
            OnWindowDestoryCallback();
            CleanUiInitParams();
        }

        #region Interface IUiController
        UiEnum IUiController.UiEnumValue => UiEnumValue;
        UiRootObject IUiController.UiRootObj { set => _uiRootObj = value; }
        void IUiController.ForceReleaseWindow()
        {
            switch (CurrentWindowState)
            {
                case WindowState.None:
                case WindowState.Closing:
                    return;
                case WindowState.Opening:
                    _willRelaseAfterOpening = DoDestroyWindow;
                    return;
                case WindowState.OnDisplay:
                    DoDestroyWindow();
                    return;
                default:
                    return;
            }
        }
        #endregion
    }
}
