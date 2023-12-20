using System.Collections.Generic;
using UnityEngine;
using Xi.Framework;
using static Xi.Gameplay.Scene.GameplaySceneObjRefHolder;

namespace Xi.Gameplay.Scene
{
    public enum GameplaySceneObjectEnum
    {
    }

    public class GameplaySceneObjRefHolder : SceneObjectReferenceHolder<GameplaySceneObjectReference>
    {
        [System.Serializable]
        public class GameplaySceneObjectReference : SceneObjectReference
        {
            public GameplaySceneObjectEnum typeEnum;
            public override int EnumIntValue => (int)typeEnum;
        }
        [SerializeField] private List<GameplaySceneObjectReference> _sceneObjectReferences = new();

        protected override List<GameplaySceneObjectReference> SceneObjectReferences => _sceneObjectReferences;

        public T GetSceneObjectReference<T>(GameplaySceneObjectEnum objectType) where T : Component
            => GetSceneObjectReference<T>(objectType.ToString(), (int)objectType);
    }
}
