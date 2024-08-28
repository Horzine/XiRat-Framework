using System;
using UnityEngine;
using Xi.Config;
using Xi.Extension.UnityExtension;
using static UnityEngine.InputSystem.InputAction;

namespace Xi.Gameplay.Character.InputHandler
{
    [Serializable]
    public class PlayerInputHandlerMotor : PlayerInputHandler
    {
        [field: SerializeField] public Vector2 MoveInput { get; private set; }
        [field: SerializeField] public Vector2 RawLookInput { get; private set; }
        [field: SerializeField] public bool CrouchInput { get; private set; }
        private bool _rawTacticalSprintInput;
        [field: SerializeField] public bool TacticalSprintInput { get; private set; }
        private bool _rawSprintInput;
        [field: SerializeField] public bool SprintInput { get; private set; }
        [field: SerializeField] public bool JumpInput { get; private set; }
        [field: SerializeField] public bool SlideInput { get; private set; }
        [field: SerializeField] public Vector2 LookInput { get; private set; }

        private Vector2 _addedLookValue;
        private float _lastSprintClickTime;

        public PlayerInputHandlerMotor(InputActionConfig.PlayerActions player) : base(player) { }

        public override void OnUpdate(float deltaTime)
        {
            MoveInput = _player.Move.ReadValue<Vector2>();
            RawLookInput = _player.Look.ReadValue<Vector2>();

            if (MoveInput.y < 0 || CrouchInput)
            {
                TacticalSprintInput = false;
                SprintInput = false;
            }
            else
            {
                TacticalSprintInput = _rawTacticalSprintInput;
                SprintInput = _rawSprintInput;
            }

            if (TacticalSprintInput)
            {
                SprintInput = false;
            }

            _player.TacticalSprint.HasDoubleClicked(ref _rawTacticalSprintInput, ref _lastSprintClickTime);

            JumpInput = _player.Jump.triggered;
            SlideInput = _player.Slide.triggered;

            var mainCamera = Camera.main;
            float sensitivity = TempSetting.CharacterSensitivity;
            float dynamicSensitivity = Time.fixedDeltaTime * (sensitivity * mainCamera.fieldOfView / 179) / 50;
            var lookInput = _addedLookValue + (RawLookInput * dynamicSensitivity);
            _addedLookValue = Vector2.zero;
            LookInput = new Vector2(lookInput.x * TempSetting.xSensitivityMultiplier, lookInput.y * TempSetting.ySensitivityMultiplier) * TempSetting.sensitivityMultiplier;
        }

        public override void AddInputListner()
        {
            _player.Sprint.performed += SprintCallback;
            _player.Sprint.canceled += SprintCallback;
            _player.Crouch.performed += CrouchCallback;
            _player.Crouch.canceled += CrouchCallback;
        }

        public override void RemoveInputListner()
        {
            _player.Sprint.performed -= SprintCallback;
            _player.Sprint.canceled -= SprintCallback;
            _player.Crouch.performed -= CrouchCallback;
            _player.Crouch.canceled -= CrouchCallback;
        }

        public void AddLookValue(Vector2 value) => _addedLookValue += value;

        private void SprintCallback(CallbackContext context)
        {
            if (context.performed)
            {
                _rawSprintInput = true;
            }

            if (context.canceled)
            {
                _rawSprintInput = false;
            }
        }

        private void CrouchCallback(CallbackContext context)
        {
            if (context.performed)
            {
                CrouchInput = true;
            }

            if (context.canceled)
            {
                CrouchInput = false;
            }
        }
    }
    public class TempSetting
    {
        public static float ySensitivityMultiplier = 1;
        public static float xSensitivityMultiplier = 1;
        public static float sensitivityMultiplier = 100;

        public static float CharacterSensitivity { get; private set; } = 100f;
    }
}
