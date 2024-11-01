﻿using UnityEngine;

namespace Xi.Framework
{
    public interface IGameInstance
    {
        internal string SceneName { get; set; }
        internal void OnCreate();
        internal void OnAfterNewSceneActive(IGameInstance oldGameInstance);
        internal void WillBeReplaced();
    }
    public abstract class GameInstance : IGameInstance
    {
        public abstract string SceneName { get; protected set; }
        protected GameInstanceObject _gameInstanceObject;
        protected abstract void OnCreate();
        private void OnAfterNewSceneActive(IGameInstance oldGameInstance)
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
        void IGameInstance.OnAfterNewSceneActive(IGameInstance oldGameInstance) => OnAfterNewSceneActive(oldGameInstance);
        void IGameInstance.WillBeReplaced() => WillBeReplaced();
        #endregion
    }
}