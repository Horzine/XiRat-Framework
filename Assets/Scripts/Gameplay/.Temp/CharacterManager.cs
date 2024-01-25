using UnityEngine;
using UnityEngine.Events;
/*
CharacterManager 类
这个类是角色管理器，负责处理角色的各种属性和行为。

onJump: 角色跳跃事件。
onLand: 角色落地事件。
Setup 方法：设置角色的各种属性，如演员、角色控制器、相机管理器等。
UpdateGroundedState(bool value): 更新角色是否着地的状态。
Update(): 更新角色的状态，包括是否着地、速度等。
AddLookValue(float vertical, float horizontal): 添加角色的视角偏移量。
IsVelocityZero(): 检查角色的速度是否为零。
*/
namespace Xi.Gameplay.Character
{
    [RequireComponent(typeof(CharacterInput))]
    public class CharacterManager : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent onJump = new();
        public UnityEvent onLand = new();

        public Actor actor { get; set; }
        public CharacterInput characterInput { get; set; }
        public CharacterController characterController { get; set; }
        public new Rigidbody rigidbody { get; set; }
        //public CameraManager cameraManager { get; set; }
        //public AudioFiltersManager audioFiltersManager { get; set; }
        public Transform orientation { get; set; }
        public ICharacterController character { get; set; }
        public Vector3 velocity { get; private set; }
        public bool isGrounded { get; set; }
        private bool previouslyGrounded;

        private void Start()
        {
            character = GetComponent<ICharacterController>();
            actor = GetComponent<Actor>();
            characterInput = GetComponent<CharacterInput>();
            characterController = GetComponent<CharacterController>();
            cameraManager = transform.SearchFor<CameraManager>();
            //audioFiltersManager = transform.SearchFor<AudioFiltersManager>();
        }

        public void Setup(Actor _actor, CharacterController _characterController, CameraManager _cameraManager, Transform _orientation)
        {
            actor = _actor;
            characterController = _characterController;
            cameraManager = _cameraManager;
            orientation = _orientation;
        }

        public void Setup(Actor _actor, Rigidbody _rigidbody, CameraManager _cameraManager, Transform _orientation)
        {
            actor = _actor;
            rigidbody = _rigidbody;
            cameraManager = _cameraManager;
            orientation = _orientation;
        }

        /// <summary>
        /// Sets isGrounded to value. Use this for rigidbody controllers.
        /// </summary>
        /// <param name="value"></param>
        public void UpdateGroundedState(bool value) => isGrounded = value;

        private void Update()
        {
            if (characterController)
            {
                isGrounded = characterController.isGrounded;
                velocity = characterController.velocity;
            }
            else if (rigidbody)
            {
                velocity = rigidbody.velocity;
            }

            if (previouslyGrounded && !isGrounded)
            {
                onJump?.Invoke();
            }

            if (!previouslyGrounded && isGrounded)
            {
                onLand?.Invoke();
            }

            previouslyGrounded = isGrounded;
        }

        public virtual void AddLookValue(float vertical, float horizontal) => characterInput.AddLookValue(new Vector2(vertical, horizontal));

        public virtual bool IsVelocityZero()
        {
            return characterController
                ? characterController.IsVelocityZero()
                : !characterController && rigidbody ? rigidbody.IsVelocityZero() : true;
        }
    }
}