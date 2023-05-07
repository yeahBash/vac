using System.Linq;
using Core;
using UnityEngine;

namespace Variants.Virus
{
    public class Envelope : CoreBase
    {
        private static readonly int OffsetProperty = Shader.PropertyToID("_Offset");

        public void ChangeShape(float offset)
        {
            CoreRenderer.material.SetFloat(OffsetProperty, offset);
        }

        public void ChangeShape()
        {
            ChangeShape(Random.value);
        }

        protected override void AffectBranches(float deltaTime)
        {
            foreach (var branch in Branches)
                branch.Length += branch.GrowSpeed * deltaTime;
        }
    }
}