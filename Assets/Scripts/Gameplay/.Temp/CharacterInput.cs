using UnityEngine;

namespace Xi.Gameplay.Character
{
/*
CharacterInput 类
这个类是角色输入控制器，处理玩家的输入操作。

moveInput: 移动输入。
rawLookInput: 视角输入。
lookInput: 经过灵敏度等因素处理后的视角输入。
sprintInput、tacticalSprintInput、jumpInput、crouchInput等：是否进行冲刺、战术冲刺、跳跃、蹲伏等输入。
rawSprintInput、rawTacticalSprintInput：未经处理的冲刺、战术冲刺输入。
Start(): 初始化输入操作。
Update(): 更新输入状态，包括移动、视角等。
AddInputListener(): 添加输入监听器。
AddLookValue(Vector2 value): 添加视角偏移量。
*/
    public class CharacterInput : MonoBehaviour
    {
        /// <summary>
        /// Main input actions class.
        /// </summary>
        public Controls controls;

        /// <summary>
        /// The target FPS Controller.
        /// </summary>
        public CharacterManager characterManager { get; protected set; }

        /// <summary>
        /// The current main camera (Cashed)
        /// </summary>
        public Camera mainCamera { get; protected set; }

        /// <summary>
        /// The result value (Vector2) of the move (Forward, Backward, Right & Left).
        /// </summary>
        public Vector2 moveInput { get; protected set; }

        /// <summary>
        /// The result value (Vector2) of the camera look (Up, Down, Right & Left).
        /// </summary>
        public Vector2 rawLookInput { get; protected set; }

        /// <summary>
        /// The result value (Vector 2) of the camera look (Up, Down, right, and Left) multiplied with senstivity and other factors.
        /// </summary>
        public Vector2 lookInput { get; protected set; }

        /// <summary>
        /// Is performing sprint input?
        /// </summary>
        public bool sprintInput { get; set; }

        /// <summary>
        /// Is performing tac sprint input?
        /// </summary>
        public bool tacticalSprintInput { get; set; }

        /// <summary>
        /// Using this raw input because the check douple clickes method needs a field not a property.
        /// </summary>
        [HideInInspector] public bool rawTacticalSprintInput;

        /// <summary>
        /// Is performing jump input?
        /// </summary>
        public bool jumpInput { get; set; }

        /// <summary>
        /// Is performing crouch input?
        /// </summary>
        public bool crouchInput { get; set; }

        /// <summary>
        /// Is performing lean right input?
        /// </summary>
        public bool leanRightInput { get; set; }

        /// <summary>
        /// Is performing lean Left input?
        /// </summary>
        public bool leanLeftInput { get; set; }

        /// <summary>
        /// The value added from the function AddLookAmount(). Setting this to anything will rotate the camera using the value.
        /// </summary>
        public Vector2 addedLookValue { get; set; }

        /// <summary>
        /// Using this value to always get sprint input regardless of the player state.
        /// This is used to then filter the input and choose when to use it.
        /// </summary>
        [HideInInspector] public bool rawSprintInput;

        private float lastSprintClickTime;

        private void Start()
        {
            //Initinaling input actins for this class.
            controls = new Controls();
            controls.Player.Enable();

            characterManager = GetComponent<CharacterManager>();

            //Using event logic to allow external disabling of the input.
            //Example: You could set the sprint value from the external class without it resting itself.
            AddInputListner();
        }

        protected void Update()
        {
            //Read values in the update. You can't change input values from the external class as it will reset itself.
            moveInput = controls.Player.Move.ReadValue<Vector2>();
            rawLookInput = controls.Player.Look.ReadValue<Vector2>();

            //Choose when to turn off sprinting input and when to use it.
            if (moveInput.y < 0 || crouchInput)
            {
                tacticalSprintInput = false;
                sprintInput = false;

            }
            else
            {
                tacticalSprintInput = rawTacticalSprintInput;
                sprintInput = rawSprintInput;
            }

            if (tacticalSprintInput)
            {
                sprintInput = false;
            }

            //Update tac sprint input.
            controls.Player.TacticalSprint.HasDoupleClicked(ref rawTacticalSprintInput, ref lastSprintClickTime);

            //Jump input
            if (PauseMenu.Instance)
            {
                if (!PauseMenu.Instance.paused)
                {
                    jumpInput = controls.Player.Jump.triggered;
                }
            }
            else
            {
                jumpInput = controls.Player.Jump.triggered;
            }

            //Find the main camera if it's null only.
            var lookInput = new Vector2();
            mainCamera = Camera.main;

            if (mainCamera)
            {
                if (PauseMenu.Instance)
                {
                    float sensitivity = 1;
                    if (characterManager.character != null)
                    {
                        sensitivity = characterManager.character.sensitivity;
                    }

                    float dynamicSensitivity = !PauseMenu.Instance.paused ? Time.fixedDeltaTime * (sensitivity * mainCamera.fieldOfView / 179) / 50 : 0;
                    lookInput = addedLookValue + (rawLookInput * dynamicSensitivity);
                    addedLookValue = Vector2.zero;
                }
                else
                {
                    float sensitivity = 1;
                    if (characterManager.character != null)
                    {
                        sensitivity = characterManager.character.sensitivity;
                    }

                    float dynamicSensitivity = Time.fixedDeltaTime * (sensitivity * mainCamera.fieldOfView / 179) / 50;
                    lookInput = addedLookValue + (rawLookInput * dynamicSensitivity);

                    addedLookValue = Vector2.zero;
                }
            }
            else
            {
                float sensitivity = 1;
                if (characterManager.character != null)
                {
                    sensitivity = characterManager.character.sensitivity;
                }

                lookInput = (sensitivity * addedLookValue) + (rawLookInput / 50);

                addedLookValue = Vector2.zero;
            }

            this.lookInput = new Vector2(lookInput.x * FPSFrameworkUtility.xSensitivityMultiplier, lookInput.y * FPSFrameworkUtility.ySensitivityMultiplier) * FPSFrameworkUtility.sensitivityMultiplier;
        }

        protected void AddInputListner()
        {
            //Sprint
            controls.Player.Sprint.performed += context =>
            {
                rawSprintInput = true;
            };

            controls.Player.Sprint.canceled += context =>
            {
                rawSprintInput = false;
            };

            //Crouch
            controls.Player.Crouch.performed += context =>
            {
                crouchInput = true;
            };

            controls.Player.Crouch.canceled += context =>
            {
                crouchInput = false;
            };

            //Lean Right
            controls.Player.LeanRight.performed += context =>
            {
                leanRightInput = true;
            };

            controls.Player.LeanRight.canceled += context =>
            {
                leanRightInput = false;
            };

            //Lean Left
            controls.Player.LeanLeft.performed += context =>
            {
                leanLeftInput = true;
            };

            controls.Player.LeanLeft.canceled += context =>
            {
                leanLeftInput = false;
            };
        }

        /// <summary>
        /// Adds amount of rotation from the given Vector2 value.
        /// </summary>
        /// <param name="value"></param>
        public void AddLookValue(Vector2 value) => addedLookValue += value;
    }
}