using UnityEngine;

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
    }
}