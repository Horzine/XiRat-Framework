using UnityEngine;
using Xi.Tools;

namespace Xi_
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public float BallRadius => 0.1f;
        private Rigidbody _rig;
        public Vector3 _direction;
        public float _speedValue;
        public Vector3 CurrentVelocity => _speedValue * _direction.normalized;
        private BallCollisionHandler _collisionHandler;


        public void SetNewVelocity(Vector3 newVelocity)
        {
            _speedValue = newVelocity.magnitude;
            _direction = newVelocity.normalized;
        }

        private void Awake()
        {
            _rig = GetComponent<Rigidbody>();
            _collisionHandler = FindObjectOfType<BallCollisionHandler>();
        }

        private void Start()
        {
            var table = FindObjectOfType<Table>();
            transform.position = table.ProjectPointOnOBB(transform.position) + new Vector3(0, BallRadius, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent<Ball>(out var ball))
            {
                _collisionHandler.OnTriggerEnterBall(this, ball);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // XiLogger.CallMark();
        }

        private void Update() => transform.Translate(CurrentVelocity * Time.deltaTime);
    }
}
