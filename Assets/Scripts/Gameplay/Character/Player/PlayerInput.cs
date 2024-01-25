using UnityEngine;
using Xi.Config;
using Xi.Extend.UnityExtend;

namespace Xi.Gameplay.Character.Player
{
    public class PlayerInput
    {
        public Vector2 MoveInput { get; private set; }
        public Vector2 RawLookInput { get; private set; }
        public bool CrouchInput { get; private set; }
        private bool _rawTacticalSprintInput;
        public bool TacticalSprintInput { get; private set; }
        private bool _rawSprintInput;
        public bool SprintInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool SlideInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        private InputActionConfig.PlayerActions _player;
        private Vector2 _addedLookValue;
        private float _lastSprintClickTime;

        public PlayerInput(InputActionConfig.PlayerActions player)
        {
            _player = player;
            AddInputListner();
        }

        public void OnUpdate(float deltaTime)
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

        private void AddInputListner()
        {
            _player.Sprint.performed += context =>
            {
                _rawSprintInput = true;
            };

            _player.Sprint.canceled += context =>
            {
                _rawSprintInput = false;
            };

            _player.Crouch.performed += context =>
            {
                CrouchInput = true;
            };

            _player.Crouch.canceled += context =>
            {
                CrouchInput = false;
            };
        }

        public void AddLookValue(Vector2 value) => _addedLookValue += value;
    }

    public class TempSetting
    {
        public static float ySensitivityMultiplier = 1;
        public static float xSensitivityMultiplier = 1;
        public static float sensitivityMultiplier = 1;

        public static float CharacterSensitivity { get; private set; } = 1f;
    }
}
