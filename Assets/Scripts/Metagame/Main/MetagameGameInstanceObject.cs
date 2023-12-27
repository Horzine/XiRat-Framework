using Xi.Framework;
using Xi.Metagame.Client.System;
using Xi.Metagame.Client.System.User;
using Xi.Metagame.Feature;
using Xi.Metagame.Scene;
using Xi.Metagame.Ui;

namespace Xi.Metagame.Main
{
    public class MetagameGameInstanceObject : GameInstanceObject
    {
        public MetagameGameInstance MetagameGameInstance => _gameInstance as MetagameGameInstance;
        private MainMenuWindowController _mainMenuCtrl;

        public void Start()
        {
            var featureController = MetagameGameInstance.SceneObjRefHolder.GetSceneObjectReference<MetagameFeatureController>(MetagameSceneObjectEnum.Feature_Controller);
            featureController.ActiveFeature(MetagameFeatureEnum.Center);

            _mainMenuCtrl = UiManager.Instance.GetController<MainMenuWindowController>(UiEnum.Metagame_MainMenu);
            var userSystem = MetagameGameInstance.Client.GetSystem<MetagameSystem_User>(MetagameSystemNameConst.kUser);
            _mainMenuCtrl.Open(userSystem, featureController);
        }

        public void OnDestroy() => _mainMenuCtrl?.Close();
    }
}
