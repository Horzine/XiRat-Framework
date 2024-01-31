using UnityEngine;

public class ActivateOnKeypress : MonoBehaviour
{
    public KeyCode ActivationKey = KeyCode.LeftControl;
    public int PriorityBoostAmount = 10;
    public GameObject Reticle;
    private Cinemachine.CinemachineVirtualCameraBase vcam;
    private bool boosted = false;

    private void Start() => vcam = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();

    private void Update()
    {
        if (vcam != null)
        {
            if (Input.GetKey(ActivationKey))
            {
                if (!boosted)
                {
                    vcam.Priority += PriorityBoostAmount;
                    boosted = true;
                }
            }
            else if (boosted)
            {
                vcam.Priority -= PriorityBoostAmount;
                boosted = false;
            }
        }

        if (Reticle != null)
        {
            Reticle.SetActive(boosted);
        }
    }
}
