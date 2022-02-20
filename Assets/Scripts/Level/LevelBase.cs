using Arm;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Level")]
    public class LevelBase : ScriptableObject
    {
        public Vector2 DestroyerPos;
        public float RotationSpeed;
        public ArmParameters[] Arms;
    }
}