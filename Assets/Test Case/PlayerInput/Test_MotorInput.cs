using UnityEngine;
using Xi.Framework;
using Xi.Gameplay.Character.InputHandler;

namespace Xi.TestCase
{
    public class Test_MotorInput : MonoBehaviour
    {
        public PlayerInputHandlerMotor input;

        private void Awake()
        {
            InputManager.Instance.SetInputEnable_Player(true);
            input = new PlayerInputHandlerMotor(InputManager.Instance.Player);
        }

        private void OnEnable() => input.AddInputListner();

        private void Update() => input.OnUpdate(Time.deltaTime);

        private void OnDisable() => input.RemoveInputListner();
    }
}
