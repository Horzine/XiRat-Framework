using System;
using System.Collections.Generic;
using UnityEngine;
using Xi.Extend.UnityExtend;
using Xi.Framework;
using Xi.Gameplay.Character.InputHandler;

namespace Xi.Gameplay.Character.Controller
{
    [RequireComponent(typeof(CharacterController))]
    public partial class FpsController : MonoBehaviour
    {
        [SerializeField] private Transform _testRenderer;
        [SerializeField] private FpsControllerConfig _config;
        public PlayerInputHandlerMotor _motorInput;
        public PlayerInputHandlerCombat _cambotInput;
        private List<PlayerInputHandler> _playerInputHandlers;
        private CharacterController _unityCharacterController;

        private Transform _selfTsf;
        private float defaultHeight;
        private float defaultstepOffset;
        private float speed;
        private float outputWalkSpeed;
        private float outputSprintSpeed;
        private float outputTacticalSprintSpeed;

        private const float kCrouchLerpSpeed = 15f;

        //input velocity
        private Vector3 desiredVelocityRef;
        private Vector3 desiredVelocity;
        private Vector3 slideVelocity;

        //out put velocity
        private Vector3 velocity;

        [Header("Slopes")]
        public bool slideDownSlopes = true;
        public float slopeSlideSpeed = 1;

        private Vector3 slopeDirection;

        private Transform SnappingRoot { get; set; }
        public CollisionFlags CollisionFlags { get; private set; }
        public bool IsOnGround => _unityCharacterController.isGrounded;

        private void Awake()
        {
            _selfTsf = transform;
            _unityCharacterController = GetComponent<CharacterController>();
            _motorInput = new(InputManager.Instance.Player);
            _cambotInput = new(InputManager.Instance.Player);

            _playerInputHandlers = new List<PlayerInputHandler>
            {
                _motorInput,
                _cambotInput,
            };

            SnappingRoot = new GameObject("SnappingRoot").transform;
            _selfTsf.AddChildAndSetIdentity(SnappingRoot);
            UpdateSnapRootPosition();

            // characterManager.orientation = Orientation;
            // characterManager.Setup(Actor, controller, cameraManager, Orientation);

            ResetSpeed();

            defaultHeight = _unityCharacterController.height;
            defaultstepOffset = _unityCharacterController.stepOffset;
            _unityCharacterController.skinWidth = _unityCharacterController.radius / 10;
        }

        private void UpdateSnapRootPosition() => SnappingRoot.localPosition = new Vector3(0, -_unityCharacterController.height / 2, 0);

        private void OnEnable()
        {
            InputManager.Instance.SetInputEnable_Player(true);
            foreach (var item in _playerInputHandlers)
            {
                item.AddInputListner();
            }
        }

        private void Update()
        {
            DoUpdateInputHandlers();

            //slide down slope if on maxed angle slope
            if (slideDownSlopes && OnMaxedAngleSlope())
            {
                slideVelocity += slopeSlideSpeed * Time.deltaTime * new Vector3(slopeDirection.x, -slopeDirection.y, slopeDirection.z);
            }
            else
            {
                //reset velocity if not on slope
                slideVelocity = Vector3.zero;
            }
            //update desiredVelocity in order to normalize it and smooth the movement
            desiredVelocity = slideVelocity + Vector3.SmoothDamp(desiredVelocity,
                ((SlopeDirection() * _motorInput.MoveInput.y) + (_selfTsf.right * _motorInput.MoveInput.x)).normalized * speed, ref desiredVelocityRef, _config.acceleration);

            //set controller height according to if player is crouching
            float crouchHeight = _config.crouchHeight;
            float targetHeight = _motorInput.CrouchInput ? crouchHeight : defaultHeight;
            float newHeight = Mathf.Lerp(_unityCharacterController.height, targetHeight, Time.deltaTime * kCrouchLerpSpeed);
            _unityCharacterController.height = newHeight;
            float targetRenderLocalPosY = _motorInput.CrouchInput ? -(defaultHeight - crouchHeight) / 2 : 0;
            float newRenderLocalPosY = Mathf.Lerp(_testRenderer.localPosition.y, targetRenderLocalPosY, Time.deltaTime * kCrouchLerpSpeed);
            _testRenderer.localPosition = new Vector3(_testRenderer.localPosition.x, newRenderLocalPosY, _testRenderer.localPosition.z);

            _unityCharacterController.stepOffset = !_unityCharacterController.isGrounded || OnSlope() ? 0 : defaultstepOffset;

            UpdateSnapRootPosition();

            //copy desiredVelocity x, z with normalized values
            velocity.x = desiredVelocity.x;
            velocity.z = desiredVelocity.z;

            //update speed according to if player is holding sprint
            if (_motorInput.SprintInput && !_motorInput.TacticalSprintInput)
            {
                speed = outputSprintSpeed;
            }
            else if (!_motorInput.TacticalSprintInput)
            {
                speed = outputWalkSpeed;
            }

            if (_motorInput.TacticalSprintInput)
            {
                speed = outputTacticalSprintSpeed;
            }

            if (IsOnGround)
            {
                HandleOnGround();
            }
            else
            {
                HandleOnAir();
            }

            //move and update CollisionFlags in order to check if collision is coming from above to center or bottom
            CollisionFlags = _unityCharacterController.Move(velocity * Time.deltaTime);

            //move camera according to controller height
            // _Camera.position = transform.position + ((transform.up * _characterController.height / 2) + _config.offset);

            //rotate camera
            // UpdateCameraRotation();
            // TacticalSprintAmount = _motorInput.TacticalSprintInput ? 1 : 0;
        }

        private void HandleOnGround()
        {
            velocity.y = _config.stickToGroundForce * Physics.gravity.y;

            if (_motorInput.JumpInput)
            {
                velocity.y = _config.jumpHeight * -Physics.gravity.y;
            }
        }

        private void HandleOnAir()
        {
            if (Mathf.Abs(velocity.y) < _config.maxFallSpeed)
            {
                velocity += Time.deltaTime * _config.gravityMultiply * Physics.gravity;
            }
        }

        private void DoUpdateInputHandlers()
        {
            foreach (var item in _playerInputHandlers)
            {
                item.OnUpdate(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance)
            {
                InputManager.Instance.SetInputEnable_Player(false);
            }

            foreach (var item in _playerInputHandlers)
            {
                item.RemoveInputListner();
            }
        }

        public void SetSpeed(float walk, float sprint, float tacSprint)
        {
            outputWalkSpeed = walk;
            outputSprintSpeed = sprint;
            outputTacticalSprintSpeed = tacSprint;
        }

        public void ResetSpeed() => SetSpeed(_config.walkSpeed, _config.sprintSpeed, _config.tacticalSprintSpeed);

        public bool OnMaxedAngleSlope()
        {
            if (_unityCharacterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out var hit, _unityCharacterController.height))
            {
                slopeDirection = hit.normal;
                return Vector3.Angle(slopeDirection, Vector3.up) > _unityCharacterController.slopeLimit;
            }

            return false;
        }

        public Vector3 SlopeDirection()
        {
            //setup a raycast from position to down at the bottom of the collider
            if (Physics.Raycast(_selfTsf.position, Vector3.down, out var slopeHit, (_unityCharacterController.height / 2) + 0.1f))
            {
                //get the direction result according to slope normal
                return Vector3.ProjectOnPlane(_selfTsf.forward, slopeHit.normal);
            }

            //if not on slope then slope is forward ;)
            return _selfTsf.forward;
        }

        public float SlopeAngle()
        {
            //setup a raycast from position to down at the bottom of the collider
            if (Physics.Raycast(transform.position, Vector3.down, out var slopeHit))
            {
                //get the direction result according to slope normal
                return (Vector3.Angle(Vector3.down, slopeHit.normal) - 180) * -1;
            }

            //if not on slope then slope is forward ;)
            return 0;
        }

        //check if slope angle is more than 0
        public bool OnSlope() => SlopeAngle() > 0;

    }

    [Serializable]
    public class FpsControllerConfig
    {
        [Tooltip("The amount of time needed to walk or sprint in full speed.")]
        public float acceleration = 0.1f;
        [Tooltip("The amount of meters to move per second while walking.")]
        public float walkSpeed = 5;
        [Tooltip("The amount of meters to move per second while sprinting.")]
        public float sprintSpeed = 10;
        [Tooltip("The amount of meters to move per second while tactical walking.")]
        public float tacticalSprintSpeed = 11;
        [Tooltip("Player height while crouching.")]
        public float crouchHeight = 1.2f;
        [Tooltip("Force multiplier from Physics/Gravity when grounded")]
        public float stickToGroundForce = 1;
        [Tooltip("Force multiplier from Physics/Gravity.")]
        public float gravityMultiply = 1;
        [Tooltip("The amount of force applied when jumping.")]
        public float jumpHeight = 3;
        [Tooltip("Camera offset from the player.")]
        public Vector3 offset = new(0, -0.2f, 0);
        [Tooltip("Max speed the player can reach while falling")]
        public float maxFallSpeed = 100;
    }

    public partial class FpsController
    {

        // 检查站起来是否有足够的空间
        private bool CheckStandUpSpace()
        {
            var origin = transform.position + (Vector3.up * (_unityCharacterController.height * 0.5f)); // 从角色头顶位置发射射线
            float maxDistance = _unityCharacterController.height * 0.5f; // 射线的最大距离为角色高度的一半

            // 检查是否有碰撞体阻挡
            return !Physics.Raycast(origin, Vector3.up, maxDistance);
        }

        // 尝试站起来
        public void TryStandUp()
        {
            if (!CheckStandUpSpace()) // 如果头顶有障碍物
            {
                Debug.Log("Cannot stand up, not enough space above.");
                return; // 无法站起来
            }

            // 如果头顶没有障碍物，就站起来
            _unityCharacterController.height = defaultHeight;
            _unityCharacterController.center = new Vector3(_unityCharacterController.center.x, defaultHeight * 0.5f, _unityCharacterController.center.z);
        }
    }

    public partial class FpsController
    {
        public void SetupControllerPosition(Vector3 position)
        {
            _unityCharacterController.enabled = false;
            _selfTsf.position = position;
            _unityCharacterController.enabled = true;
        }

        public void SetupControllerRotation(Quaternion rotation) => _selfTsf.rotation = rotation;

        public void SetupControllerPositionAndRotation(Vector3 position, Quaternion rotation) => _selfTsf.SetPositionAndRotation(position, rotation);

        public void SetupSnappingRootPosition(Vector3 position)
        {
            var newPos = position - SnappingRoot.localPosition;
            SetupControllerPosition(newPos);
        }

        public void SetupSnappingRootPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            var newPos = position - SnappingRoot.localPosition;
            SetupControllerPositionAndRotation(newPos, rotation);
        }
    }
}
