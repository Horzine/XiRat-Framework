using Xi.Framework;
using Xi.Metagame.Client.System;
using Xi.Metagame.Client.System.User;
using Xi.Metagame.Ui;

namespace Xi.Metagame.Main
{
    public class MetagameGameInstanceObject : GameInstanceObject
    {
        public MetagameGameInstance MetagameGameInstance => _gameInstance as MetagameGameInstance;
        private MainMenuWindowController _mainMenuCtrl;

        public void Start()
        {
            _mainMenuCtrl = UiManager.Instance.GetController<MainMenuWindowController>(UiEnum.Metagame_MainMenu);
            var userSystem = GameMain.Instance.GetMetagameSystem<MetagameSystem_User>(MetagameSystemNameConst.kUser);
            _mainMenuCtrl.Open(userSystem);
        }

        public void OnDestroy() => _mainMenuCtrl?.Close();
    }
}
