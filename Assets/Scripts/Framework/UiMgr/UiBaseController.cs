using Cysharp.Threading.Tasks;

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
        protected TWindow _windowObject;
        public abstract UiEnum UiEnum { get; }
        private UiRootObject _uiRootObject;
        UiEnum IUiController.UiEnum => UiEnum;
        UiRootObject IUiController.UiRootObject { set => _uiRootObject = value; }
        public abstract void ForceReleaseWindow();
        protected async UniTask ShowAsync()
        {

        }
        protected async UniTask CloseAsync()
        {

        }
    }
}
