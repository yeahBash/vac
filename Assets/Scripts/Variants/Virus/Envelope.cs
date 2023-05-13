using Core;
using UnityEngine;

namespace Variants.Virus
{
    public class Envelope : CoreBase
    {
        private static readonly int OffsetProperty = Shader.PropertyToID("_Offset");

        public float OffsetPerSec = 10f;

        protected override void Update()
        {
            base.Update();

            if (Input.GetMouseButton(0)) ChangeShape();
        }

        private void ChangeShape(float offset)
        {
            CoreRenderer.material.SetFloat(OffsetProperty, offset);
        }

        private void ChangeShape()
        {
            var current = CoreRenderer.material.GetFloat(OffsetProperty);
            ChangeShape(current + OffsetPerSec * Time.deltaTime);
        }

        protected override void AffectBranches(float deltaTime)
        {
            foreach (var branch in ActiveBranches)
                branch.Length += branch.GrowSpeed * deltaTime;
        }
    }
}