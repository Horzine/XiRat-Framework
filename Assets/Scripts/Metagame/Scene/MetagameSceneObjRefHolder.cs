using System.Collections.Generic;
using UnityEngine;
using Xi.Framework;
using static Xi.Metagame.MetagameSceneObjRefHolder;

namespace Xi.Metagame
{
    public enum MetagameSceneObjectEnum
    {
    }

    public class MetagameSceneObjRefHolder : SceneObjectReferenceHolder<MetagameSceneObjectReference>
    {
        [System.Serializable]
        public class MetagameSceneObjectReference : SceneObjectReference
        {
            public MetagameSceneObjectEnum typeEnum;
            public override int EnumIntValue => (int)typeEnum;
        }
        [SerializeField] private List<MetagameSceneObjectReference> _sceneObjectReferences = new();

        protected override List<MetagameSceneObjectReference> SceneObjectReferences => _sceneObjectReferences;

        public T GetSceneObjectReference<T>(MetagameSceneObjectEnum objectType) where T : Component
            => GetSceneObjectReference<T>(objectType.ToString(), (int)objectType);
    }   
}
