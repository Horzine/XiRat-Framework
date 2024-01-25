using UnityEngine;
using UnityEngine.InputSystem;
using Xi.Config;
using static UnityEngine.InputSystem.InputAction;

namespace Xi.TestCase
{
    public class Test_InputSystem_Player : MonoBehaviour
    {
        public void InputMove(CallbackContext context)
        {
            var moveInput = context.ReadValue<Vector2>();
            print($"InputMove: {moveInput}");
        }

        public void TTTTT(CallbackContext context)
        {
            var moveInput = context.ReadValue<Vector2>();
            print($"TTTTT: {moveInput}");
        }

        //  private void Update() => KeyWasPressedThisFrame();

        private void KeyWasPressedThisFrame() => print(Keyboard.current.spaceKey.wasPressedThisFrame);

        private void Start()
        {
            var playerInput = FindObjectOfType<PlayerInput>();
            var actions = playerInput.actions;
            var map = actions.FindActionMap("Player");
            var moveAction = map.FindAction("Move");
            moveAction.performed += TTTTT;

            // var inputObj = new InputActionConfig();
            // inputObj.Enable();
            // inputObj.Player.SetCallbacks(this);
        }
    }
}
