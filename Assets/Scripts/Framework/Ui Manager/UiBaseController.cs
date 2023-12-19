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

        protected TWindow WindowObj { get; private set; }
        private UiRootObject _uiRootObject;
        private Action _willRelaseAfterOpening = null;
        public WindowState CurrentWindowState { get; protected set; } = WindowState.None;
        protected abstract UiEnum UiEnum { get; }
        protected abstract (string groupName, string uiFeatureName, string uiPrefabName) PrefabAssetPath { get; }
        protected abstract bool IsOverlayMode { get; }

        protected abstract void CleanControllerDependency();
        protected abstract void OnOpenAccomplishCallback();
        protected abstract void OnCloseAccomplishCallback();

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
            WindowObj = await AssetManager.Instance.InstantiateScriptAsync<TWindow>(UiNameConst_Extend.AddressableName(PrefabAssetPath),
                Vector3.zero,
                Quaternion.identity,
                IsOverlayMode ? _uiRootObject.OverlayModeCanvasTsf : _uiRootObject.CameraModeCanvasTsf,
                CancellationToken.None,
                false);
            WindowObj.GetRectTransform().anchoredPosition3D = Vector3.zero;
            WindowObj.BaseWindowInit(UiEnum_Extend.GetSortingOrder(UiEnum));
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

            CleanControllerDependency();
            CurrentWindowState = WindowState.Closing;
            await DoCloseAsync();
        }

        protected async UniTask DoCloseAsync()
        {
            await WindowObj.CloseAsync();
            DestroyWindow();
            OnCloseAccomplishCallback();
        }

        protected void DestroyWindow()
        {
            if (WindowObj)
            {
                WindowObj.DestroySelfGameObject();
                XiLogger.Log($"Destroy {UiEnum} Window");
            }

            WindowObj = null;
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
                CleanControllerDependency();
                DestroyWindow();
            }
        }
        #endregion
    }
}
