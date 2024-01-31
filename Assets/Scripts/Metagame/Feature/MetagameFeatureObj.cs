using Cinemachine;
using UnityEngine;

namespace Xi.Metagame.Feature
{
    public abstract class MetagameFeatureObj : MonoBehaviour
    {
        [field: SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
        [field: SerializeField] public MetagameFeatureEnum FeatureEnum { get; private set; }
        public void Setup() => VirtualCamera.enabled = false;
        public void SetupActive()
        {
            VirtualCamera.enabled = true;
            OnFeatureActive();
        }
        public void SetupDeactive()
        {
            VirtualCamera.enabled = false;
            OnFeatureDeactive();
        }
        protected abstract void OnFeatureActive();
        protected abstract void OnFeatureDeactive();
    }
}
