using Branch;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Level")]
    public class LevelBase : ScriptableObject
    {
        public Vector2 CorePosition;
        public Vector2[] DestroyerPositions;
        public float RotationSpeed;
        public BranchBaseParameters[] Branches;
    }
}