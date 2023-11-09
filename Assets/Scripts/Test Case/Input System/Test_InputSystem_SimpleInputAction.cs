using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Xi.TestCase
{
    public class Test_InputSystem_SimpleInputAction : MonoBehaviour
    {
        public InputAction _reloadAction;
        public InputAction _walkAction;

        // Invoke Callback Option 1: Add event 
        private void Awake()
        {
            _reloadAction.performed += Fire;
            _walkAction.performed += Walk;
        }

        // Invoke Callback Option2: Update detect and call
        private void Update()
        {
            if (_reloadAction.triggered)
            {
                // Fire();
            }

            if (_walkAction.triggered)
            {
                // Walk();
            }
        }

        private void OnEnable()
        {
            _reloadAction.Enable();
            _walkAction.Enable();
        }

        private void OnDisable()
        {
            _reloadAction.Disable();
            _walkAction.Disable();
        }

        private void Fire(CallbackContext ctx)
        {
            //...
        }

        private void Walk(CallbackContext ctx)
        {
            //...
        }
    }
}
