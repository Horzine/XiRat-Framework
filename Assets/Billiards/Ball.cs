using System.Collections;
using System.Collections.Generic;
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

        private void Awake()
        {
            _rig = GetComponent<Rigidbody>();

        }
        private void Start()
        {
            var table = FindObjectOfType<Table>();
            transform.position = table.ProjectPointOnOBB(transform.position) + new Vector3(0, BallRadius, 0);

        }

        private void OnTriggerEnter(Collider other)
        {
            XiLogger.CallMark();
        }

        private void OnTriggerExit(Collider other)
        {
            XiLogger.CallMark();
        }

        private void Update()
        {
            transform.Translate(CurrentVelocity * Time.deltaTime);
        }
    }
}
