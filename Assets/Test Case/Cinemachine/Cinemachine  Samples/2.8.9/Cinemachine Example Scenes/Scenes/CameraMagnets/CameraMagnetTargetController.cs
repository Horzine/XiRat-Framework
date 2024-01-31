using Cinemachine;
using UnityEngine;

public class CameraMagnetTargetController : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;

    private int playerIndex;
    private CameraMagnetProperty[] cameraMagnets;

    // Start is called before the first frame update
    private void Start()
    {
        cameraMagnets = GetComponentsInChildren<CameraMagnetProperty>();
        playerIndex = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 1; i < targetGroup.m_Targets.Length; ++i)
        {
            float distance = (targetGroup.m_Targets[playerIndex].target.position -
                              targetGroup.m_Targets[i].target.position).magnitude;
            targetGroup.m_Targets[i].weight = distance < cameraMagnets[i - 1].Proximity
                ? cameraMagnets[i - 1].MagnetStrength *
                                                  (1 - (distance / cameraMagnets[i - 1].Proximity))
                : 0;
        }
    }
}
