using UnityEngine;
using UnityEngine.InputSystem;
using Xi.Config;
using static UnityEngine.InputSystem.InputAction;

namespace Xi.TestCase
{
    public class Test_InputSystem_Player : MonoBehaviour, InputActionConfig.IPlayerActions
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

        void InputActionConfig.IPlayerActions.OnFire(CallbackContext context) => print(nameof(InputActionConfig.IPlayerActions.OnFire));
        void InputActionConfig.IPlayerActions.OnLook(CallbackContext context) => print(nameof(InputActionConfig.IPlayerActions.OnLook));
        void InputActionConfig.IPlayerActions.OnMove(CallbackContext context) => print(nameof(InputActionConfig.IPlayerActions.OnMove));
        void InputActionConfig.IPlayerActions.OnReload(CallbackContext context) => print(nameof(InputActionConfig.IPlayerActions.OnReload));

        private void Start()
        {
            // var playerInput = FindObjectOfType<PlayerInput>();
            // var actions = playerInput.actions;
            // var map = actions.FindActionMap("Player");
            // var moveAction = map.FindAction("Move");
            // moveAction.performed += TTTTT;

            var inputObj = new InputActionConfig();
            inputObj.Enable();
            inputObj.Player.SetCallbacks(this);
        }
    }
}
