using System;
using UnityEngine;

namespace Xi_
{
    public enum BoardDirectionEnum
    {
        Forward,
        Backward,
        Left,
        Right,
    }
    public class TableBoard : MonoBehaviour
    {
        [SerializeField] private BoardDirectionEnum _BoardDirection;

        private Vector3 _boardNormalDirection;
        private Action<BoardDirectionEnum, Ball, Vector3> _onTriggerEnterBallAction;
        private Action<BoardDirectionEnum, Ball> _onTriggerExitBallAction;

        public float bounceForce = 10f;

        public void Init(Action<BoardDirectionEnum, Ball, Vector3> onTriggerEnterBallAction,
            Action<BoardDirectionEnum, Ball> onTriggerExitBallAction,
            Vector3 boardToTable)
        {
            _onTriggerEnterBallAction = onTriggerEnterBallAction;
            _onTriggerExitBallAction = onTriggerExitBallAction;
            _boardNormalDirection = boardToTable.normalized;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent<Ball>(out var ball))
            {
                _onTriggerEnterBallAction?.Invoke(_BoardDirection, ball, _boardNormalDirection);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent<Ball>(out var ball))
            {
                _onTriggerExitBallAction?.Invoke(_BoardDirection, ball);
            }
        }
    }
}
