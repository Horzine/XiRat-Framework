using Xi.Framework;
using Xi.Metagame.Client.System;
using Xi.Metagame.Client.System.User;
using Xi.Metagame.Feature;
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
            var userSystem = MetagameGameInstance.Client.GetSystem<MetagameSystem_User>(MetagameSystemNameConst.kUser);
            var featureController = MetagameGameInstance.SceneObjRefHolder.GetSceneObjectReference<MetagameFeatureController>(Scene.MetagameSceneObjectEnum.Feature_Controller);
            _mainMenuCtrl.Open(userSystem, featureController);
        }

        public void OnDestroy() => _mainMenuCtrl?.Close();
    }
}
