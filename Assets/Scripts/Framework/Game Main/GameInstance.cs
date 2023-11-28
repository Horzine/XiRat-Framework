using UnityEngine;

namespace Xi.Framework
{
    public interface IGameInstance
    {
        string SceneName { get; set; }
        void OnCreate();
        void OnNewSceneActive(IGameInstance oldGameInstance);
        void WillBeReplaced();
    }
    public abstract class GameInstance : IGameInstance
    {
        public abstract string SceneName { get; protected set; }
        protected GameInstanceObject _gameInstanceObject;
        protected abstract void OnCreate();
        private void OnNewSceneActive(IGameInstance oldGameInstance)
        {
            CreateAndSetupGameInstanceObject();
            AfterNewSceneActiveAndCreateObject(oldGameInstance, _gameInstanceObject);
        }
        protected abstract void AfterNewSceneActiveAndCreateObject(IGameInstance oldGameInstance, GameInstanceObject gameInstanceObject);
        protected abstract void WillBeReplaced();
        protected abstract GameInstanceObject AddGameInstanceObjectComponent(GameObject go);
        private void CreateAndSetupGameInstanceObject()
        {
            var go = new GameObject($"{SceneName} GameInstance");
            go.transform.SetSiblingIndex(0);
            _gameInstanceObject = AddGameInstanceObjectComponent(go);
            _gameInstanceObject.Init(this);
        }

        #region Interface IGameInstance
        string IGameInstance.SceneName { get => SceneName; set => SceneName = value; }
        void IGameInstance.OnCreate() => OnCreate();
        void IGameInstance.OnNewSceneActive(IGameInstance oldGameInstance) => OnNewSceneActive(oldGameInstance);
        void IGameInstance.WillBeReplaced() => WillBeReplaced();
        #endregion
    }
}