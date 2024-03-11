using UnityEngine;
using Xi.Gameplay.Character.Controller;

namespace Xi.TestCase
{
    public class Test_FpsController : MonoBehaviour
    {
        private FpsController _target;
        private void Awake() => _target = FindObjectOfType<FpsController>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _target.SetupSnappingRootPosition(new Vector3(3, 0, 3));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _target.SetupControllerPosition(new Vector3(3, 0, 3));
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {

            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {

            }
        }
    }
}
