using Xi.Framework;
using Xi.Metagame.Ui;

namespace Xi.Metagame
{
    public class MetagameGameInstanceObject : GameInstanceObject
    {
        public MetagameGameInstance MetagameGameInstance => _gameInstance as MetagameGameInstance;
        private MainMenuWindowController _mainMenuCtrl;

        public void Start()
        {
            _mainMenuCtrl = UiManager.Instance.GetController<MainMenuWindowController>(UiEnum.Metagame_MainMenu);
            _mainMenuCtrl.Open();
        }

        public void OnDestroy()
        {
            _mainMenuCtrl?.Close();
        }
    }
}
