using System.Collections.Generic;
using UnityEngine;
using Xi.Tools;

namespace Xi.Framework
{
    public abstract class SceneObjectReference
    {
        public abstract int EnumIntValue { get; }
        public GameObject sceneObject;
    }
    public interface ISceneObjectReferenceHolder { T GetSceneObjectReference<T>(string objectEnumName, int intValue) where T : Component; }
    public abstract class SceneObjectReferenceHolder<TSceneObjectReference> : MonoBehaviour, ISceneObjectReferenceHolder where TSceneObjectReference : SceneObjectReference
    {
        protected abstract List<TSceneObjectReference> SceneObjectReferences { get; }
        protected T GetSceneObjectReference<T>(string objectEnumName, int intValue) where T : Component
        {
            var reference = SceneObjectReferences.Find(item => item.EnumIntValue == intValue);
            if (reference == null)
            {
                XiLogger.LogError($"Object of type {objectEnumName} not found!");
                return null;
            }

            if (!reference.sceneObject.TryGetComponent<T>(out var component))
            {
                XiLogger.LogError($"{reference.sceneObject.name}  not found {nameof(T)} component");
                return null;
            }

            return component;
        }

        T ISceneObjectReferenceHolder.GetSceneObjectReference<T>(string objectEnumName, int intValue)
            => GetSceneObjectReference<T>(objectEnumName, intValue);
    }
}
