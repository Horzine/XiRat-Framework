using UnityEngine;

namespace Xi.Framework
{
    public class UiRootObject : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Canvas _cameraModeCanvas;
        [SerializeField] private Canvas _overlayModeCanvas;
        public Transform CameraModeCanvasTsf => _cameraModeCanvas.transform;
        public Transform OverlayModeCanvasTsf => _overlayModeCanvas.transform;
        public Camera UICamera => _uiCamera;
    }
}
