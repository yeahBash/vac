using UnityEngine;
using Vac.Branch;

namespace Vac.Level
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Level")]
    public class Level : ScriptableObject
    {
        public Vector2 DestroyerPos;
        public float RotationSpeed;
        public BranchParameters[] Branches;
    }
}