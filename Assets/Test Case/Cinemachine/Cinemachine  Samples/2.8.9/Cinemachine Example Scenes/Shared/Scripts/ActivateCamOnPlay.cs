using UnityEngine;

namespace Cinemachine.Examples
{

    [AddComponentMenu("")] // Don't display in add component menu
    public class ActivateCamOnPlay : MonoBehaviour
    {
        public CinemachineVirtualCameraBase vcam;

        // Use this for initialization
        private void Start()
        {
            if (vcam)
            {
                vcam.MoveToTopOfPrioritySubqueue();
            }
        }
    }
}