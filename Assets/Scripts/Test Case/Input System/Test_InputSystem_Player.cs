using UnityEngine;
using UnityEngine.InputSystem;
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

        private void Update() => KeyWasPressedThisFrame();

        private void KeyWasPressedThisFrame() => print(Keyboard.current.spaceKey.wasPressedThisFrame);


        void Test()
        {
            var playerInput = GetComponent<PlayerInput>();
           var reloadAction =  playerInput.actions.FindActionMap("player").FindAction("Reload");
        }
    }
}
