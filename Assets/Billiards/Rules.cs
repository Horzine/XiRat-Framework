using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xi_
{
    public static class Rules
    {
        public static void TableBoardReflectBall(Ball ball, Vector3 _boardNormal)
        {
            Vector3 normal = _boardNormal;
            Vector3 incomingVector = ball.CurrentVelocity;
            Vector3 reflectedVector = Vector3.Reflect(incomingVector, normal.normalized);
            ball._direction = reflectedVector;
        }
    }
}
