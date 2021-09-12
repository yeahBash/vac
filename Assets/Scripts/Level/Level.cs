using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vac.Level
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Level")]
    public class Level : ScriptableObject
    {
        public Sprite Core;
        public BranchParameters[] Branches;
    }

    [System.Serializable]
    public struct BranchParameters
    {
        public Sprite Body;
        public Sprite Top;
        public int Size;
        public int AnglePosition;
    }
}