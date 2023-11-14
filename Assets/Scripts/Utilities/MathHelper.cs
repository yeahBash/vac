using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public static class MathHelper
    {
        public static Vector2 Project(Vector2 vector, Vector2 onNormal, out float projectionMultiplier)
        {
            projectionMultiplier = 0f;

            var onNormalSqrMagnitude = onNormal.sqrMagnitude;
            if (onNormalSqrMagnitude < (double)Mathf.Epsilon)
                return Vector2.zero;

            var dot = Vector2.Dot(vector, onNormal);
            projectionMultiplier = dot / onNormalSqrMagnitude;
            return onNormal * projectionMultiplier;
        }

        public static Vector2 GetNormal(Vector2 vector, Vector2 onNormal, out float projectionMultiplier)
        {
            return vector - Project(vector, onNormal, out projectionMultiplier);
        }

        public static float GetRandomValue(float minValue, float maxValue)
        {
            return minValue + Random.value * (maxValue - minValue);
        }

        public static int GetRandomValue(int minValue, int maxValue)
        {
            return (int) Math.Round(GetRandomValue((float)minValue, (float)maxValue));
        }

        public static Vector2 MoveTowards(Vector2 curPos, Vector2 target, float speed, float deltaTime)
        {
            var moveVector = target - curPos;
            var step = moveVector.normalized * (speed * deltaTime);
            var newPos = curPos + step;
            var newMoveVector = target - newPos;

            if (Vector2.Dot(moveVector, newMoveVector) <= 0f)
                newPos = target;

            return newPos;
        }
    }
}