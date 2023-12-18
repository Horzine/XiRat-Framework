using Cysharp.Threading.Tasks;
using Xi.Config;

namespace Xi.Framework
{
    public class InputManager : MonoSingleton<InputManager>, ISingleton
    {
        private readonly InputActionConfig _actionConfig = new();
        public InputActionConfig.PlayerActions Player { get; private set; }
        public InputActionConfig.UiActions Ui { get; private set; }
        public InputActionConfig.HudActions Hud { get; private set; }

        void ISingleton.OnCreate()
        {
            Player = _actionConfig.Player;
            Ui = _actionConfig.Ui;
            Hud = _actionConfig.Hud;
            _actionConfig.Enable();
        }

        public async UniTask InitAsync() => await UniTask.Yield();

        public void SetInputEnable_Player(bool enable)
        {
            if (enable)
            {
                Player.Enable();
            }
            else
            {
                Player.Disable();
            }
        }
        public void SetInputEnable_UI(bool enable)
        {
            if (enable)
            {
                Ui.Enable();
            }
            else
            {
                Ui.Disable();
            }
        }
        public void SetInputEnable_Hud(bool enable)
        {
            if (enable)
            {
                Hud.Enable();
            }
            else
            {
                Hud.Disable();
            }
        }
    }
}
