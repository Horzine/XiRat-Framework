using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xi_
{
    public class BallCollisionHandler : MonoBehaviour
    {
        private readonly HashSet<(Ball, Ball)> _cachedTriggerEnterBall = new();
        private readonly HashSet<(Ball, Vector3)> _cachedTriggerEnterTableBoard = new();
        private Table _table;

        private void Awake()
        {
            _table = FindObjectOfType<Table>();
        }

        public void OnTriggerEnterBall(Ball triggered, Ball other)
        {
            var invert = (other, triggered);
            if (_cachedTriggerEnterBall.Contains(invert))
            {
                return;
            }

            var info = (triggered, other);
            if (_cachedTriggerEnterBall.Contains(info))
            {
                return;
            }

            _cachedTriggerEnterBall.Add(invert);
        }

        public void OnTriggerEnterTableBoard(Ball ballObj, Vector3 boardNormal)
        {
            _cachedTriggerEnterTableBoard.Add((ballObj, boardNormal));
        }

        private void LateUpdate()
        {
            foreach (var (ballA, ballB) in _cachedTriggerEnterBall)
            {
                SimulateBallCollision(ballA, ballB);
            }
            _cachedTriggerEnterBall.Clear();

            foreach (var (ball, boardNormal) in _cachedTriggerEnterTableBoard)
            {
                SimulateTableBoardCollision(ball, boardNormal);
                ball.transform.position = _table.ProjectPointOnOBB(ball.transform.position) + new Vector3(0, ball.BallRadius, 0);
            }
            _cachedTriggerEnterTableBoard.Clear();
        }

        private void SimulateBallCollision(Ball ballA, Ball ballB)
        {
            // 获取球的质量
            float mass1 = 1;
            float mass2 = 1;

            // 获取球的速度向量
            Vector3 velocity1 = ballA.CurrentVelocity;
            Vector3 velocity2 = ballB.CurrentVelocity;

            // 计算碰撞点的法线方向
            Vector3 collisionNormal = (ballA.transform.position - ballB.transform.position).normalized;

            // 计算碰撞点的切线方向
            Vector3 collisionTangent = Vector3.Cross(Vector3.up, collisionNormal).normalized;

            // 计算速度向量在法线方向和切线方向上的投影
            float velocity1Normal = Vector3.Dot(velocity1, collisionNormal);
            float velocity1Tangent = Vector3.Dot(velocity1, collisionTangent);
            float velocity2Normal = Vector3.Dot(velocity2, collisionNormal);
            float velocity2Tangent = Vector3.Dot(velocity2, collisionTangent);

            // 计算沿法线方向的动量守恒
            float newVelocity1Normal = (mass1 - mass2) / (mass1 + mass2) * velocity1Normal + 2 * mass2 / (mass1 + mass2) * velocity2Normal;
            float newVelocity2Normal = 2 * mass1 / (mass1 + mass2) * velocity1Normal + (mass2 - mass1) / (mass1 + mass2) * velocity2Normal;

            // 计算沿切线方向的速度不变
            float newVelocity1Tangent = velocity1Tangent;
            float newVelocity2Tangent = velocity2Tangent;

            // 计算碰撞后的速度向量
            Vector3 newVelocity1 = newVelocity1Normal * collisionNormal + newVelocity1Tangent * collisionTangent;
            Vector3 newVelocity2 = newVelocity2Normal * collisionNormal + newVelocity2Tangent * collisionTangent;

            // 更新球的速度
            ballA.SetNewVelocity(newVelocity1);
            ballB.SetNewVelocity(newVelocity2);
        }

        private void SimulateTableBoardCollision(Ball ball, Vector3 boardNormal)
        {
            Rules.TableBoardReflectBall(ball, boardNormal);
        }
    }
}
