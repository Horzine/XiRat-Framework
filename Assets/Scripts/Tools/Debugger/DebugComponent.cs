using UnityEngine;

namespace Xi.Tools
{
    internal abstract class DebugComponent<T> : IDebugComponent where T : Component
    {
        protected T target;

        object IDebugComponent.Target
        {
            set => target = (T)value;
        }

        protected abstract void OnSceneWindow();

        void IDebugComponent.OnSceneWindow() => OnSceneWindow();
    }
}